using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LawfulBlade.Core
{
    public class Preferences
    {
        /// <summary>
        /// Enables automatically checking for updates on start up.
        /// </summary>
        [JsonInclude]
        public bool AutoCheckForUpdates = false;

        /// <summary>
        /// Enables showing the debug console
        /// </summary>
        [JsonInclude]
        public bool ShowDebugConsole = false;

        /// <summary>
        /// If Locale Emulator should be used when launching instances and projects
        /// </summary>
        [JsonInclude]
        public bool UseLocaleEmulator = false;

        /// <summary>
        /// The path to locale emulators installation directory
        /// </summary>
        [JsonInclude]
        public string LocaleEmulatorPath = string.Empty;

        /// <summary>
        /// Private Constructor.<br/>
        /// We only want the preferences to be constructed internally.
        /// </summary>
        [JsonConstructor]
        Preferences() { }

        // Constant path to the preferences file
        [JsonIgnore]
        static readonly string PreferencesFile = Path.Combine(App.AppDataPath, "preferences.json");

        /// <summary>
        /// Saves preferences to the disc...
        /// </summary>
        public void Save() =>
            File.WriteAllText(PreferencesFile, JsonSerializer.Serialize(this, JsonSerializerOptions.Default));

        /// <summary>
        /// Loads preferences from disc...
        /// </summary>
        public static Preferences Load()
        {
            // If the preferences file doesn't exist, we create a new one...
            if (!File.Exists(PreferencesFile))
                return new Preferences();

            // Preference file must exist at this point, so lets try to load it. If it fails to load, we can also return the default...
            return JsonSerializer.Deserialize<Preferences>(File.ReadAllText(PreferencesFile), JsonSerializerOptions.Default) ?? new Preferences();
        }
    }
}
