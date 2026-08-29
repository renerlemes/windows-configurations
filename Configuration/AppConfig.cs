using System;
using System.IO;
using System.Text.Json;

namespace Windows.Configurations.Configuration
{
    public static class AppConfig
    {
        public static string FilePath => Path.Combine(AppContext.BaseDirectory, "Windows.Configurations.json");

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };

        public static AppConfiguration Load()
        {
            if (!File.Exists(FilePath))
                return new AppConfiguration();

            try
            {
                AppConfiguration settings = JsonSerializer.Deserialize<AppConfiguration>(File.ReadAllText(FilePath), JsonOptions) ?? new AppConfiguration();
                
                settings.EnsureDefaults();
                
                return settings;
            }
            catch (JsonException)
            {
                return new AppConfiguration();
            }
        }

        public static void Save(AppConfiguration settings)
        {
            settings.EnsureDefaults();

            File.WriteAllText(FilePath, JsonSerializer.Serialize(settings, JsonOptions));
        }
    }
}
