using System;
using System.IO;
using System.Text.Json;

namespace Windows.Configurations.Configuration
{
    public static class AppConfig
    {
        /// <summary>
        /// O aplicativo roda sem elevação e a pasta de instalação é somente leitura para o
        /// usuário: a configuração precisa ficar no perfil dele.
        /// </summary>
        public static string FilePath => Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "Windows Configurations",
            "Windows.Configurations.json");

        /// <summary>
        /// Onde as versões elevadas gravavam, e onde o instalador deixa o arquivo padrão.
        /// Serve de origem na primeira execução depois da atualização.
        /// </summary>
        private static string SeedFilePath => Path.Combine(AppContext.BaseDirectory, "Windows.Configurations.json");

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };

        public static AppConfiguration Load()
        {
            AppConfiguration settings = Read(FilePath) ?? Read(SeedFilePath) ?? new AppConfiguration();

            settings.EnsureDefaults();

            return settings;
        }

        public static void Save(AppConfiguration settings)
        {
            settings.EnsureDefaults();

            string path = FilePath;

            Directory.CreateDirectory(Path.GetDirectoryName(path));

            File.WriteAllText(path, JsonSerializer.Serialize(settings, JsonOptions));
        }

        private static AppConfiguration Read(string path)
        {
            if (!File.Exists(path))
                return null;

            try
            {
                return JsonSerializer.Deserialize<AppConfiguration>(File.ReadAllText(path), JsonOptions);
            }
            catch (Exception ex) when (ex is JsonException or IOException or UnauthorizedAccessException)
            {
                return null;
            }
        }
    }
}
