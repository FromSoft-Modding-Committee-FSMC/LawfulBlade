using System.Diagnostics;
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
        /*
        Process.Start(new ProcessStartInfo
        {
            FileName  = "Unsealer.exe",
            Arguments = $"/D:Unsealer.dll \"{LauncherConfig.GameExecutable}\"",
            CreateNoWindow = true
        });
        */

        Injector.StartProcessWithDlls(
            new ProcessStartupConfig(
                [Path.Combine(ProgramPath, LauncherConfig.GameExecutable)],
                false,
                false,
                false
                ),
            [Path.Combine(ProgramPath, LauncherConfig.GamePatch)]
            );

        Current.Shutdown(0);
    }
}

