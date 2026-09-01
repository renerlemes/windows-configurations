using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Windows.Configurations.Updater
{
    internal static class UpdateChecker
    {
        private const string ReleasesUrl = "https://api.github.com/repos/renerlemes/windows-configurations/releases/latest";

        public static async Task<AvailableUpdate> CheckAsync(CancellationToken cancellationToken = default)
        {
            using HttpClient client = CreateClient();

            GitHubRelease release = await client.GetFromJsonAsync<GitHubRelease>(ReleasesUrl, cancellationToken);

            if (release is null || release.Prerelease || string.IsNullOrWhiteSpace(release.TagName))
                return null;

            if (!AppVersion.IsNewer(release.TagName, Application.ProductVersion))
                return null;

            GitHubReleaseAsset asset = release.Assets?
                .FirstOrDefault(item =>
                    !string.IsNullOrWhiteSpace(item?.Name)
                    && item.Name.EndsWith("_Setup.exe", StringComparison.OrdinalIgnoreCase)
                    && !string.IsNullOrWhiteSpace(item.BrowserDownloadUrl));

            if (asset is null)
                return null;

            return new AvailableUpdate
            {
                VersionDisplay = AppVersion.Format(release.TagName),
                Changelog = release.Body?.Trim(),
                FileName = asset.Name,
                DownloadUrl = asset.BrowserDownloadUrl
            };
        }

        internal static HttpClient CreateClient()
        {
            HttpClient client = new()
            {
                Timeout = TimeSpan.FromMinutes(5)
            };

            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Windows.Configurations", AppVersion.CurrentDisplay));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github+json"));

            return client;
        }
    }
}
