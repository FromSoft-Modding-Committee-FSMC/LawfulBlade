namespace Lawful.Resource
{
    /// <summary>
    /// Resource source is a tag to tell us the origin of a resource
    /// </summary>
    public enum ResourceSource
    {
        /// <summary>The resource was loaded externally from the file system</summary>
        FileSystem,

        /// <summary>The resource was loaded from an asset bundle</summary>
        AssetBundle,

        /// <summary>The resource was either placed manually or is from the unity asset database</summary>
        Other
    }
}