using System.Text.Json.Serialization;
using System.Text.Json;
using System.Runtime.CompilerServices;

namespace LawfulBladeManager.Core
{
    public class Preferences
    {
        /// <summary>
        /// Location of the package cache
        /// </summary>
        [JsonInclude]
        public string PackageCacheDirectory = Path.Combine(ProgramContext.AppDataPath, @"PackageCache");

        /// <summary>
        /// The file which stores preferences.
        /// </summary>
        public static readonly string PreferencesFile = Path.Combine(ProgramContext.AppDataPath, "preferences.json");


        /// <summary>
        /// Loads preferences from disk
        /// </summary>
        /// <returns></returns>
        public static Preferences Load()
        {
            // If the preferences file does not exist, return a new instance of it.
            if (!File.Exists(PreferencesFile))
                return new Preferences();
            
            // The file exists, so we must serialize it.
            Preferences? preferencesFile = JsonSerializer.Deserialize<Preferences>(File.ReadAllText(PreferencesFile), JsonSerializerOptions.Default);

            // If preferences was not deserialized properly, we use the default.
            preferencesFile ??= new Preferences();

            return preferencesFile;
        }

        /// <summary>
        /// Saves preferences to disk
        /// </summary>
        public void Save() =>
            File.WriteAllText(PreferencesFile, JsonSerializer.Serialize(this, JsonSerializerOptions.Default));
    }
}
