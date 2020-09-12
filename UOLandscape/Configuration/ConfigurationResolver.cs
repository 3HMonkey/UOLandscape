using Newtonsoft.Json;
using System.IO;
using System.Text.RegularExpressions;
using UOLandscape.Utility.Logging;

namespace UOLandscape.Configuration
{
    internal static class ConfigurationResolver
    {
        public static T Load<T>(string file) where T : class
        {
            if( !File.Exists(file) )
            {
                Log.Warn(file + " not found.");

                return null;
            }

            string text = File.ReadAllText(file);
            text = Regex.Replace(text,
                                         @"(?<!\\)  # lookbehind: Check that previous character isn't a \
                                                \\         # match a \
                                                (?!\\)     # lookahead: Check that the following character isn't a \",
                                    @"\\", RegexOptions.IgnorePatternWhitespace);

            T settings = JsonConvert.DeserializeObject<T>(text);

            return settings;
        }

        public static void Save<T>(T obj, string file) where T : class
        {
            File.WriteAllText(file, JsonConvert.SerializeObject(obj, Formatting.Indented));
        }
    }
}
