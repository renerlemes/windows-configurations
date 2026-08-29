using Microsoft.Win32;
using System;
using Windows.Configurations.Features.Shell;

namespace Windows.Configurations.Features.Personalizacao
{
    public class DisableItemsTaskbar : IWindowsAction
    {
        private const string SearchPath = @"Software\Microsoft\Windows\CurrentVersion\Search";
        private const string AdvancedPath = @"Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced";
        private const string ResumePath = @"Software\Microsoft\Windows\CurrentVersion\CrossDeviceResume\Configuration";

        public string Name => "Ocultar itens da barra de tarefas";

        public string Description => "Oculta Pesquisa e desativa Visão de tarefas, Widgets e Continuar";

        public bool Get()
        {
            return ReadDword(SearchPath, "SearchboxTaskbarMode", 1) == 0
                && ReadDword(AdvancedPath, "ShowTaskViewButton", 1) == 0
                && ReadDword(ResumePath, "IsResumeAllowed", 1) == 0;
        }

        public void Execute()
        {
            WriteDword(SearchPath, "SearchboxTaskbarMode", 0);
            WriteDword(AdvancedPath, "ShowTaskViewButton", 0);
            WriteDword(ResumePath, "IsResumeAllowed", 0);
            WriteDword(ResumePath, "IsOneDriveResumeAllowed", 0);

            RefreshTaskbar();
        }

        public void Undo()
        {
            WriteDword(SearchPath, "SearchboxTaskbarMode", 1);
            WriteDword(AdvancedPath, "ShowTaskViewButton", 1);
            WriteDword(ResumePath, "IsResumeAllowed", 1);
            WriteDword(ResumePath, "IsOneDriveResumeAllowed", 1);

            RefreshTaskbar();
        }

        private static int ReadDword(string path, string name, int defaultValue)
        {
            using RegistryKey key = Registry.CurrentUser.OpenSubKey(path);
            object value = key?.GetValue(name);
            return value is null ? defaultValue : Convert.ToInt32(value);
        }

        private static void WriteDword(string path, string name, int value)
        {
            using RegistryKey key = Registry.CurrentUser.CreateSubKey(path);
            key?.SetValue(name, value, RegistryValueKind.DWord);
        }

        private static void RefreshTaskbar()
        {
            ShellNotifier.NotifySettingChange("TraySettings");
            ShellNotifier.RestartExplorer();
        }
    }
}
