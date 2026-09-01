using System;
using System.Runtime.InteropServices;

namespace Windows.Configurations.Features.Audio
{
    [ComImport]
    [Guid("BCDE0395-E52F-467C-8E3D-C4579291692E")]
    internal class MMDeviceEnumeratorComObject
    {
    }

    [ComImport]
    [Guid("A95664D2-9614-4F35-A746-DE8DB63617E6")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IMMDeviceEnumerator
    {
        int EnumAudioEndpoints(int dataFlow, int dwStateMask, out IMMDeviceCollection ppDevices);

        int GetDefaultAudioEndpoint(int dataFlow, int role, out IMMDevice ppDevice);
    }

    [ComImport]
    [Guid("0BD7A1BE-7A1A-44DB-8397-CC5392387B5E")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IMMDeviceCollection
    {
        int GetCount(out int pcDevices);

        int Item(int nDevice, out IMMDevice ppDevice);
    }

    [ComImport]
    [Guid("D666063F-1587-4E43-81F1-B948E807363F")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IMMDevice
    {
        int Activate(ref Guid iid, int dwClsCtx, IntPtr pActivationParams, [MarshalAs(UnmanagedType.IUnknown)] out object ppInterface);

        int OpenPropertyStore(int stgmAccess, out IPropertyStore ppProperties);

        int GetId([MarshalAs(UnmanagedType.LPWStr)] out string ppstrId);
    }

    [ComImport]
    [Guid("886d8eeb-8cf2-4446-8d02-cdba1dbdcf99")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IPropertyStore
    {
        int GetCount(out int cProps);

        int GetAt(int iProp, out PROPERTYKEY pkey);

        int GetValue(ref PROPERTYKEY key, out PROPVARIANT pv);
    }

    [ComImport]
    [Guid("5CDF2C82-841E-4546-9722-0CF74078229A")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IAudioEndpointVolume
    {
        int RegisterControlChangeNotify(IntPtr pNotify);

        int UnregisterControlChangeNotify(IntPtr pNotify);

        int GetChannelCount(out int pnChannelCount);

        int SetMasterVolumeLevel(float fLevelDB, ref Guid pguidEventContext);

        int SetMasterVolumeLevelScalar(float fLevel, ref Guid pguidEventContext);

        int GetMasterVolumeLevel(out float pfLevelDB);

        int GetMasterVolumeLevelScalar(out float pfLevel);

        int SetChannelVolumeLevel(uint nChannel, float fLevelDB, ref Guid pguidEventContext);

        int SetChannelVolumeLevelScalar(uint nChannel, float fLevel, ref Guid pguidEventContext);

        int GetChannelVolumeLevel(uint nChannel, out float pfLevelDB);

        int GetChannelVolumeLevelScalar(uint nChannel, out float pfLevel);

        int SetMute([MarshalAs(UnmanagedType.Bool)] bool bMute, ref Guid pguidEventContext);

        int GetMute([MarshalAs(UnmanagedType.Bool)] out bool pbMute);
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct PROPERTYKEY
    {
        public Guid fmtid;
        public int pid;
    }

    /// <summary>
    /// O layout precisa ter o tamanho exato do PROPVARIANT nativo (24 bytes em x64): declarar
    /// menos faz o GetValue escrever além do buffer e corromper a pilha.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct PROPVARIANT
    {
        public ushort vt;
        public ushort wReserved1;
        public ushort wReserved2;
        public ushort wReserved3;
        public IntPtr pointerValue;
        public IntPtr unionRemainder;
    }
}
