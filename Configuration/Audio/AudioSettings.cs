namespace SoundSwitch.Configuration.Audio
{
    public class AudioSettings
    {
        public bool MuteOnLock { get; set; }

        public bool ShowNotificationOnDeviceChange { get; set; } = true;

        public AudioDevicesSettings Devices { get; set; } = new AudioDevicesSettings();
    }
}
