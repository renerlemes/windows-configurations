using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Windows.Configurations.Features.Audio
{
    internal sealed class AudioEndpoint
    {
        public string Id { get; init; }

        public string Name { get; init; }

        public string IconPath { get; init; }
    }

    internal static class AudioEndpointEnumerator
    {
        private const int eRender = 0;
        private const int eCapture = 1;
        private const int eMultimedia = 1;
        private const int DEVICE_STATE_ACTIVE = 1;
        private const int STGM_READ = 0;
        private const ushort VT_LPWSTR = 31;

        private static readonly PROPERTYKEY PkeyDeviceFriendlyName = new()
        {
            fmtid = new Guid("a45c254e-df1c-4efd-8020-67d146a850e0"),
            pid = 14
        };

        private static readonly PROPERTYKEY PkeyDeviceClassIconPath = new()
        {
            fmtid = new Guid("259abffc-50a7-47ce-af08-68c9a7d73366"),
            pid = 12
        };

        public static IReadOnlyList<AudioEndpoint> ListPlayback() => List(eRender);

        public static IReadOnlyList<AudioEndpoint> ListRecording() => List(eCapture);

        public static string GetDefaultPlaybackId() => GetDefaultId(eRender);

        public static string GetDefaultRecordingId() => GetDefaultId(eCapture);

        private static IReadOnlyList<AudioEndpoint> List(int dataFlow)
        {
            List<AudioEndpoint> result = [];
            IMMDeviceEnumerator enumerator = null;
            IMMDeviceCollection collection = null;

            try
            {
                enumerator = (IMMDeviceEnumerator)new MMDeviceEnumeratorComObject();
                enumerator.EnumAudioEndpoints(dataFlow, DEVICE_STATE_ACTIVE, out collection);
                collection.GetCount(out int count);

                for (int i = 0; i < count; i++)
                {
                    IMMDevice device = null;

                    try
                    {
                        collection.Item(i, out device);
                        AudioEndpoint endpoint = ReadEndpoint(device);

                        if (endpoint != null)
                            result.Add(endpoint);
                    }
                    finally
                    {
                        if (device != null)
                            Marshal.ReleaseComObject(device);
                    }
                }
            }
            catch (COMException)
            {
            }
            finally
            {
                if (collection != null)
                    Marshal.ReleaseComObject(collection);

                if (enumerator != null)
                    Marshal.ReleaseComObject(enumerator);
            }

            return result;
        }

        private static string GetDefaultId(int dataFlow)
        {
            IMMDeviceEnumerator enumerator = null;
            IMMDevice device = null;

            try
            {
                enumerator = (IMMDeviceEnumerator)new MMDeviceEnumeratorComObject();
                enumerator.GetDefaultAudioEndpoint(dataFlow, eMultimedia, out device);
                device.GetId(out string id);
                return id;
            }
            catch (COMException)
            {
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

        private static AudioEndpoint ReadEndpoint(IMMDevice device)
        {
            IPropertyStore store = null;

            try
            {
                device.GetId(out string id);

                if (string.IsNullOrWhiteSpace(id))
                    return null;

                device.OpenPropertyStore(STGM_READ, out store);

                string name = ReadString(store, PkeyDeviceFriendlyName);

                return new AudioEndpoint
                {
                    Id = id,
                    Name = string.IsNullOrWhiteSpace(name) ? id : name,
                    IconPath = ReadString(store, PkeyDeviceClassIconPath)
                };
            }
            finally
            {
                if (store != null)
                    Marshal.ReleaseComObject(store);
            }
        }

        private static string ReadString(IPropertyStore store, PROPERTYKEY key)
        {
            PROPVARIANT variant = default;

            try
            {
                store.GetValue(ref key, out variant);

                return variant.vt == VT_LPWSTR
                    ? Marshal.PtrToStringUni(variant.pointerValue)
                    : null;
            }
            catch (COMException)
            {
                return null;
            }
            finally
            {
                PropVariantClear(ref variant);
            }
        }

        [DllImport("ole32.dll")]
        private static extern int PropVariantClear(ref PROPVARIANT pvar);
    }
}
