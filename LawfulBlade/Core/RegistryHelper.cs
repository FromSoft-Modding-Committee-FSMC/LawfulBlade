using Microsoft.Win32;

namespace LawfulBlade.Core
{
    /// <summary>
    /// Allows access to the Sword of Moonlight registry values in a convinient and friendly way.
    /// </summary>
    public static class RegistryHelper
    {
        /// <summary>
        /// Root location of SoM registry values
        /// </summary>
        static RegistryKey registryRoot;

        /// <summary>
        /// Handles initialization of the registry class the first time it is accessed...
        /// </summary>
        static RegistryHelper()
        {
            // Attempt to get the root Sword of Moonlight registry
            // When it fails, we just create it...
            registryRoot = Registry.CurrentUser.OpenSubKey("Software\\FROMSOFTWARE\\SOM", true)
                ?? Registry.CurrentUser.CreateSubKey("Software\\FROMSOFTWARE\\SOM", true);
        }

        /// <summary>
        /// Sets (optionally creating) a value
        /// </summary>
        public static void SetValue(string root, string key, object value)
        {
            // If there is a subkey, we must open it...
            RegistryKey intRoot = registryRoot;

            if (root != string.Empty)
                intRoot = intRoot.OpenSubKey(root, true) ?? intRoot.CreateSubKey(root, true);

            // Now we can set our value...
            intRoot.SetValue(key, value);
        }

        /// <summary>
        /// Gets a value
        /// </summary>
        public static bool GetValue(string root, string key, out object value)
        {
            RegistryKey intRoot = registryRoot;

            if (root != string.Empty)
                intRoot = intRoot.OpenSubKey(root, true) ?? intRoot.CreateSubKey(root, true);

            value = intRoot.GetValue(key);

            return value != null;
        }
    }
}
