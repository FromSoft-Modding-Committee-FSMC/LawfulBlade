using LawfulBladeManager.Core;
using LawfulBladeManager.Instances;
using LawfulBladeManager.Networking;
using LawfulBladeManager.Packages;
using LawfulBladeManager.Projects;

namespace LawfulBladeManager
{
    public static class Program
    {
        // Configuration
        public readonly static Preferences Preferences = Preferences.Load();

        // Delegates
        public delegate bool OnBoolEvent();

        // Events
        public static event OnBoolEvent OnShutdown = () => { Preferences.Save(); Environment.Exit(0); return true; };

        // Managers
        #pragma warning disable CS8618 // Non-nullable field must contain a non-null value... It's set in the ProgramContext, chill the fuck out.
        public static DownloadManager DownloadManager;
        public static PackageManager  PackageManager;
        public static InstanceManager InstanceManager;
        public static ProjectManager  ProjectManager;
        #pragma warning restore CS8618

        // Context and Program Properties
        public readonly static ProgramContext Context = new();

        /// <summary>
        /// Program Entry Point
        /// </summary>
        [STAThread] 
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(Context);
        }

        /// <summary>
        /// Raises the OnShutdown event.
        /// </summary>
        public static void Shutdown() =>
            OnShutdown?.Invoke();
    }
}