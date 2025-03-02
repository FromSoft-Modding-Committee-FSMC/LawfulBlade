using ImageMagick;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

using LawfulBlade.Core;
using LawfulBlade.Core.Package;
using LawfulBladeSDK.Generator;
using System.Windows.Controls;
using System.Reflection.Emit;
using System.Windows.Media;

namespace LawfulBlade.Dialog
{

    public partial class PublishProjectDialog : Window
    {
        /// <summary>
        /// Reference Instance...
        /// </summary>
        Instance Instance { get; set; }

        /// <summary>
        /// Reference Project...
        /// </summary>
        Project Project   { get; set; }

        /// <summary>
        /// Reference Generator...
        /// </summary>
        IRuntimeGenerator Generator = null;

        /// <summary>
        /// Stores a reference to generator properties grouping
        /// </summary>
        GroupBox GeneratorProperties = null;

        /// <summary>
        /// Default Constructor.<br/>
        /// </summary>
        public PublishProjectDialog(Project project)
        {
            InitializeComponent();

            // Populate the runtime fields...
            runtimeList.Items.Clear();
            runtimeList.SelectionChanged += OnRuntimeChanged;

            // Grab the instance of the project
            Instance = InstanceManager.GetInstanceByUUID(project.InstanceUUID);

            foreach (IRuntimeGenerator generator in RuntimeManager.Generators)
            {
                // Check if the generator supports this instance or project
                bool supportsInstance = false;
                foreach (string coreUUID in generator.SupportedCores)
                    supportsInstance |= Instance.HasPackageByUUID(coreUUID);

                if (!supportsInstance)
                    continue;

                runtimeList.Items.Add(generator.Name);
            }

            // Copy from project...
            nameField.Text   = project.Name;
            descField.Text   = project.Description;
            authorField.Text = Environment.GetEnvironmentVariable("USERNAME");
            iconField.Source = project.Icon.ToBitmapSource();
            targetField.Text = project.Root;

            Project = project;
        }

        /// <summary>
        /// Event Callback.<br/>
        /// </summary>
        void OnRuntimeChanged(object sender, SelectionChangedEventArgs e)
        {
            Generator = RuntimeManager.Generators[((ComboBox)sender).SelectedIndex];

            if (GeneratorProperties != null)
                propertyGroups.Children.Remove(GeneratorProperties);

            // Cache some shit
            SolidColorBrush ForegroundBrush = new SolidColorBrush(Color.FromRgb(240, 240, 240));
            SolidColorBrush BackgroundBrush = new SolidColorBrush(Color.FromRgb(32, 32, 32));
            SolidColorBrush BorderBrush     = new SolidColorBrush(Color.FromRgb(64, 64, 64));

            Thickness labelMargin = new Thickness(0, 0, 0, 2);
            Thickness proptMargin = new Thickness(0, 0, 0, 4);

            // Construct Generator Properties...
            StackPanel propertiesList = new StackPanel { Orientation = Orientation.Vertical };

            // Add each property to the list...
            foreach (GeneratorProperty property in Generator.Properties)
            {
                // Property Label
                propertiesList.Children.Add(new TextBlock { Text = property.Name, Foreground = ForegroundBrush, Margin = labelMargin });

                // Property Value   - this is fucking awful, save me from terrible code and make a PR!..
                switch (property.Type)
                {
                    // STRING VALUE
                    case Type stingType when stingType == typeof(string):
                        TextBox stringParam = new TextBox
                        {
                            Background  = BackgroundBrush,
                            BorderBrush = BorderBrush,
                            Foreground  = ForegroundBrush,
                            Margin      = proptMargin
                        };

                        stringParam.Text = (string)property.Value;
                        stringParam.TextChanged += (object obj, TextChangedEventArgs args) =>
                        {
                            property.Value = (obj as TextBox).Text;
                        };

                        propertiesList.Children.Add(stringParam);
                        break;

                    // BOOL VALUE
                    case Type boolType when boolType == typeof(bool):
                        CheckBox boolParam = new CheckBox
                        {
                            Margin = proptMargin,
                        };

                        boolParam.IsChecked = (bool)property.Value;
                        boolParam.Click += (object obj, RoutedEventArgs args) =>
                        {
                            property.Value = (obj as CheckBox).IsChecked;
                            Debug.Warn($"{(obj as CheckBox).IsChecked}");
                        };

                        propertiesList.Children.Add(boolParam);
                        break;

                    default:
                        Debug.Critical($"Unknown Generator Property Type! ({property.Type.Name})");
                        break;
                }
            }

            GeneratorProperties = new GroupBox
            {
                Header  = "Generator Properties...",
                Content = propertiesList,
                Margin  = new Thickness(0, 16, 0, 0)
            };

            propertyGroups.Children.Add(GeneratorProperties);
        }
            

        /// <summary>
        /// Event Callback.<br/>
        /// </summary>
        void OnClickTargetField(object sender, RoutedEventArgs e)
        {
            // Show the folder selector dialog...
            OpenFolderDialog dialog = new OpenFolderDialog
            {
                DefaultDirectory = targetField.Text,
                InitialDirectory = targetField.Text
            };

            // Open the dialog and get the path
            if (dialog.ShowDialog() == true)
                targetField.Text = dialog.FolderName;
        }

        /// <summary>
        /// Event Callback.<br/>
        /// Called when the cancel button is pressed.
        /// </summary>
        void OnCancelButton(object sender, RoutedEventArgs e) =>
            Close();

        /// <summary>
        /// Event Callback.<br/>
        /// Called when the confirm button is pressed.
        /// </summary>
        void OnConfirmButton(object sender, RoutedEventArgs e)
        {
            if (Message.Warning("Generator is not set!", Generator == null))
                return;

            bool publishPathExists =
                (Path.GetDirectoryName(targetField.Text) != null) |
                Directory.Exists(targetField.Text);

            if (Message.Warning("You must set the path for publishing!", !publishPathExists))
                return;

            // Bind to Generator Start Event...
            Generator.UpdateStatus += OnGeneratorUpdateStatus;

            BusyDialog.ShowBusy("Generating Runtime...");

            Generator.StartGenerator(new GeneratorStartArgs
            {
                ProjectPath  = Project.Root,
                InstancePath = Instance.Root,
                PublishPath  = targetField.Text
            });

            BusyDialog.HideBusy();

            Close();
        }

        void OnGeneratorUpdateStatus(string feedback) =>
            BusyDialog.SetBusyMessage(feedback);
    }
}
