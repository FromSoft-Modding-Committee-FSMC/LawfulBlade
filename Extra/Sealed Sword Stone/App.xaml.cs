using System.IO;
using System.Text.Json;
using System.Windows;

namespace Sealed_Sword_Stone;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static LaunchConfig LauncherConfig { get; private set; }

    public static string ProgramPath = Path.GetDirectoryName(Environment.ProcessPath);

    void OnApplicationStartup(object sender, StartupEventArgs e)
    {
        // Load Launcher Config
        LauncherConfig = JsonSerializer.Deserialize<LaunchConfig>(
            File.ReadAllText(
                Path.Combine(App.ProgramPath, "LAUNCHER", "launcher_config.json")
                )
            );
    }

    public static void StartGame()
    {
        // Create our suspended process
        ProcessEx process = ProcessEx.StartSuspendedProcess(Path.Combine(ProgramPath, LauncherConfig.GameExecutable));

        // Inject our patch
        process.InjectPayload(Path.Combine(ProgramPath, LauncherConfig.GamePatch));

        // Resume the process
        process.ResumeProcess();
    }
}

