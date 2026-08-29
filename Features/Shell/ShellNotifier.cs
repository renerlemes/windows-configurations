using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Windows.Configurations.Features.Shell
{
    internal static class ShellNotifier
    {
        private static readonly IntPtr HWND_BROADCAST = new IntPtr(0xFFFF);
        private const uint WM_SETTINGCHANGE = 0x001A;

        /// <summary>
        /// Envia WM_SETTINGCHANGE sem aguardar resposta. O broadcast síncrono alcança a própria
        /// janela do aplicativo e travaria a thread de interface enquanto ela processa o clique.
        /// </summary>
        public static void NotifySettingChange(string section)
        {
            SendNotifyMessage(HWND_BROADCAST, WM_SETTINGCHANGE, UIntPtr.Zero, section);
        }

        public static void RestartExplorer()
        {
            foreach (Process process in Process.GetProcessesByName("explorer"))
            {
                try
                {
                    process.Kill();
                    process.WaitForExit(3000);
                }
                catch
                {
                }
            }

            // O Windows costuma reiniciar o shell sozinho; iniciar outro abriria uma janela do Explorer.
            if (Process.GetProcessesByName("explorer").Length > 0)
                return;

            Process.Start(new ProcessStartInfo
            {
                FileName = "explorer.exe",
                UseShellExecute = true
            });
        }

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool SendNotifyMessage(
            IntPtr hWnd,
            uint msg,
            UIntPtr wParam,
            string lParam);
    }
}
