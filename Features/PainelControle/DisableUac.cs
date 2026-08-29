using Microsoft.Win32;
using System;

namespace Windows.Configurations.Features.PainelControle
{
    public class DisableUac : IWindowsAction
    {
        private const string RegistryPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System";
        private const string ConsentPrompt = "ConsentPromptBehaviorAdmin";
        private const string SecureDesktop = "PromptOnSecureDesktop";

        // Padrão do Windows: notificar apenas quando apps tentarem alterar o computador
        private const int DefaultConsentPrompt = 5;
        private const int DefaultSecureDesktop = 1;

        public string Name => "Desativar UAC";

        public string Description => "Define o Controle de Conta de Usuário como Nunca notificar";

        public void Execute()
        {
            using RegistryKey key = Registry.LocalMachine.OpenSubKey(RegistryPath, true);

            key?.SetValue(ConsentPrompt, 0, RegistryValueKind.DWord);
            key?.SetValue(SecureDesktop, 0, RegistryValueKind.DWord);
        }

        public bool Get()
        {
            using RegistryKey key = Registry.LocalMachine.OpenSubKey(RegistryPath);

            if (key is null)
                return false;

            return ReadDword(key, ConsentPrompt) == 0 && ReadDword(key, SecureDesktop) == 0;
        }

        public void Undo()
        {
            using RegistryKey key = Registry.LocalMachine.OpenSubKey(RegistryPath, true);

            key?.SetValue(ConsentPrompt, DefaultConsentPrompt, RegistryValueKind.DWord);
            key?.SetValue(SecureDesktop, DefaultSecureDesktop, RegistryValueKind.DWord);
        }

        private static int? ReadDword(RegistryKey key, string name) => key.GetValue(name) is object value ? Convert.ToInt32(value) : null;
    }
}
