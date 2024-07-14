using System.Text.Json.Serialization;

namespace LawfulBladeManager.Packages
{
    /// <summary>
    /// A struct representing a source of packages
    /// </summary>
    public struct PackageSource
    {
        /// <summary>
        /// Creation Date is used to poll source updates.
        /// </summary>
        [JsonInclude]
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// URI is the origin of the package. Used when we need to pull updates.
        /// </summary>
        [JsonInclude]
        public string URI            { get; set; }

        /// <summary>
        /// Packages contained in the source.
        /// </summary>
        [JsonInclude]
        public Package[] Packages    { get; set; }
    }
}
