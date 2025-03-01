using ImageMagick;
using LawfulBlade.Core;
using LawfulBlade.Core.Extensions;
using LawfulBlade.Dialog;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace LawfulBlade.Control
{
    /// <summary>
    /// Interaction logic for InstanceControl.xaml
    /// </summary>
    public partial class InstanceControl : UserControl
    {
        public Instance Instance { get; private set; }

        public InstanceControl(Instance instance)
        {
            // Initialize the control
            InitializeComponent();

            // Now copy data from the instance...
            instIconField.Source = instance.IconImage.ToBitmapSource();
            instNameField.Text   = instance.Name;
            instDescField.Text   = instance.Description;

            // Load tags from the instance
            tagList.Children.Clear();

            foreach(string tag in instance.Tags)
            {
                // Add all letters of tag together
                int tagValue = 0;
                foreach (char c in tag)
                    tagValue += c;

                // Calculate the background colour of the tag...
                Color background;
                background = (tagValue % 7) switch
                {
                    0 => Color.FromRgb(0xE7, 0x4C, 0x3C),
                    1 => Color.FromRgb(0x2E, 0xCC, 0x71),
                    2 => Color.FromRgb(0x34, 0x98, 0xDB),
                    3 => Color.FromRgb(0xF1, 0xC4, 0x0F),
                    4 => Color.FromRgb(0x8E, 0x44, 0xAD),
                    5 => Color.FromRgb(0x1A, 0xBC, 0x9C),
                    _ => Color.FromRgb(0xBD, 0xC3, 0xC7)
                };

                background = background.Mix(((tagValue ^ 0x4) % 7) switch
                {
                    0 => Color.FromRgb(0xA9, 0x32, 0x26),
                    1 => Color.FromRgb(0x22, 0x99, 0x54),
                    2 => Color.FromRgb(0x24, 0x71, 0xA3),
                    3 => Color.FromRgb(0xD6, 0x89, 0x10),
                    4 => Color.FromRgb(0x5B, 0x2C, 0x6F),
                    5 => Color.FromRgb(0x13, 0x8D, 0x75),
                    _ => Color.FromRgb(0x70, 0x7B, 0x7C)
                }, 0.5f);

                // Calculate the foreground colour of the tag
                int b = (int)(255f * background.GetSaturation() + background.GetBrightness()) / 2;

                tagList.Children.Add(new TextBlock
                {
                    Text       = tag,
                    Foreground = new SolidColorBrush(Color.FromRgb((byte)b, (byte)b, (byte)b)),
                    Background = new SolidColorBrush(background),
                    Margin     = new Thickness(0, 0, 8, 0),
                    Padding    = new Thickness(4, 0, 4, 0)
                });
            }

            Instance = instance;
        }

        /// <summary>
        /// Event Callback.<br/>
        /// Called when a user clicks the delete button.
        /// </summary>
        void OnClickDelete(object sender, RoutedEventArgs e)
        {
            // Maybe move this UI code somewhere else...
            if (MessageBox.Show("Warning! This will remove the instance from Lawful Blade and the file system! Are you sure you want to continue?", "Lawful Blade", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                return;

            // Delete Instance Content...
            InstanceManager.RemoveInstance(Instance);
            Instance.Delete();
        }

        /// <summary>
        /// Event Callback.<br/>
        /// Creates a shortcut to the instance when pressed.
        /// </summary>
        void OnClickCreateShortcut(object sender, RoutedEventArgs e) =>
            Instance.CreateShortcut();

        /// <summary>
        /// Event Callback.<br/>
        /// Opens the package manager dialog with this instance as the target
        /// </summary>
        void OnClickManagePackages(object sender, RoutedEventArgs e)
        {
            // Show the Package Manager Dialog
            (new PackageManagerDialog(Instance)).ShowDialog();

            Instance.Save();
        }

        /// <summary>
        /// Event Callback.<br/>
        /// Launches the instance.
        /// </summary>
        void OnClickLaunch(object sender, RoutedEventArgs e) =>
            Instance.Launch(null, false);

        #region Highlighting
        void OnMouseEnter(object sender, MouseEventArgs e) =>
            Background = new SolidColorBrush(Color.FromRgb(32, 48, 64));

        void OnMouseLeave(object sender, MouseEventArgs e) =>
            Background = new SolidColorBrush(Color.FromRgb(32, 32, 32));
        #endregion
    }
}
