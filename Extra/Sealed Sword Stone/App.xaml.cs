using Sealed_Sword_Stone.Core;
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
    public static UserConfig UserConfig { get; private set; }
    public static string ProgramPath = Path.GetDirectoryName(Environment.ProcessPath);
    public static ControllerState ControllerState { get; private set; } = new ControllerState();

    void OnApplicationStartup(object sender, StartupEventArgs e)
    {
        // Load Launcher Config
        LauncherConfig = JsonSerializer.Deserialize<LaunchConfig>(
            File.ReadAllText(
                Path.Combine(App.ProgramPath, "LAUNCHER", "launcher_config.json")
                )
            );

        UserConfig = UserConfig.Load();
    }

    public static void StartGame()
    {
        // Temp OooOooOoo
        UserConfig.Save();

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

