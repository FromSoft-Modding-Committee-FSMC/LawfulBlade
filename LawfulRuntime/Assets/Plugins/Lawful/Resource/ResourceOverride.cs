using UnityEngine;

namespace Lawful.Resource
{
    /// <summary>
    /// Resource override is used for replacing resources with files from asset bundles
    /// </summary>
    public struct ResourceOverride
    {
        /// <summary>
        /// The ID of an asset bundle stored in a list
        /// </summary>
        public int bundleID;

        /// <summary>
        /// The path of the asset as it is inside the asset bundle
        /// </summary>
        public string bundlePath;
    }
}
