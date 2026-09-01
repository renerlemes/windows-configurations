using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;

namespace Windows.Configurations.Features.Audio
{
    internal static class AudioDeviceIcon
    {
        /// <summary>
        /// O endpoint informa o ícone no formato do Shell ("arquivo,índice"), como
        /// <c>%windir%\system32\mmres.dll,-3010</c>. Índice negativo é identificador de recurso.
        /// </summary>
        public static Icon Load(string iconPath, int size)
        {
            if (!TryParseLocation(iconPath, out string file, out int index))
                return null;

            IntPtr handle = IntPtr.Zero;

            try
            {
                if (SHDefExtractIcon(file, index, 0, out handle, IntPtr.Zero, (uint)size) != 0 || handle == IntPtr.Zero)
                    return null;

                using Icon extracted = Icon.FromHandle(handle);

                // Clone() apenas compartilharia o HICON, que é destruído no finally: o
                // ícone precisa ser recriado a partir dos próprios dados para sobreviver.
                using MemoryStream buffer = new();

                extracted.Save(buffer);
                buffer.Position = 0;

                return new Icon(buffer);
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
