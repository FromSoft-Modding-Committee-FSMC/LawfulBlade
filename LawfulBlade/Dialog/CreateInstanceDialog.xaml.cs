using LawfulBlade.Core;
using LawfulBlade.Core.Instance;
using LawfulBlade.Core.Package;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LawfulBlade.Dialog
{
    /// <summary>
    /// Interaction logic for CreateInstanceDialog.xaml
    /// </summary>
    public partial class CreateInstanceDialog : Window
    {
        protected string lastIconPath = string.Empty;

        protected RepositoryPackage[] corePackages;

        public CreateInstanceDialog()
        {
            InitializeComponent();

            // Some Event Binding...
            Loaded += OnLoaded;

            // Grab all core packages...
            corePackages = PackageManager.GetRepositoryPackagesByTag("Core");

            if (corePackages.Length > 0)
            {
                coreComboBox.ItemsSource = corePackages.Select(x => x.Name);
                coreComboBox.SelectedIndex = 0;
                coreComboBox.Text = corePackages[0].Name;
            }
        }

        /// <summary>
        /// Event Callback.<br/>
        /// Called when the form is loaded
        /// </summary>
        void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (corePackages.Length <= 0)
            {
                MessageBox.Show("No core packages were avaliable! An instance cannot be created!", "Lawful Blade", MessageBoxButton.OK, MessageBoxImage.Warning);
                OnCancelButton(null, null);
            }
        }

        /// <summary>
        /// Event Callback.<br/>
        /// Called when the create button is pressed.
        /// </summary>
        void OnCreateButton(object sender, RoutedEventArgs e)
        {
            // Try to create the instance...
            try
            {
                // Check 'Name'
                if (instNameField.Text == string.Empty)
                    throw new Exception("'Name' field cannot be empty!");

                Instance instance = Instance.Create(new InstanceCreateArgs
                {
                    Name            = instNameField.Text,
                    Description     = instDescField.Text,
                    IconFilePath    = lastIconPath,
                    Tags            = [],
                    CorePackageUUID = corePackages[coreComboBox.SelectedIndex].UUID
                });

                // First creation always saves...
                instance.Save();

                InstanceManager.AddInstance(instance);
            } 
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lawful Blade", MessageBoxButton.OK, MessageBoxImage.Error);
                // Return now
                return;
            }

            // On Successful creation, we want to close this dialog
            Close();
        }

        /// <summary>
        /// Event Callback.<br/>
        /// Called when the cancel button is pressed. Closes the dialog.
        /// </summary>
        void OnCancelButton(object sender, RoutedEventArgs e) =>
            Close();

        /// <summary>
        /// Event Callback.<br/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnIconMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2)
            {
                // Show an open file dialog
                OpenFileDialog ofd = new()
                {
                    Multiselect = false,
                    Filter      = "Portable Network Graphics (*.PNG)|*.png|All files (*.*)|*.*"
                };

                // Show the OFD
                if (ofd.ShowDialog() == true)
                {
                    // Make sure the file exists...
                    if (!File.Exists(ofd.FileName))
                        return;

                    instIconField.Source = new BitmapImage(new Uri(ofd.FileName));
                    lastIconPath = ofd.FileName;
                }
            }
        }
    }
}
