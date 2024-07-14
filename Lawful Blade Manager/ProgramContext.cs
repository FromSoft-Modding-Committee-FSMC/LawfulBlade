using LawfulBladeManager.Dialog;
using LawfulBladeManager.Forms;
using LawfulBladeManager.Packages;

namespace LawfulBladeManager
{
    public class ProgramContext : ApplicationContext
    {
        // Private Data
        static readonly string pathAppData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FSMC", "LawfulBlade");
        static readonly string pathProgram = Application.StartupPath;
        readonly FormMain mainWindow;

        // Properties
        public static string AppDataPath => pathAppData;
        public static string ProgramPath => pathProgram;

        // Constructors
        public ProgramContext()
        {
            // Event Binding
            //Application.ApplicationExit += OnApplicationExit;

            // Basic Initialization
            InitializeFirstTime();

            mainWindow = new FormMain();
            mainWindow.FormClosing += OnApplicationExit;    // Yeah... The other one isn't firing?
            mainWindow.Show();

            // Lets check if we must wait now for packages to be finished...
            if(Program.PackageManager.State != PackageManager.PackageManagerState.Ready)
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
