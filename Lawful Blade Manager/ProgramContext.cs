using LawfulBladeManager.Dialog;
using LawfulBladeManager.Forms;
using LawfulBladeManager.Packages;
using LawfulBladeManager.Projects;

namespace LawfulBladeManager
{
    public class ProgramContext : ApplicationContext
    {
        // Private Data
        static readonly string pathAppData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FSMC", "LawfulBlade");
        static readonly string pathProgram = Application.StartupPath;
        readonly MainForm mainWindow;

        // Properties
        public static string AppDataPath => pathAppData;
        public static string ProgramPath => pathProgram;

        /**
         * The normal version string you can set in the program properties is crap.
         *   D = Development Build, S = Stable Build
         **/
        public static string Version => "0.28D";

        // Constructors
        public ProgramContext()
        {
            // Event Binding
            //Application.ApplicationExit += OnApplicationExit;

            // Basic Initialization
            InitializeFirstTime();

            mainWindow = new MainForm();
            mainWindow.FormClosing += OnApplicationExit;    // Yeah... The other one isn't firing?
            mainWindow.Show();

            // Lets check if we must wait now for packages to be finished...
            if(Program.PackageManager.State != PackageManagerState.Ready)
            {
                Program.PackageManager.OnPackagePrepareCompleted += OnPackagesPrepared;

                // Stop the main window from being interacted with while we wait for the package manager...
                mainWindow.Enabled = false;

                // Spawn the busy form
                BusyDialog.Instance.ShowBusy();
            }
        }

        void OnPackagesPrepared()
        {
            // Hide the busy instance
            BusyDialog.Instance.HideBusy();

            // Re Enable window interaction
            mainWindow.BeginInvoke(() => mainWindow.Enabled = true);

            // Check if any instances or projects have updates avaliable
            Program.InstanceManager.CheckInstancesForOutdatedPackages();
            Program.ProjectManager.CheckProjectsForOutdatedPackages();

            // Remove me from the event handler
            Program.PackageManager.OnPackagePrepareCompleted -= OnPackagesPrepared;
        } 

        /// <summary>
        /// Initializes data on the first run.
        /// </summary>
        static void InitializeFirstTime()
        {
            // We exit early if the appdata path already exists.
            if (Directory.Exists(AppDataPath))
                return;

            // Recursively create all application data paths.
            Directory.CreateDirectory(AppDataPath);
        }

        // Event Callbacks
        void OnApplicationExit(object? sender, EventArgs e) =>
            Program.Shutdown();
    }
}
