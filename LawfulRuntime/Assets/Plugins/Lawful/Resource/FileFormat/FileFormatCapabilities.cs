namespace Lawful.Resource.FileFormat
{
    /// <summary>
    /// File Format Capabilities are used to store what the format can be used for.<br/>
    /// The class is abstract as is expected to be overriden per format type (Image, Model etc)
    /// </summary>
    public abstract class FileFormatCapabilities
    {
        /// <summary>Set when the format handler should allow exporting</summary>
        public bool allowExport;

        /// <summary>Set when the format handler should allow importing</summary>
        public bool allowImport;

        /// <summary>Set when the format is considered to be superseded by an alternative</summary>
        public bool deprecated;

        /// <summary>Set when the format is considered experimental e.g. research, toy, or formats under active development</summary>
        public bool experimental;
    }
}