using System.IO;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace UOLandscape.Configuration
{
    internal sealed class ConfigurationSaver : IConfigurationSaver
    {
        private readonly ILogger<ConfigurationSaver> _logger;

        public ConfigurationSaver(ILogger<ConfigurationSaver> logger)
        {
            _logger = logger;
        }
        
        public void SaveConfiguration<T>(string fileName, T configuration) where T: class
        {
            _logger.LogInformation($"Writing Configuration: {fileName}...");
            File.WriteAllText(fileName, JsonConvert.SerializeObject(configuration, Formatting.Indented));
            _logger.LogInformation($"Writing Configuration: {fileName}...Done");
        }
    }
}