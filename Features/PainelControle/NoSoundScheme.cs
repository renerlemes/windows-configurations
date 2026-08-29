using Microsoft.Win32;
using System;
using Windows.Configurations.Features.Shell;

namespace Windows.Configurations.Features.PainelControle
{
    public class NoSoundScheme : IWindowsAction
    {
        private const string SchemesPath = @"AppEvents\Schemes";
        private const string AppsPath = @"AppEvents\Schemes\Apps";
        private const string NoneScheme = ".None";
        private const string DefaultScheme = ".Default";

        public string Name => "Nenhum som";

        public string Description => "Define o esquema de som como Nenhum som";

        public bool Get()
        {
            using RegistryKey key = Registry.CurrentUser.OpenSubKey(SchemesPath);

            string current = key?.GetValue(null) as string;

            return string.Equals(current, NoneScheme, StringComparison.OrdinalIgnoreCase);
        }

        public void Execute()
        {
            ApplyScheme(NoneScheme);
        }

        public void Undo()
        {
            ApplyScheme(DefaultScheme);
        }

        private static void ApplyScheme(string schemeName)
        {
            using (RegistryKey schemes = Registry.CurrentUser.OpenSubKey(SchemesPath, writable: true))
            {
                schemes?.SetValue(null, schemeName);
            }

            using RegistryKey apps = Registry.CurrentUser.OpenSubKey(AppsPath);

            if (apps is null)
                return;

            foreach (string appName in apps.GetSubKeyNames())
            {
                using RegistryKey appKey = apps.OpenSubKey(appName);

                if (appKey is null)
                    continue;

                foreach (string eventName in appKey.GetSubKeyNames())
                {
                    string eventPath = $@"{AppsPath}\{appName}\{eventName}";

                    using RegistryKey schemeKey = Registry.CurrentUser.OpenSubKey($@"{eventPath}\{schemeName}");

                    // Só aplica quando o evento tem a subchave do esquema (mesmo comportamento do mmsys.cpl)
                    if (schemeKey is null)
                        continue;

                    string soundPath = schemeKey.GetValue(null) as string ?? string.Empty;

                    using RegistryKey currentKey = Registry.CurrentUser.CreateSubKey($@"{eventPath}\.Current");
                    currentKey?.SetValue(null, soundPath);
                }
            }

            ShellNotifier.NotifySettingChange("AppEvents");
        }
    }
}
