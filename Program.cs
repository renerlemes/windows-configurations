using System;
using System.Threading;
using System.Windows.Forms;

namespace Windows.Configurations
{
    internal static class Program
    {
        private const string MutexName = @"Local\Windows.Configurations";

        [STAThread]
        static int Main(string[] args)
        {
            // A instância elevada aplica uma ação e sai: não abre janela nem disputa o mutex.
            if (ElevatedAction.TryHandleCommandLine(args, out int actionExitCode))
                return actionExitCode;

            ApplicationConfiguration.Initialize();

            using Mutex mutex = new(true, MutexName, out bool createdNew);

            if (!createdNew)
            {
                MessageBox.Show(
                    "O Windows Configurations já está em execução.",
                    "Windows Configurations",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return 0;
            }

            Application.Run(new frmDefault());

            return 0;
        }
    }
}
