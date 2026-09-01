using System;

namespace Windows.Configurations.Updater
{
    internal static class AppVersion
    {
        public static string CurrentDisplay => Format(ApplicationProductVersion());

        public static Version Current => Parse(ApplicationProductVersion()) ?? new Version(0, 0, 0);

        public static string Format(string version)
        {
            Version parsed = Parse(version);

            if (parsed is null)
                return version?.Trim().TrimStart('v', 'V');

            int build = parsed.Build < 0 ? 0 : parsed.Build;

            return $"{parsed.Major}.{parsed.Minor}.{build}";
        }

        public static Version Parse(string version)
        {
            if (string.IsNullOrWhiteSpace(version))
                return null;

            string numeric = version.Trim().TrimStart('v', 'V');
            int metadata = numeric.IndexOfAny(['+', '-']);

            if (metadata >= 0)
                numeric = numeric[..metadata];

            return Version.TryParse(numeric, out Version parsed) ? parsed : null;
        }

        public static bool IsNewer(string candidate, string current)
        {
            Version newer = Parse(candidate);
            Version installed = Parse(current);

            return newer is not null && installed is not null && newer > installed;
        }

        private static string ApplicationProductVersion()
        {
            return System.Windows.Forms.Application.ProductVersion;
        }
    }
}
