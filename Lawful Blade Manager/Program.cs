using LawfulBladeManager.Instances;
using LawfulBladeManager.Networking;
using LawfulBladeManager.Packages;
using LawfulBladeManager.Projects;
using System.Text.Json;

namespace LawfulBladeManager
{
    public static class Program
    {
        // Delegates
        public delegate bool OnBoolEvent();

        // Events
        public static event OnBoolEvent OnShutdown = () => { Environment.Exit(0); return true; };

        // Managers
        public readonly static DownloadManager DownloadManager = new();
        public readonly static PackageManager  PackageManager  = new();
        public readonly static InstanceManager InstanceManager = new();
        public readonly static ProjectManager  ProjectManager  = new();

        // Context and Program Properties
        public readonly static ProgramContext Context = new();

        /// <summary>
        /// Program Entry Point
        /// </summary>
        [STAThread] static void Main()
        {
            ApplicationConfiguration.Initialize();

            // PackageManager.CreatePackageSource("D:\\dev\\.NET Projects\\Lawful-Blade\\Lawful Blade Manager\\bin\\Debug\\net7.0-windows\\Packages", "D:\\dev\\.NET Projects\\Lawful-Blade\\Lawful Blade Manager\\bin\\Debug\\net7.0-windows\\Packages");

            Application.Run(Context);
        }

        /// <summary>
        /// Raises the OnShutdown event.
        /// </summary>
        public static void Shutdown() =>
            OnShutdown?.Invoke();
    }
}