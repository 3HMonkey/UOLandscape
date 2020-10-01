using System.IO;
using Newtonsoft.Json;
using Serilog;

namespace UOLandscape.Configuration
{
    internal sealed class ConfigurationSaver : IConfigurationSaver
    {
        private readonly ILogger _logger;

        public ConfigurationSaver(ILogger logger)
        {
            _logger = logger;
        }
        
        public void SaveConfiguration<T>(string fileName, T configuration) where T: class
        {
            _logger.Information($"Writing Configuration: {fileName}...");
            File.WriteAllText(fileName, JsonConvert.SerializeObject(configuration, Formatting.Indented));
            _logger.Information($"Writing Configuration: {fileName}...Done");
        }
    }
}