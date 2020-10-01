using System;
using System.IO;
using Newtonsoft.Json;
using Serilog;

namespace UOLandscape.Configuration
{
    public sealed class ConfigurationLoader : IConfigurationLoader
    {
        private readonly ILogger _logger;

        public ConfigurationLoader(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public T LoadConfiguration<T>(string fileName) where T : class
        {
            if (!File.Exists(fileName))
            {
                _logger.Warning($"Configuration file: '{fileName}' does not exist.");
                return null;
            }

            _logger.Information($"Loading Configuration {fileName}...");
            var configurationContent = File.ReadAllText(fileName);
            _logger.Information($"Loading Configuration {fileName}...Done.");
            return JsonConvert.DeserializeObject<T>(configurationContent);
        }
    }
}