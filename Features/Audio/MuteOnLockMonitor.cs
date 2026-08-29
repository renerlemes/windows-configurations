using Microsoft.Win32;
using System;

namespace Windows.Configurations.Features.Audio
{
    public sealed class MuteOnLockMonitor : IDisposable
    {
        private bool _enabled;
        private bool _mutedByUs;
        private bool _savedMute;
        private float _savedVolume;

        public void SetEnabled(bool enabled)
        {
            if (enabled == _enabled)
                return;

            _enabled = enabled;

            if (enabled)
            {
                SystemEvents.SessionSwitch += OnSessionSwitch;
                return;
            }

            SystemEvents.SessionSwitch -= OnSessionSwitch;
            RestoreIfNeeded();
        }

        private void OnSessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            if (!_enabled)
                return;

            if (e.Reason == SessionSwitchReason.SessionLock)
            {
                Mute();
                return;
            }

            if (e.Reason == SessionSwitchReason.SessionUnlock)
                RestoreIfNeeded();
        }

        private void Mute()
        {
            _savedMute = SystemVolume.GetMute();
            _savedVolume = SystemVolume.GetVolume();
            SystemVolume.SetMute(true);
            _mutedByUs = true;
        }

        private void RestoreIfNeeded()
        {
            if (!_mutedByUs)
                return;

            SystemVolume.SetVolume(_savedVolume);
            SystemVolume.SetMute(_savedMute);
            _mutedByUs = false;
        }

        public void Dispose()
        {
            SetEnabled(false);
        }
    }
}
