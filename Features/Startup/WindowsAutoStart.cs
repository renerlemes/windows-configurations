using System;
using System.Diagnostics;
using System.IO;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Windows.Configurations.Features.Startup
{
    /// <summary>
    /// O aplicativo exige administrador: a chave Run do Registro não eleva no logon.
    /// Uma tarefa ONLOGON com RunLevel HighestAvailable inicia sem prompt de UAC.
    /// A tarefa é registrada por XML porque os padrões que o schtasks aplica quando a
    /// tarefa é criada por linha de comando impedem o início na bateria e encerram o
    /// processo depois de 72 horas.
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
            string xmlPath = Path.Combine(Path.GetTempPath(), "Windows.Configurations.AutoStart.xml");

            // O schtasks só lê o XML em UTF-16 com BOM.
            File.WriteAllText(xmlPath, BuildTaskXml(), new UnicodeEncoding(false, true));

            try
            {
                Run("schtasks", $"/Create /TN \"{TaskName}\" /XML \"{xmlPath}\" /F", allowNonZeroExit: false);
            }
            finally
            {
                try
                {
                    File.Delete(xmlPath);
                }
                catch (IOException)
                {
                }
            }
        }

        private static void Delete()
        {
            Run("schtasks", $"/Delete /TN \"{TaskName}\" /F", allowNonZeroExit: true);
        }

        /// <summary>
        /// A ordem dos elementos segue o esquema de tarefas do Windows: fora dela o schtasks recusa o XML.
        /// </summary>
        private static string BuildTaskXml()
        {
            string exe = Application.ExecutablePath;
            string command = SecurityElement.Escape(exe);
            string workingDirectory = SecurityElement.Escape(Path.GetDirectoryName(exe));
            string user = SecurityElement.Escape($"{Environment.UserDomainName}\\{Environment.UserName}");

            return $"""
                <?xml version="1.0" encoding="UTF-16"?>
                <Task version="1.2" xmlns="http://schemas.microsoft.com/windows/2004/02/mit/task">
                  <RegistrationInfo>
                    <Description>Inicia o Windows Configurations no logon.</Description>
                  </RegistrationInfo>
                  <Triggers>
                    <LogonTrigger>
                      <Enabled>true</Enabled>
                      <UserId>{user}</UserId>
                      <Delay>PT15S</Delay>
                    </LogonTrigger>
                  </Triggers>
                  <Principals>
                    <Principal id="Author">
                      <UserId>{user}</UserId>
                      <LogonType>InteractiveToken</LogonType>
                      <RunLevel>HighestAvailable</RunLevel>
                    </Principal>
                  </Principals>
                  <Settings>
                    <MultipleInstancesPolicy>IgnoreNew</MultipleInstancesPolicy>
                    <DisallowStartIfOnBatteries>false</DisallowStartIfOnBatteries>
                    <StopIfGoingOnBatteries>false</StopIfGoingOnBatteries>
                    <AllowHardTerminate>true</AllowHardTerminate>
                    <StartWhenAvailable>true</StartWhenAvailable>
                    <RunOnlyIfNetworkAvailable>false</RunOnlyIfNetworkAvailable>
                    <IdleSettings>
                      <StopOnIdleEnd>false</StopOnIdleEnd>
                      <RestartOnIdle>false</RestartOnIdle>
                    </IdleSettings>
                    <AllowStartOnDemand>true</AllowStartOnDemand>
                    <Enabled>true</Enabled>
                    <Hidden>false</Hidden>
                    <RunOnlyIfIdle>false</RunOnlyIfIdle>
                    <WakeToRun>false</WakeToRun>
                    <ExecutionTimeLimit>PT0S</ExecutionTimeLimit>
                    <Priority>7</Priority>
                  </Settings>
                  <Actions Context="Author">
                    <Exec>
                      <Command>{command}</Command>
                      <WorkingDirectory>{workingDirectory}</WorkingDirectory>
                    </Exec>
                  </Actions>
                </Task>
                """;
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

            // Os dois pipes são drenados em paralelo: esperar o schtasks com um deles cheio travaria os dois processos.
            Task<string> error = process.StandardError.ReadToEndAsync();
            Task<string> output = process.StandardOutput.ReadToEndAsync();

            process.WaitForExit();

            if (allowNonZeroExit || process.ExitCode == 0)
                return;

            string message = error.Result.Trim();

            if (string.IsNullOrEmpty(message))
                message = output.Result.Trim();

            throw new InvalidOperationException(
                string.IsNullOrEmpty(message)
                    ? $"Não foi possível atualizar a inicialização com o Windows (código {process.ExitCode})."
                    : message);
        }
    }
}
