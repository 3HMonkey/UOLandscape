using Newtonsoft.Json;
using System.IO;

namespace UOLandscape.Configuration
{
    internal sealed class ConfigurationSettings
    {
        private const string CONFIGURATION_SETTINGS_FILENAME = "config.json";
        private static string CustomSettingsFilepath = null;
        


        public static ConfigurationSettings GlobalSettings = new ConfigurationSettings();

        public ConfigurationSettings()
        {
            
        }
        

        [JsonProperty("uopath")]
        public string UOPath { get; set; } = string.Empty;

        [JsonProperty("testvar")]
        public string Testvar { get; set; } = string.Empty;

        public static string GetSettingsFilepath()
        {
            if( CustomSettingsFilepath != null )
            {
                if( Path.IsPathRooted(CustomSettingsFilepath) )
                    return CustomSettingsFilepath;
                else
                    return Path.Combine(UOLandscapeEnvironment.ExecutablePath, CustomSettingsFilepath);
            }

            return Path.Combine(UOLandscapeEnvironment.ExecutablePath, CONFIGURATION_SETTINGS_FILENAME);
        }

        public void Save()
        {
            
            ConfigurationSettings settingsToSave = JsonConvert.DeserializeObject<ConfigurationSettings>(JsonConvert.SerializeObject(this)); 
            ConfigurationResolver.Save(settingsToSave, GetSettingsFilepath());
        }
    }


}
