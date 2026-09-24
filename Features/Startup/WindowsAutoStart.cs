using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Windows.Configurations.Features.Startup
{
    /// <summary>
    /// O aplicativo roda sem elevação, então a chave Run do usuário resolve e continua
    /// funcionando em máquina gerenciada. A tarefa agendada usada antes precisava de
    /// privilégio para ser registrada e não subia quando o usuário não era administrador.
    /// </summary>
    internal static class WindowsAutoStart
    {
        private const string RunPath = @"Software\Microsoft\Windows\CurrentVersion\Run";
        private const string ValueName = "Windows Configurations";
        private const string LegacyTaskName = "Windows Configurations";

        private static bool _legacyTaskChecked;

        public static void SetEnabled(bool enabled)
        {
            RemoveLegacyTask();

            using RegistryKey key = Registry.CurrentUser.CreateSubKey(RunPath)
                ?? throw new InvalidOperationException("Não foi possível abrir a chave de inicialização do Windows.");

            if (enabled)
                key.SetValue(ValueName, $"\"{Application.ExecutablePath}\"", RegistryValueKind.String);
            else
                key.DeleteValue(ValueName, throwOnMissingValue: false);
        }

        /// <summary>
        /// Sem isso a tarefa antiga continuaria tentando subir o app em paralelo no logon.
        /// Só funciona se o usuário for administrador; quando não é, a tarefa também não
        /// consegue iniciar o app, então falhar aqui é inofensivo.
        /// </summary>
        private static void RemoveLegacyTask()
        {
            if (_legacyTaskChecked)
                return;

            _legacyTaskChecked = true;

            try
            {
                using Process process = Process.Start(new ProcessStartInfo
                {
                    FileName = "schtasks",
                    Arguments = $"/Delete /TN \"{LegacyTaskName}\" /F",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true
                });

                process?.WaitForExit(5000);
            }
            catch (Exception ex) when (ex is System.ComponentModel.Win32Exception or InvalidOperationException)
            {
            }
        }
    }
}
