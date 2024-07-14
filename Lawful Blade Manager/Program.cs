using LawfulBladeManager.Instances;
using LawfulBladeManager.Networking;
using LawfulBladeManager.Packages;
using LawfulBladeManager.Projects;
using Microsoft.VisualBasic.ApplicationServices;

namespace LawfulBladeManager
{
    public static class Program
    {
        public delegate bool OnVoidEvent();
        public static event OnVoidEvent? OnShutdown;

        // Properties
        public readonly static InstanceManager? InstanceManager = new();
        public readonly static DownloadManager? DownloadManager = new();
        public readonly static ProjectManager? ProjectManager   = new();
        public readonly static ProgramContext? Context          = new();
        public readonly static PackageManager? PackageManager   = new();

        // Entry Point
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // Bind shutdown event...
            OnShutdown += ProgramShutdown;

            Application.Run(Context);
        }

        static bool ProgramShutdown()
        {
            Environment.Exit(0);
            
            return true;
        }

        public static void Shutdown() =>
            OnShutdown?.Invoke();
    }
}