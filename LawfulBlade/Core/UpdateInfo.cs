using System.Text.Json.Serialization;

namespace LawfulBlade.Core
{
    public struct UpdateInfo
    {
        /// <summary>
        /// Version information for the update
        /// </summary>
        [JsonInclude]
        public string Version;

        /// <summary>Source file for the update</summary>
        [JsonInclude]
        public string SourceF;
    }
}
