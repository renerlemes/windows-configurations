using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Windows.Configurations.Features.Startup
{
    /// <summary>
    /// O aplicativo exige administrador: a chave Run do Registro não eleva no logon.
    /// Uma tarefa ONLOGON com /RL HIGHEST inicia sem prompt de UAC.
    /// </summary>
    internal static class WindowsAutoStart
    {
        private const string TaskName = "Windows Configurations";

        public static void SetEnabled(bool enabled)
        {
            if (enabled)
                Create();
            else
                Delete();
        }

        private static void Create()
        {
            string exe = Application.ExecutablePath;
            string arguments = $"/Create /TN \"{TaskName}\" /TR \"\\\"{exe}\\\"\" /SC ONLOGON /RL HIGHEST /F";

            Run("schtasks", arguments, allowNonZeroExit: false);
        }

        private static void Delete()
        {
            Run("schtasks", $"/Delete /TN \"{TaskName}\" /F", allowNonZeroExit: true);
        }

        private static void Run(string fileName, string arguments, bool allowNonZeroExit)
        {
            using Process process = Process.Start(new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            });

            process.WaitForExit();

            if (allowNonZeroExit || process.ExitCode == 0)
                return;

            string error = process.StandardError.ReadToEnd().Trim();

            throw new InvalidOperationException(
                string.IsNullOrEmpty(error)
                    ? $"Não foi possível atualizar a inicialização com o Windows (código {process.ExitCode})."
                    : error);
        }
    }
}
