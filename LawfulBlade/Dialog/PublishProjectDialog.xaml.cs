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

        IRuntimeGenerator Generator = null;

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
        void OnRuntimeChanged(object sender, SelectionChangedEventArgs e) =>
            Generator = RuntimeManager.Generators[((ComboBox)sender).SelectedIndex];

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

            Generator.StartGenerator(new GeneratorStartArgs
            {
                ProjectPath  = Project.Root,
                InstancePath = Instance.Root,
                PublishPath  = targetField.Text
            });

            Close();
        }
    }
}
