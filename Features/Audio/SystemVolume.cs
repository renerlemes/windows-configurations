using System;
using System.Runtime.InteropServices;

namespace Windows.Configurations.Features.Audio
{
    internal static class SystemVolume
    {
        public static bool GetMute()
        {
            IAudioEndpointVolume volume = GetMasterVolume();

            if (volume is null)
                return false;

            try
            {
                volume.GetMute(out bool muted);
                return muted;
            }
            finally
            {
                Marshal.ReleaseComObject(volume);
            }
        }

        public static void SetMute(bool mute)
        {
            IAudioEndpointVolume volume = GetMasterVolume();

            if (volume is null)
                return;

            try
            {
                Guid eventContext = Guid.Empty;
                volume.SetMute(mute, ref eventContext);
            }
            finally
            {
                Marshal.ReleaseComObject(volume);
            }
        }

        public static float GetVolume()
        {
            IAudioEndpointVolume volume = GetMasterVolume();

            if (volume is null)
                return 0f;

            try
            {
                volume.GetMasterVolumeLevelScalar(out float level);
                return level;
            }
            finally
            {
                Marshal.ReleaseComObject(volume);
            }
        }

        public static void SetVolume(float level)
        {
            IAudioEndpointVolume volume = GetMasterVolume();

            if (volume is null)
                return;

            try
            {
                Guid eventContext = Guid.Empty;
                volume.SetMasterVolumeLevelScalar(level, ref eventContext);
            }
            finally
            {
                Marshal.ReleaseComObject(volume);
            }
        }

        private static IAudioEndpointVolume GetMasterVolume()
        {
            IMMDeviceEnumerator enumerator = null;
            IMMDevice device = null;

            try
            {
                enumerator = (IMMDeviceEnumerator)new MMDeviceEnumeratorComObject();
                enumerator.GetDefaultAudioEndpoint(0, 1, out device);

                Guid iid = typeof(IAudioEndpointVolume).GUID;
                device.Activate(ref iid, 1, IntPtr.Zero, out object endpointVolume);

                return endpointVolume as IAudioEndpointVolume;
            }
            finally
            {
                if (device != null)
                    Marshal.ReleaseComObject(device);

                if (enumerator != null)
                    Marshal.ReleaseComObject(enumerator);
            }
        }
    }
}
