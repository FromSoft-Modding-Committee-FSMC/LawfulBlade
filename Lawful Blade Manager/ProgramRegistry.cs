using Microsoft.Win32;

namespace LawfulBladeManager
{
    /// <summary>
    /// Provides a wrapping of registry values to enable easy access as properties.
    /// </summary>
    public static class ProgramRegistry
    {
        // Private Data
        static readonly RegistryKey RegKeySOM;

        // Properties
        public static string SOMInstDir
        {
            get
            {
                object? registryValue = RegKeySOM.GetValue("InstDir", "");
                if (registryValue == null)
                    return "";
                return (string)registryValue;
            }

            set => RegKeySOM.SetValue("InstDir", value, RegistryValueKind.String);
        }

        static ProgramRegistry()
        {
            // Attempt to get the default Sword of Moonlight Registry Subkey
            RegistryKey? tempKey = Registry.CurrentUser.OpenSubKey("Software\\FROMSOFTWARE\\SOM\\INSTALL", true);
            if (tempKey != null)
                RegKeySOM = tempKey;
            else
                RegKeySOM = Registry.CurrentUser.CreateSubKey("Software\\FROMSOFTWARE\\SOM\\INSTALL", true);
        }
    }
}
