using System;
using System.Runtime.InteropServices;

namespace Windows.Configurations.Features.Audio
{
    internal static class SystemVolume
    {
        private const int CLSCTX_INPROC_SERVER = 1;

        public static bool GetMute(string deviceId)
        {
            IAudioEndpointVolume volume = GetEndpointVolume(deviceId);

            if (volume is null)
                return false;

            try
            {
                volume.GetMute(out bool muted);
                return muted;
            }
            catch (COMException)
            {
                return false;
            }
            finally
            {
                Marshal.ReleaseComObject(volume);
            }
        }

        public static void SetMute(string deviceId, bool mute)
        {
            IAudioEndpointVolume volume = GetEndpointVolume(deviceId);

            if (volume is null)
                return;

            try
            {
                Guid eventContext = Guid.Empty;
                volume.SetMute(mute, ref eventContext);
            }
            catch (COMException)
            {
            }
            finally
            {
                Marshal.ReleaseComObject(volume);
            }
        }

        private static IAudioEndpointVolume GetEndpointVolume(string deviceId)
        {
            IMMDeviceEnumerator enumerator = null;
            IMMDevice device = null;

            try
            {
                enumerator = (IMMDeviceEnumerator)new MMDeviceEnumeratorComObject();
                enumerator.GetDevice(deviceId, out device);

                Guid iid = typeof(IAudioEndpointVolume).GUID;
                device.Activate(ref iid, CLSCTX_INPROC_SERVER, IntPtr.Zero, out object endpointVolume);

                return endpointVolume as IAudioEndpointVolume;
            }
            catch (COMException)
            {
                // O dispositivo pode ter sido desconectado entre a enumeração e o uso.
                return null;
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
