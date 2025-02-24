using LawfulBlade.Core;
using System.Windows;
using System.IO;
using LawfulBlade.Dialog;
using LawfulBlade.Core.Extensions;
using System.ComponentModel;
using LawfulBlade.Core.Instance;
using LawfulBlade.Core.Package;

namespace LawfulBlade
{
    public partial class App : Application
    {
        /// <summary>
        /// 'Global' Constant Data
        /// </summary>
        public static readonly string Version           = @"0.31";
        public static readonly string ProgramPath       = AppDomain.CurrentDomain.BaseDirectory;
        public static readonly string AppDataPath       = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FSMC", "LawfulBlade");
        public static readonly string ProjectPath       = Path.Combine(AppDataPath, "projects");
        public static readonly string InstancePath      = Path.Combine(AppDataPath, "instances");
        public static readonly string PackageCachePath  = Path.Combine(AppDataPath, "packageCache");
        public static readonly string TemporaryPath     = Path.Combine(Path.GetTempPath(), "FSMC", "LawfulBlade");
        public static readonly string ResourcePath      = Path.Combine(ProgramPath, "Resource");
        public static readonly string ReleaseNotesFile  = Path.Combine(ProgramPath, $"release-notes-v{Version.Strip('.')}.txt");
        public static readonly string ReportAProblemURL = @"https://github.com/FromSoft-Modding-Committee-FSMC/Lawful-Blade/issues";
        public static readonly string RemoteVersionURL  = @"https://raw.githubusercontent.com/FromSoft-Modding-Committee-FSMC/Lawful-Blade/main/version";

        public static Preferences Preferences { get; private set; }

        /// <summary>
        /// Set during the second launch stage, when we have a chance of editing anything...
        /// </summary>
        public static bool FullShutdownEnabled { get; private set; } = false;

        /// <summary>
        /// Event Handler.<br/>
        /// Called when the program is started.
        /// </summary>
        void OnApplicationStartup(object sender, StartupEventArgs e)
        {
            // Initialization Stage #1
            // Here is command line interaction...

            // This needs parsing properly, lad...

            // If there are any arguments, we can kick out...
            if (e.Args.Length > 0)
            {
                switch(e.Args[0])
                {
                    case "inst":
                        // If we want to launch an instance, we need to initialize those
                        InstanceManager.Initialize();

                        // Logic for executing an instance!
                        break;

                    case "proj":
                        // If we want to launch a project, we need to initialize instances and projects...
                        InstanceManager.Initialize();
                        
                        // Logic for executing an project!
                        break;

                    case "mkpk":    // MAKE PACKAGE
                        PackageCreateArgs packageCreationArgs = PackageCreateArgs.FromFile(e.Args[1]);
                        Package.Create(packageCreationArgs).Build(packageCreationArgs);
                        break;

                    case "mkrp":    // MAKE REPOSITORY
                        RepositoryCreateArgs repositoryCreateArgs = RepositoryCreateArgs.FromFile(e.Args[1]);
                        Repository.Create(repositoryCreateArgs).Build(repositoryCreateArgs);
                        break;
                }

                // Since we're using the command line, exit now...
                Current.Shutdown(0);
                return;
            }

            FullShutdownEnabled = true;

            // Initialization Stage #2
            // This is all stuff that only needs to be done if we're going into the program proper...

            // Does our temporary folder already exist? if so, we can nuke it
            if (Directory.Exists(TemporaryPath))
                Directory.Delete(TemporaryPath, true);

            // Create our directories
            Directory.CreateDirectory(TemporaryPath);
            Directory.CreateDirectory(AppDataPath);

            // Load Preferences
            Preferences = Preferences.Load();

            // Fix Section
            Winblows.ApplyFixes();

            // Optional Debugging Console
            if (Preferences.ShowDebugConsole)
                Debug.InitConsole();

            // Load Repositories
            PackageManager.Initialize();
            Debug.Info($"There are {PackageManager.PackageCount} packages avaliable across {PackageManager.RepositoryCount} repositories...");

            // Load Instances, Projects
            InstanceManager.Initialize();
            Debug.Info($"Managing {InstanceManager.Count} instances, {0} projects...");       

            // Construct new main window
            MainWindow = new MainWindow();
            MainWindow.Closing += (object sender, CancelEventArgs e) => { Current.Shutdown(); };   // Windows yet again sucking a big cock
            MainWindow.Show();

            // Do we want to check for updates?
            if (Preferences.AutoCheckForUpdates)
            {
                if (CheckForProgramUpdate(out UpdateInfo updateInfo))
                {
                    // Do we want the update?
                    if (MessageBox.Show($"A new version of Lawful Blade is avaliable! (Your version = {Version}, new version = {updateInfo.version})\nDo you want to update?", "Lawful Blade", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.OK)
                        PerformUpdate(updateInfo);
                }
            }
        }

        /// <summary>
        /// Event Handler.<br/>
        /// Called when the program is shut down.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnApplicationShutdown(object sender, ExitEventArgs e)
        {
            // Exit early when fullshut down is not enabled
            if (!FullShutdownEnabled)
                return;

            // Shutdown Instance Manager
            InstanceManager.Shutdown();

            // Shutdown package manager
            PackageManager.Shutdown();

            // Save Preferences
            Preferences.Save();
        }

        /// <summary>
        /// Checks for remote updates
        /// </summary>
        public bool CheckForProgramUpdate(out UpdateInfo info)
        {
            // The version info file does exist, lets download it into our temporary path
            string versionFile = Path.Combine(TemporaryPath, "version");
            string updatesFile = Path.Combine(TemporaryPath, "updates.zip");

            UpdateInfo updateInfo = new UpdateInfo();

            // Task to check if there is an update ready...
            Task<bool> checkUpdatesTask = new Task<bool>(() =>
            {
                // Check if the version info file exists...
                Uri versionInfo = new Uri(RemoteVersionURL);

                if (!DownloadManager.DownloadExists(versionInfo))
                    Debug.Warn($"Cannot retrieve version information from URL: '{RemoteVersionURL}'");
                else
                {
                    DownloadManager.DownloadSync(versionInfo, Path.Combine(TemporaryPath, versionFile));

                    // Assuming the file downloaded correctly, we can now open and read it.
                    string[] versionFileLines = File.ReadAllLines(versionFile);

                    // We must parse the versions as integers to do comparisons...
                    int currentVersion = int.Parse(Version.Strip('.'));
                    int newVersion     = int.Parse(versionFileLines[0].Strip('.').TrimEnd('D', 'S'));

                    // Only offer an update if the current version is older
                    if (currentVersion < newVersion)
                    {
                        // Copy information into the updateInfo struct
                        updateInfo.version   = versionFileLines[0];
                        updateInfo.targetUrl = versionFileLines[1];

                        // Download the update file in advance, even if the user doesn't want it. Screw 'em...
                        DownloadManager.DownloadSync(new Uri(updateInfo.targetUrl), updatesFile);

                        // Hide busy
                        BusyDialog.Instance.HideBusy();

                        // Return true because there is an update
                        return true;
                    }
                }

                // Hide busy
                BusyDialog.Instance.HideBusy();

                // Return false because there is no update
                return false;
            });
            
            // Start the check updates task
            checkUpdatesTask.Start();

            // Show the busy message
            BusyDialog.Instance.ShowBusy("So long, fare thee well. Pip pip cheerio - \r\nWe'll be back soon!");

            // Wait for the check to complete
            checkUpdatesTask.Wait();

            info = updateInfo;

            return checkUpdatesTask.Result;
        }

        public void PerformUpdate(UpdateInfo updateInfo)
        {
            MessageBox.Show("Unimplemented");
        }
    }
}