using System.IO;
using System.Text.Json;



namespace mazda_5.Services
{
    public class AppConfig
    {
        public string SelectedPort { get; set; } = "";
        public int SelectedBaud { get; set; } = 0;
    }

    public static class ConfigManager
    {
        private static readonly string ConfigPath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");

        public static AppConfig Load()
        {
            try
            {
                if (!File.Exists(ConfigPath))
                    return new AppConfig();

                string json = File.ReadAllText(ConfigPath);

                if (string.IsNullOrWhiteSpace(json))
                    return new AppConfig();

                var cfg = JsonSerializer.Deserialize<AppConfig>(json);

                return cfg ?? new AppConfig();
            }
            catch
            {
                return new AppConfig();
            }
        }

        public static void Save(AppConfig config)
        {
            var json = JsonSerializer.Serialize(config, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(ConfigPath, json);
        }
    }
}
