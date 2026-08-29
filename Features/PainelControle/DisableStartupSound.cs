using Microsoft.Win32;
using System;

namespace Windows.Configurations.Features.PainelControle
{
    public class DisableStartupSound : IWindowsAction
    {
        private const string BootAnimationPath =
            @"SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\LogonUI\BootAnimation";

        private const string EditionOverridesPath =
            @"SOFTWARE\Microsoft\Windows\CurrentVersion\EditionOverrides";

        private const string DisableStartupSoundValue = "DisableStartupSound";
        private const string UserSettingDisableStartupSound = "UserSetting_DisableStartupSound";

        public string Name => "Desativar som na inicialização";

        public string Description => "Não tocar o som na inicialização do Windows";

        public bool Get()
        {
            using RegistryKey key = Registry.LocalMachine.OpenSubKey(BootAnimationPath);

            if (key?.GetValue(DisableStartupSoundValue) is not object value)
                return false;

            return Convert.ToInt32(value) == 1;
        }

        public void Execute()
        {
            SetStartupSoundDisabled(disabled: true);
        }

        public void Undo()
        {
            SetStartupSoundDisabled(disabled: false);
        }

        private static void SetStartupSoundDisabled(bool disabled)
        {
            int disableValue = disabled ? 1 : 0;

            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(BootAnimationPath, writable: true)
                                 ?? Registry.LocalMachine.CreateSubKey(BootAnimationPath))
            {
                key?.SetValue(DisableStartupSoundValue, disableValue, RegistryValueKind.DWord);
            }

            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(EditionOverridesPath, writable: true)
                                 ?? Registry.LocalMachine.CreateSubKey(EditionOverridesPath))
            {
                key?.SetValue(UserSettingDisableStartupSound, disableValue, RegistryValueKind.DWord);
            }
        }
    }
}
