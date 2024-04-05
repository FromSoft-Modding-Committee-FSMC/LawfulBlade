using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawfulBladeManager
{
    /// <summary>
    /// Provides a wrapping of registry values to enable easy access as properties.
    /// </summary>
    public static class ProgramRegistry
    {
        // Private Data
        static RegistryKey RegKeySOM;

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

            set => RegKeySOM.SetValue("InstDir", (object)value, RegistryValueKind.String);
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
