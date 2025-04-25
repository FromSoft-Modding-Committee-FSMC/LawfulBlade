using Microsoft.Win32;
using System.IO;
using System.Text;
using System.Windows;

namespace ParamPro;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// Defines a list of possible SoM targets
    /// </summary>
    public enum SomTargetType
    {
        Unknown,
        Editor,
        Project
    }

    /// <summary>
    /// The type of target
    /// </summary>
    public static SomTargetType TargetType { get; private set; }

    /// <summary>
    /// The path to the target
    /// </summary>
    public static string TargetPath { get; private set; }

    /// <summary>
    /// The SJIS Encoding format
    /// </summary>
    public static Encoding SJisEncoding { get; private set; }

    /// <summary>
    /// Event Handler.<br/>
    /// Called when the program is started.
    /// </summary>
    void OnApplicationStartup(object sender, StartupEventArgs e)
    {
        // Must have one argument. The path to the target (Editor or Project)
        if (e.Args.Length == 0)
        {
            // Try to get the instance path from the registry if we didn't pass an argument
            try
            {
                FindTargetPath();
            }
            catch
            {
                Current.Shutdown();
            }
        }
        else
            TargetPath = e.Args[0];

        // Get the target type
        FindTargetType();

        if (TargetType == SomTargetType.Unknown)
        {
            MessageBox.Show("No target given!");
            Current.Shutdown();
        }

        // Set up SHIFT-JIS encoding
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        SJisEncoding = Encoding.GetEncoding(932);

        // Create and show the main window
        new MainWindow().Show();
    }

    void FindTargetPath()
    {
        // We will check the registry...
        RegistryKey? regKey = Registry.CurrentUser.OpenSubKey("Software\\FROMSOFTWARE\\SOM\\INSTALL", true);

        if (regKey == null)
            throw new Exception("No target path given, and no registry key set!");

        // Get the path from the registry
        object? regVal = regKey.GetValue("InstDir");

        if (regVal == null)
            throw new Exception("No target path given, and no registry key set!");

        TargetPath = (string)regVal;
    }

    void FindTargetType()
    {
        // Use "DATA" "TOOL" dirs to detect editor
        if (Directory.Exists(Path.Combine(TargetPath, "DATA")) && Directory.Exists(Path.Combine(TargetPath, "TOOL")))
        {
            TargetType = SomTargetType.Editor;
            return;
        }

        TargetType = SomTargetType.Unknown;
    }
}

