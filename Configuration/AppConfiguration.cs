namespace Windows.Configurations.Configuration
{
    public class AppConfiguration
    {
        public Audio.AudioSettings Audio { get; set; } = new Audio.AudioSettings();

        public General.GeneralSettings General { get; set; } = new General.GeneralSettings();

        public void EnsureDefaults()
        {
            Audio ??= new Audio.AudioSettings();
            Audio.Devices ??= new Audio.AudioDevicesSettings();
            Audio.Devices.Playback ??= [];
            Audio.Devices.Recording ??= [];
            General ??= new General.GeneralSettings();
        }
    }
}