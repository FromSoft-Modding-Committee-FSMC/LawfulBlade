using ImageMagick;
using LawfulBlade.Core;
using LawfulBlade.Dialog;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace LawfulBlade.Control
{
    public partial class ProjectControl : UserControl
    {
        public Project Project { get; private set; }

        public ProjectControl(Project project)
        {
            InitializeComponent();

            nameField.Text   = project.Name;
            descField.Text   = project.Description;
            iconField.Source = project.Icon.ToBitmapSource();

            Project = project;
        }

        /// <summary>
        /// Event Callback.<br/>
        /// Called when a user clicks the delete button.
        /// </summary>
        void OnClickDelete(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Warning! This will remove the project from Lawful Blade and the file system! Are you sure you want to continue?", "Lawful Blade", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                return;

            // Delete Project Content...
            ProjectManager.RemoveProject(Project);
            Project.Delete();
        }

        /// <summary>
        /// Event Callback.<br/>
        /// Creates a shortcut to the project when pressed.
        /// </summary>
        void OnClickCreateShortcut(object sender, RoutedEventArgs e) =>
            Project.CreateShortcut();

        /// <summary>
        /// Event Callback.<br/>
        /// Creates a shortcut to the instance when pressed.
        /// </summary>
        void OnClickCreateRuntime(object sender, RoutedEventArgs e)
        {
            (new PublishProjectDialog(Project)).ShowDialog();
        }

        /// <summary>
        /// Event Callback.<br/>
        /// Launches the project.
        /// </summary>
        void OnClickLaunch(object sender, RoutedEventArgs e) =>
            Project.Launch(false);

        #region Highlighting
        void OnMouseEnter(object sender, MouseEventArgs e) =>
            Background = new SolidColorBrush(Color.FromRgb(32, 48, 64));

        void OnMouseLeave(object sender, MouseEventArgs e) =>
            Background = new SolidColorBrush(Color.FromRgb(32, 32, 32));
        #endregion
    }
}
