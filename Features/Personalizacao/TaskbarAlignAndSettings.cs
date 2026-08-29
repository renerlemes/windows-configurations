using Microsoft.Win32;
using System;
using Windows.Configurations.Features.Shell;

namespace Windows.Configurations.Features.Personalizacao
{
    public class TaskbarAlignAndSettings : IWindowsAction
    {
        private const string AdvancedPath = @"Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced";
        private const string StuckRectsPath = @"Software\Microsoft\Windows\CurrentVersion\Explorer\StuckRects3";

        public string Name => "Comportamentos da barra de tarefas";

        public string Description => "Alinha a barra à esquerda e aplica os comportamentos configurados";

        public bool Get()
        {
            return ReadDword("TaskbarAl", 1) == 0
                && !IsAutoHideEnabled()
                && ReadDword("TaskbarBadges", 1) == 1
                && ReadDword("TaskbarFlashing", 1) == 1
                && ReadDword("MMTaskbarEnabled", 1) == 1
                && ReadDword("MMTaskbarMode", 0) == 0
                && ReadDword("TaskbarSn", 1) == 1
                && ReadDword("TaskbarSd", 1) == 1
                && ReadDword("TaskbarGlomLevel", 0) == 0
                && ReadDword("MMTaskbarGlomLevel", 0) == 0;
        }

        public void Execute()
        {
            WriteDword("TaskbarAl", 0);
            WriteDword("TaskbarBadges", 1);
            WriteDword("TaskbarFlashing", 1);
            WriteDword("MMTaskbarEnabled", 1);
            WriteDword("MMTaskbarMode", 0);
            WriteDword("TaskbarSn", 1);
            WriteDword("TaskbarSd", 1);
            WriteDword("TaskbarGlomLevel", 0);
            WriteDword("MMTaskbarGlomLevel", 0);
            SetAutoHide(enabled: false);

            RefreshTaskbar();
        }

        public void Undo()
        {
            WriteDword("TaskbarAl", 1);
            WriteDword("TaskbarBadges", 1);
            WriteDword("TaskbarFlashing", 1);
            WriteDword("MMTaskbarEnabled", 1);
            WriteDword("MMTaskbarMode", 0);
            WriteDword("TaskbarSn", 1);
            WriteDword("TaskbarSd", 1);
            WriteDword("TaskbarGlomLevel", 0);
            WriteDword("MMTaskbarGlomLevel", 0);
            SetAutoHide(enabled: false);

            RefreshTaskbar();
        }

        private static bool IsAutoHideEnabled()
        {
            using RegistryKey key = Registry.CurrentUser.OpenSubKey(StuckRectsPath);

            if (key?.GetValue("Settings") is not byte[] settings || settings.Length < 9)
                return false;

            return (settings[8] & 0x01) != 0;
        }

        private static void SetAutoHide(bool enabled)
        {
            using RegistryKey key = Registry.CurrentUser.OpenSubKey(StuckRectsPath, writable: true);

            if (key?.GetValue("Settings") is not byte[] settings || settings.Length < 9)
                return;

            byte[] updated = (byte[])settings.Clone();
            updated[8] = enabled
                ? (byte)(updated[8] | 0x01)
                : (byte)(updated[8] & ~0x01);

            key.SetValue("Settings", updated, RegistryValueKind.Binary);
        }

        private static int ReadDword(string name, int defaultValue)
        {
            using RegistryKey key = Registry.CurrentUser.OpenSubKey(AdvancedPath);
            object value = key?.GetValue(name);
            return value is null ? defaultValue : Convert.ToInt32(value);
        }

        private static void WriteDword(string name, int value)
        {
            using RegistryKey key = Registry.CurrentUser.CreateSubKey(AdvancedPath);
            key?.SetValue(name, value, RegistryValueKind.DWord);
        }

        private static void RefreshTaskbar()
        {
            ShellNotifier.NotifySettingChange("TraySettings");
            ShellNotifier.RestartExplorer();
        }
    }
}
