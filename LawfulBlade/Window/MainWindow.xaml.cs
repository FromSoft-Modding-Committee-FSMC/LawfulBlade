using LawfulBlade.Dialog;
using System.Windows;
using System.IO;
using System.Diagnostics;
using System.Net.NetworkInformation;
using LawfulBlade.Core;
using LawfulBlade.Control;
using LawfulBlade.Core.Instance;
using Microsoft.Win32;
using LawfulBlade.Core.Extensions;

namespace LawfulBlade
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Population
            PopulateInstanceList();

            // Populate the instance list every single time the instance manager content changes...
            InstanceManager.InstancesChanged += PopulateInstanceList;
        }

        /**
         * This region stores all events fired from the 'Instances' tab
        **/
        #region Tab - Instances
        /// <summary>
        /// Event Handler<br/>
        /// Listens for the 'Create New Instance' button
        /// </summary>
        void OnInstanceCreateInstance(object sender, RoutedEventArgs e)
        {
            (new CreateInstanceDialog()).ShowDialog();

            // After the dialog has been shown, we must repopulate our instances...
            PopulateInstanceList();
        }

        void OnInstanceImportInstance(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog ofd = new OpenFolderDialog { };

            if (ofd.ShowDialog() ?? false)  // What the fuck, microsoft?
            {
                if (!Directory.Exists(ofd.FolderName))
                    return;

                // Task to check if there is an update ready...
                Task<Instance> importInstanceTask = new Task<Instance>(() =>
                {
                    Instance newInstance = Instance.Import(ofd.FolderName);
                    newInstance.Save();

                    // BusyDialog.Instance.HideBusy();

                    return newInstance;
                });
                importInstanceTask.Start();

                // BusyDialog.Instance.ShowBusy();

                // Wait for result to be ready...
                importInstanceTask.Wait();

                InstanceManager.AddInstance(importInstanceTask.Result);

                PopulateInstanceList();
            }
        }

        /// <summary>
        /// Populates the instance list with instance controls...
        /// </summary>
        void PopulateInstanceList()
        {
            // Kill off any old instance controls
            instanceList.Children.Clear();

            // Populate with new instance controls based on the currently held instances...
            foreach(Instance instance in InstanceManager.Instances)
            {
                // Create an instance control for this instance
                InstanceControl instanceControl = new InstanceControl(instance)
                {
                    // Initial configuration requirements, because WPF sorta sucky
                    Margin = new Thickness(1, 1, 1, 0),
                    Height = 128
                };

                // Shove this into the instance list...
                instanceList.Children.Add(instanceControl);
            }
        }

        #endregion

        /**
         * This region stores all events fired from the 'File' section of the menu
        **/
        #region Menu - File

        /// <summary>
        /// Event Handler<br/>
        /// Opens up the preferences dialog when called
        /// </summary>
        void OnMenuFilePreferences(object sender, RoutedEventArgs e) =>
            new PreferencesDialog().ShowDialog();

        /// <summary>
        /// Event Handler<br/>
        /// Listens for the 'Exit' button
        /// </summary>
        void OnMenuFileExit(object sender, RoutedEventArgs e) =>
            Application.Current.Shutdown();

        #endregion

        /**
         * This region stores all events fired from the 'Help' section of the menu
        **/
        #region Menu - Help

        /// <summary>
        /// Event Handler<br/>
        /// Listens for the 'Check for Updates' button in the help menu
        /// </summary>
        void OnMenuHelpCheckForUpdates(object sender, RoutedEventArgs e)
        {
            if(((App)Application.Current).CheckForProgramUpdate(out UpdateInfo updateInfo))
            {
                // Do we want the update?
                if (MessageBox.Show($"A new version of Lawful Blade is avaliable! (Your version = {App.Version}, new version = {updateInfo.version})\nDo you want to update?", "Lawful Blade", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.OK)
                    ((App)Application.Current).PerformUpdate(updateInfo);
            }
            else
                MessageBox.Show("Up to date!", "Lawful Blade", MessageBoxButton.OK, MessageBoxImage.Information);
        }
            
        /// <summary>
        /// Event Handler<br/>
        /// Listens for the 'Report a Problem' button in the help menu
        /// </summary>
        void OnMenuHelpReportAProblem(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!NetworkInterface.GetIsNetworkAvailable())
                    throw new Exception("No network connection!");

                if (Process.Start(new ProcessStartInfo() { FileName = "explorer.exe", Arguments = App.ReportAProblemURL }) == null)
                    throw new Exception($"Couldn't start browser process with URL: '{App.ReportAProblemURL}'");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lawful Blade", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Event Handler<br/>
        /// Listens for the 'Release Notes' button in the help menu
        /// </summary>
        void OnMenuHelpReleaseNotes(object sender, RoutedEventArgs e)
        {
            try
            {
                // Does the release notes file exist?
                if (!File.Exists(App.ReleaseNotesFile))
                    throw new Exception($"Couldn't find release notes: '{App.ReleaseNotesFile}'");

                // Are we successful in starting a process to read the notes?
                if (Process.Start(new ProcessStartInfo() { FileName = App.ReleaseNotesFile, UseShellExecute = true }) == null)
                    throw new Exception($"Couldn't start text editor with file: '{App.ReleaseNotesFile}'");
            } 
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lawful Blade", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Event Handler<br/>
        /// Listens for the 'About' button in the help menu
        /// </summary>
        void OnMenuHelpAbout(object sender, RoutedEventArgs e) =>
            new AboutDialog().ShowDialog();

        #endregion

    }
}