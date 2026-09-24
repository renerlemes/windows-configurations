using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Security;
using System.Security.Principal;
using System.Windows.Forms;

namespace Windows.Configurations
{
    /// <summary>
    /// As ações que gravam em HKLM não rodam no processo principal, que é asInvoker.
    /// O app sobe uma instância elevada de si mesmo para aplicar uma única ação e sair.
    /// </summary>
    internal static class ElevatedAction
    {
        private const string ActionSwitch = "--action";
        private const string ExecuteSwitch = "--execute";
        private const string UndoSwitch = "--undo";

        // Código de saída da instância elevada, lido pelo processo principal.
        private const int ExitApplied = 0;
        private const int ExitFailed = 1;
        private const int ExitInvalidArguments = 2;
        private const int ExitDenied = 3;

        // 1223 = ERROR_CANCELLED: o usuário recusou o prompt do UAC.
        private const int ErrorCancelled = 1223;

        public static bool IsElevated { get; } = CheckElevated();

        /// <summary>
        /// Chamado no início do Main. Na instância elevada aplica a ação pedida e devolve
        /// true para o processo encerrar sem abrir a interface.
        /// </summary>
        public static bool TryHandleCommandLine(string[] args, out int exitCode)
        {
            exitCode = ExitApplied;

            if (args is null || args.Length < 3)
                return false;

            if (!string.Equals(args[0], ActionSwitch, StringComparison.OrdinalIgnoreCase))
                return false;

            IWindowsAction action = Actions.Find(args[1]);

            if (action is null)
            {
                exitCode = ExitInvalidArguments;
                return true;
            }

            bool enable = string.Equals(args[2], ExecuteSwitch, StringComparison.OrdinalIgnoreCase);

            if (!enable && !string.Equals(args[2], UndoSwitch, StringComparison.OrdinalIgnoreCase))
            {
                exitCode = ExitInvalidArguments;
                return true;
            }

            try
            {
                if (enable)
                    action.Execute();
                else
                    action.Undo();
            }
            catch (Exception ex) when (ex is UnauthorizedAccessException or SecurityException)
            {
                // Já está elevado: negar aqui significa política da máquina, não falta de privilégio.
                exitCode = ExitDenied;
            }
            catch (Exception)
            {
                exitCode = ExitFailed;
            }

            return true;
        }

        public static ActionResult Run(IWindowsAction action, bool enable)
        {
            string arguments = $"{ActionSwitch} {action.GetType().Name} {(enable ? ExecuteSwitch : UndoSwitch)}";

            try
            {
                using Process process = Process.Start(new ProcessStartInfo
                {
                    FileName = Application.ExecutablePath,
                    Arguments = arguments,

                    // Verb runas só vale com UseShellExecute: é o ShellExecute que dispara o UAC.
                    UseShellExecute = true,
                    Verb = "runas",
                    WindowStyle = ProcessWindowStyle.Hidden
                });

                if (process is null)
                    return ActionResult.Failed;

                process.WaitForExit();

                return process.ExitCode switch
                {
                    ExitApplied => ActionResult.Applied,
                    ExitDenied => ActionResult.Denied,
                    _ => ActionResult.Failed
                };
            }
            catch (Win32Exception ex) when (ex.NativeErrorCode == ErrorCancelled)
            {
                return ActionResult.Cancelled;
            }
            catch (Win32Exception)
            {
                // Elevação indisponível para esta conta: a política não permite elevar.
                return ActionResult.Denied;
            }
        }

        private static bool CheckElevated()
        {
            try
            {
                using WindowsIdentity identity = WindowsIdentity.GetCurrent();

                return new WindowsPrincipal(identity).IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
