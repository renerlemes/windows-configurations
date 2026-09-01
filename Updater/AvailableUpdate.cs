namespace Windows.Configurations.Updater
{
    internal sealed class AvailableUpdate
    {
        public string VersionDisplay { get; init; }

        public string Changelog { get; init; }

        public string FileName { get; init; }

        public string DownloadUrl { get; init; }
    }
}
