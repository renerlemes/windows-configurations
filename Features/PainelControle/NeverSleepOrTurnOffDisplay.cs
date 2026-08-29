using Microsoft.Win32;
using System;
using System.Diagnostics;

namespace Windows.Configurations.Features.PainelControle
{
    public class NeverSleepOrTurnOffDisplay : IWindowsAction
    {
        private const string ActiveSchemePath = @"SYSTEM\CurrentControlSet\Control\Power\User\PowerSchemes";

        private const string VideoSubGuid = "7516b95f-f776-4464-8c53-06167f40cc99";
        private const string VideoIdleGuid = "3c0bc021-c8a8-4e07-a973-6b14cbcb2b7e";

        private const string SleepSubGuid = "238c9fa8-0aad-41ed-83f4-97be242c8f20";
        private const string StandbyIdleGuid = "29f6c1db-86da-48c5-9fdb-f2b67b1f44da";

        // 0 = Nunca
        private const int Never = 0;

        // Padrão típico do plano Equilibrado (segundos)
        private const int DefaultVideoAc = 600;   // 10 min
        private const int DefaultVideoDc = 300;   // 5 min
        private const int DefaultSleepAc = 1800;  // 30 min
        private const int DefaultSleepDc = 900;   // 15 min

        public string Name => "Vídeo e suspensão";

        public string Description => "Desliga o vídeo e suspende o computador: Nunca (bateria e conectado)";

        public bool Get()
        {
            if (!TryReadSetting(VideoSubGuid, VideoIdleGuid, out int videoAc, out int videoDc))
                return false;

            if (!TryReadSetting(SleepSubGuid, StandbyIdleGuid, out int sleepAc, out int sleepDc))
                return false;

            return videoAc == Never
                && videoDc == Never
                && sleepAc == Never
                && sleepDc == Never;
        }

        public void Execute()
        {
            SetTimeouts(Never, Never, Never, Never);
        }

        public void Undo()
        {
            SetTimeouts(DefaultVideoAc, DefaultVideoDc, DefaultSleepAc, DefaultSleepDc);
        }

        private static bool TryReadSetting(string subGuid, string settingGuid, out int ac, out int dc)
        {
            ac = -1;
            dc = -1;

            using RegistryKey schemes = Registry.LocalMachine.OpenSubKey(ActiveSchemePath);

            if (schemes?.GetValue("ActivePowerScheme") is not string activeScheme)
                return false;

            string settingPath = $@"{ActiveSchemePath}\{activeScheme}\{subGuid}\{settingGuid}";

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

        private static void SetTimeouts(int videoAc, int videoDc, int sleepAc, int sleepDc)
        {
            RunPowerCfg($"/setacvalueindex SCHEME_CURRENT SUB_VIDEO VIDEOIDLE {videoAc}");
            RunPowerCfg($"/setdcvalueindex SCHEME_CURRENT SUB_VIDEO VIDEOIDLE {videoDc}");
            RunPowerCfg($"/setacvalueindex SCHEME_CURRENT SUB_SLEEP STANDBYIDLE {sleepAc}");
            RunPowerCfg($"/setdcvalueindex SCHEME_CURRENT SUB_SLEEP STANDBYIDLE {sleepDc}");
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
