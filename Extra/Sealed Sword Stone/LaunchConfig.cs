using System.Text.Json.Serialization;

namespace Sealed_Sword_Stone
{
    public class LaunchConfig
    {
        [JsonInclude]
        public string GameName { get; private set; }

        [JsonInclude]
        public string GameExecutable { get; private set; } = string.Empty;

        [JsonInclude]
        public string GamePatch { get; private set; } = string.Empty;

        [JsonInclude]
        public string LauncherMusic { get; private set; } = string.Empty;
    }
}
