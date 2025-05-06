using ImageMagick;
using LawfulBlade.Core;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace LawfulBlade.Dialog
{
    public partial class CreateProjectDialog : Window
    {
        /// <summary>
        /// Default Constructor.<br/>
        /// </summary>
        public CreateProjectDialog()
        {
            InitializeComponent();

            // Event Bindings
            Loaded += OnLoaded;

            // Populate the instance field...
            if (InstanceManager.Count > 0)
            {
                instanceField.ItemsSource = InstanceManager.Instances.Select(x => x.Name);
                instanceField.SelectedIndex = 0;
                instanceField.Text = InstanceManager.Instances[0].Name;
            }
        }

        /// <summary>
        /// Event Callback.<br/>
        /// When there are no instances, this event gets fired.
        /// </summary>
        void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (Message.Warning("No instances have been created! You must create an instance before you create a project!", (InstanceManager.Count == 0)))
                OnCancelButton(null, null);
        }

        /// <summary>
        /// Event Callback.<br/>
        /// Called when the create button is pressed.
        /// </summary>
        void OnCreateButton(object sender, RoutedEventArgs e)
        {
            // Validate Name
            if (Message.Warning("Name field cannot be empty.", (nameField.Text == string.Empty)))
                return;

            // Retrieve the icon file path from the source
            string iconFilePath = string.Empty;
            if (((BitmapImage)iconField.Source) != null)
            {
                if (((BitmapImage)iconField.Source).UriSource != null)
                    iconFilePath = ((BitmapImage)iconField.Source).UriSource.AbsolutePath;
            }

            // Create the project
            Project project = Project.Create(new ProjectCreateArgs
            {
                Name        = nameField.Text,
                Description = descField.Text,
                Author      = authorField.Text,
                IconFile    = iconFilePath,
                Owner       = InstanceManager.Instances[instanceField.SelectedIndex],
            });

            ProjectManager.AddProject(project);
            project.Save();

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
                foreach(MagickFormatInfo format in MagickNET.SupportedFormats)
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
                    Filter      = magickFilter
                };
                
                // Show the OFD
                if (ofd.ShowDialog() == true)
                {
                    // Make sure the file exists...
                    if (!File.Exists(ofd.FileName))
                        return;

                    iconField.Source = new BitmapImage(new Uri(ofd.FileName));
                }
            }
        }
    }
}
