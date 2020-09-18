using Newtonsoft.Json;

namespace UOLandscape.Configuration
{
    internal sealed class Configuration
    {
        [JsonProperty("uopath")]
        public string UOPath { get; set; } = string.Empty;

        [JsonProperty("testvar")]
        public string Testvar { get; set; } = string.Empty;

        public string ClientVersion { get; internal set; }
    }
}
