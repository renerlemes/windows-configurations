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
        /// <summary>
        /// <paramref name="progress"/> recebe o percentual concluído. Fica sem relatos
        /// quando o servidor não informa o tamanho total: aí só o fim do download é certo.
        /// </summary>
        public static async Task<string> DownloadAsync(
            AvailableUpdate update,
            IProgress<int> progress = null,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(update);

            string path = Path.Combine(Path.GetTempPath(), update.FileName);

            using HttpClient client = UpdateChecker.CreateClient();
            using HttpResponseMessage response = await client.GetAsync(update.DownloadUrl, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

            response.EnsureSuccessStatusCode();

            long total = response.Content.Headers.ContentLength ?? 0;

            await using Stream source = await response.Content.ReadAsStreamAsync(cancellationToken);
            await using FileStream file = File.Create(path);

            byte[] buffer = new byte[81920];
            long received = 0;
            int lastPercent = -1;
            int read;

            while ((read = await source.ReadAsync(buffer, cancellationToken)) > 0)
            {
                await file.WriteAsync(buffer.AsMemory(0, read), cancellationToken);

                received += read;

                if (total <= 0 || progress is null)
                    continue;

                int percent = (int)(received * 100 / total);

                // Só relata quando o número muda: evita repintar a barra a cada bloco.
                if (percent == lastPercent)
                    continue;

                lastPercent = percent;
                progress.Report(percent);
            }

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
