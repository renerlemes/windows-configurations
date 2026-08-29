using Microsoft.Win32;
using System;
using System.Diagnostics;

namespace Windows.Configurations.Features.PainelControle
{
    public class LidCloseDoNothing : IWindowsAction
    {
        private const string ActiveSchemePath = @"SYSTEM\CurrentControlSet\Control\Power\User\PowerSchemes";
        private const string ButtonsGuid = "4f971e89-eebd-4455-a8de-9e59040e7347";
        private const string LidActionGuid = "5ca83367-6e45-459f-a27b-476b1d01c936";

        // 0 = Nada a fazer, 1 = Suspender (padrão típico do Windows)
        private const int DoNothing = 0;
        private const int Sleep = 1;

        public string Name => "Fechar a tampa";

        public string Description => "Ao fechar a tampa, não faz nada (bateria e conectado)";

        public bool Get()
        {
            if (!TryReadLidAction(out int ac, out int dc))
                return false;

            return ac == DoNothing && dc == DoNothing;
        }

        public void Execute()
        {
            SetLidAction(DoNothing);
        }

        public void Undo()
        {
            SetLidAction(Sleep);
        }

        private static bool TryReadLidAction(out int ac, out int dc)
        {
            ac = -1;
            dc = -1;

            using RegistryKey schemes = Registry.LocalMachine.OpenSubKey(ActiveSchemePath);

            if (schemes?.GetValue("ActivePowerScheme") is not string activeScheme)
                return false;

            string settingPath = $@"{ActiveSchemePath}\{activeScheme}\{ButtonsGuid}\{LidActionGuid}";

            using RegistryKey setting = Registry.LocalMachine.OpenSubKey(settingPath);

            if (setting is null)
                return false;

            object acValue = setting.GetValue("ACSettingIndex");
            object dcValue = setting.GetValue("DCSettingIndex");

            if (acValue is null || dcValue is null)
                return false;

            ac = Convert.ToInt32(acValue);
            dc = Convert.ToInt32(dcValue);
            return true;
        }

        private static void SetLidAction(int value)
        {
            RunPowerCfg($"/setacvalueindex SCHEME_CURRENT SUB_BUTTONS LIDACTION {value}");
            RunPowerCfg($"/setdcvalueindex SCHEME_CURRENT SUB_BUTTONS LIDACTION {value}");
            RunPowerCfg("/setactive SCHEME_CURRENT");
        }

        private static void RunPowerCfg(string arguments)
        {
            using Process process = Process.Start(new ProcessStartInfo
            {
                FileName = "powercfg",
                Arguments = arguments,
                CreateNoWindow = true,
                UseShellExecute = false
            });

            process?.WaitForExit(5000);
        }
    }
}
