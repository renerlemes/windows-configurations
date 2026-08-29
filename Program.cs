using System;
using System.Threading;
using System.Windows.Forms;

namespace Windows.Configurations
{
    internal static class Program
    {
        private const string MutexName = @"Local\Windows.Configurations";

        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            using Mutex mutex = new(true, MutexName, out bool createdNew);

            if (!createdNew)
            {
                MessageBox.Show(
                    "O Windows Configurations já está em execução.",
                    "Windows Configurations",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            Application.Run(new frmDefault());
        }
    }
}