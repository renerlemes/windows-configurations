using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Windows.Configurations.Updater
{
    internal static class UpdateInstaller
    {
        public static async Task<string> DownloadAsync(AvailableUpdate update, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(update);

            string path = Path.Combine(Path.GetTempPath(), update.FileName);

            using HttpClient client = UpdateChecker.CreateClient();
            using HttpResponseMessage response = await client.GetAsync(update.DownloadUrl, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

            response.EnsureSuccessStatusCode();

            await using FileStream file = File.Create(path);
            await response.Content.CopyToAsync(file, cancellationToken);

            return path;
        }

        public static void Start(string installerPath)
        {
            if (string.IsNullOrWhiteSpace(installerPath) || !File.Exists(installerPath))
                throw new FileNotFoundException("O instalador da atualização não foi encontrado.", installerPath);

            string installDir = AppContext.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            if (installDir.EndsWith(':'))
                installDir += Path.DirectorySeparatorChar;

            ProcessStartInfo start = new()
            {
                FileName = installerPath,
                Arguments = $"/VERYSILENT /SUPPRESSMSGBOXES /NOCANCEL /NORESTART /CLOSEAPPLICATIONS /DIR=\"{installDir}\"",
                UseShellExecute = true
            };

            Process.Start(start);
        }
    }
}
