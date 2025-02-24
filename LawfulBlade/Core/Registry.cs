namespace LawfulBlade.Core
{
    /// <summary>
    /// Allows access to the Sword of Moonlight registry values in a convinient and friendly way.
    /// </summary>
    public static class Registry
    {
        /// <summary>
        /// Root location of SoM registry values
        /// </summary>
        static Microsoft.Win32.RegistryKey registryRoot;
        static Microsoft.Win32.RegistryKey registryInstall;

        /// <summary>
        /// Handles initialization of the registry class the first time it is accessed...
        /// </summary>
        static Registry()
        {
            // Attempt to get the root Sword of Moonlight registry
            // When it fails, we just create it...
            registryRoot = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software\\FROMSOFTWARE\\SOM", true)
                ?? Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Software\\FROMSOFTWARE\\SOM", true);

            // Get or create the INSTALL subkey
            registryInstall = registryRoot.OpenSubKey("INSTALL") ?? registryRoot.CreateSubKey("INSTALL");
        }

        //
        // Now we can define a load of properties which enable us to easily access the registry...
        //

        /// <summary>The current editor installation directory</summary>
        public static string InstDir
        {
            get => (string)registryInstall.GetValue("InstDir", "") ?? string.Empty;
            set => registryInstall.SetValue("InstDir", value);
        }
    }
}
