﻿using ImageMagick;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

using LawfulBlade.Core;
using LawfulBlade.Core.Package;

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
            // Validate 'nameField'
            if (Message.Warning("Name field cannot be empty.", (nameField.Text == string.Empty)))
                return;

            // Create instance
            Instance instance = Instance.Create(new InstanceCreateArgs
            {
                Name            = nameField.Text,
                Description     = instDescField.Text,
                IconFilePath    = lastIconPath,
                Tags            = [],
                CorePackageUUID = corePackages[coreComboBox.SelectedIndex].UUID
            });

            InstanceManager.AddInstance(instance);
            instance.Save();

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
                string magickFilter = "All Files (*.*)|*.*";

                // Build a filter form ImageMagick Supported formats...
                foreach (MagickFormatInfo format in MagickNET.SupportedFormats)
                {
                    // Skip any formats that cannot be read...
                    if (!format.SupportsReading)
                        continue;

                    magickFilter += $"|{format.Description} (*.{format.Format})|*.{format.Format}";
                }

                // Show an open file dialog
                OpenFileDialog ofd = new()
                {
                    Multiselect = false,
                    Filter = magickFilter
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
