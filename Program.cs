using System;
using System.Threading;
using System.Windows.Forms;

namespace SoundSwitch
{
    internal static class Program
    {
        private const string MutexName = @"Local\SoundSwitch";

        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            using Mutex mutex = new(true, MutexName, out bool createdNew);

            if (!createdNew)
            {
                MessageBox.Show(
                    "O SoundSwitch já está em execução.",
                    "SoundSwitch",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            Application.Run(new frmDefault());
        }
    }
}
