using System;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Windows.Configurations.Features.Audio
{
    /// <summary>
    /// Ícone extraído do endpoint, dono do HICON correspondente. Converter o handle em outro
    /// formato (Save, ToBitmap) descarta o canal alfa e deixa o fundo preto, então o handle
    /// original é mantido vivo enquanto o ícone estiver em uso.
    /// </summary>
    internal sealed class AudioDeviceIcon : IDisposable
    {
        private IntPtr _handle;

        private AudioDeviceIcon(IntPtr handle)
        {
            _handle = handle;
            Icon = Icon.FromHandle(handle);
        }

        public Icon Icon { get; private set; }

        /// <summary>
        /// O endpoint informa o ícone no formato do Shell ("arquivo,índice"), como
        /// <c>%windir%\system32\mmres.dll,-3010</c>. Índice negativo é identificador de recurso.
        /// </summary>
        public static AudioDeviceIcon Load(string iconPath, int size)
        {
            if (!TryParseLocation(iconPath, out string file, out int index))
                return null;

            IntPtr handle = IntPtr.Zero;

            try
            {
                if (SHDefExtractIcon(file, index, 0, out handle, IntPtr.Zero, (uint)size) != 0 || handle == IntPtr.Zero)
                    return null;

                AudioDeviceIcon icon = new(handle);

                handle = IntPtr.Zero;

                return icon;
            }
            catch (ArgumentException)
            {
                return null;
            }
            finally
            {
                if (handle != IntPtr.Zero)
                    DestroyIcon(handle);
            }
        }

        public void Dispose()
        {
            Icon?.Dispose();
            Icon = null;

            if (_handle == IntPtr.Zero)
                return;

            DestroyIcon(_handle);
            _handle = IntPtr.Zero;
        }

        private static bool TryParseLocation(string iconPath, out string file, out int index)
        {
            file = null;
            index = 0;

            if (string.IsNullOrWhiteSpace(iconPath))
                return false;

            string location = Environment.ExpandEnvironmentVariables(iconPath.Trim()).Trim('"');
            int separator = location.LastIndexOf(',');

            if (separator < 0)
            {
                file = location;

                return file.Length > 0;
            }

            if (!int.TryParse(location[(separator + 1)..].Trim(), NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out index))
                return false;

            file = location[..separator].Trim().Trim('"');

            return file.Length > 0;
        }

        [DllImport("shell32.dll", CharSet = CharSet.Unicode, EntryPoint = "SHDefExtractIconW")]
        private static extern int SHDefExtractIcon(string pszIconFile, int iIndex, uint uFlags, out IntPtr phiconLarge, IntPtr phiconSmall, uint nIconSize);

        [DllImport("user32.dll")]
        private static extern bool DestroyIcon(IntPtr hIcon);
    }
}
