using System;
using System.Runtime.InteropServices;

namespace Windows.Configurations.Features.Audio
{
    internal static class AudioDefaultDevice
    {
        private const int eConsole = 0;
        private const int eMultimedia = 1;
        private const int eCommunications = 2;

        public static bool SetDefault(string deviceId)
        {
            if (string.IsNullOrEmpty(deviceId))
                return false;

            IPolicyConfig policy = null;

            try
            {
                policy = (IPolicyConfig)new PolicyConfigClient();

                return policy.SetDefaultEndpoint(deviceId, eConsole) == 0
                    && policy.SetDefaultEndpoint(deviceId, eMultimedia) == 0
                    && policy.SetDefaultEndpoint(deviceId, eCommunications) == 0;
            }
            catch (COMException)
            {
                return false;
            }
            finally
            {
                if (policy != null)
                    Marshal.ReleaseComObject(policy);
            }
        }
    }

    [ComImport]
    [Guid("870af99c-171d-4f9e-af0d-e63df40c2bc9")]
    internal class PolicyConfigClient
    {
    }

    [ComImport]
    [Guid("f8679f50-850a-41cf-9c72-430f290290c8")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IPolicyConfig
    {
        [PreserveSig]
        int GetMixFormat([MarshalAs(UnmanagedType.LPWStr)] string pszDeviceName, out IntPtr ppFormat);

        [PreserveSig]
        int GetDeviceFormat([MarshalAs(UnmanagedType.LPWStr)] string pszDeviceName, int bDefault, out IntPtr ppFormat);

        [PreserveSig]
        int ResetDeviceFormat([MarshalAs(UnmanagedType.LPWStr)] string pszDeviceName);

        [PreserveSig]
        int SetDeviceFormat([MarshalAs(UnmanagedType.LPWStr)] string pszDeviceName, IntPtr pEndpointFormat, IntPtr pMixFormat);

        [PreserveSig]
        int GetProcessingPeriod([MarshalAs(UnmanagedType.LPWStr)] string pszDeviceName, int bDefault, out long pmftDefaultPeriod, out long pmftMinimumPeriod);

        [PreserveSig]
        int SetProcessingPeriod([MarshalAs(UnmanagedType.LPWStr)] string pszDeviceName, ref long pmftPeriod);

        [PreserveSig]
        int GetShareMode([MarshalAs(UnmanagedType.LPWStr)] string pszDeviceName, IntPtr pMode);

        [PreserveSig]
        int SetShareMode([MarshalAs(UnmanagedType.LPWStr)] string pszDeviceName, IntPtr pMode);

        [PreserveSig]
        int GetPropertyValue([MarshalAs(UnmanagedType.LPWStr)] string pszDeviceName, int bFxStore, ref PROPERTYKEY key, out PROPVARIANT pv);

        [PreserveSig]
        int SetPropertyValue([MarshalAs(UnmanagedType.LPWStr)] string pszDeviceName, int bFxStore, ref PROPERTYKEY key, ref PROPVARIANT pv);

        [PreserveSig]
        int SetDefaultEndpoint([MarshalAs(UnmanagedType.LPWStr)] string wszDeviceId, int eRole);

        [PreserveSig]
        int SetEndpointVisibility([MarshalAs(UnmanagedType.LPWStr)] string pszDeviceName, int bVisible);
    }
}
