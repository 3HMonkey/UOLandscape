using System;
using System.IO;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace UOLandscape.Configuration
{
    public sealed class ConfigurationLoader : IConfigurationLoader
    {
        private readonly ILogger _logger;

        public ConfigurationLoader(ILogger<ConfigurationLoader> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public T LoadConfiguration<T>(string fileName) where T : class
        {
            if (!File.Exists(fileName))
            {
                _logger.LogWarning($"Configuration file: '{fileName}' does not exist.");
                return null;
            }

            _logger.LogInformation($"Loading Configuration {fileName}...");
            var configurationContent = File.ReadAllText(fileName);
            _logger.LogInformation($"Loading Configuration {fileName}...Done.");
            return JsonConvert.DeserializeObject<T>(configurationContent);
        }
    }
}