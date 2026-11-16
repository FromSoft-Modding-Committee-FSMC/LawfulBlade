using LawfulBlade.Core;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LawfulBlade.Control
{
    /// <summary>
    /// Interaction logic for RepositoryControl.xaml
    /// </summary>
    public partial class RepositoryControl : UserControl
    {
        public Repository Repository { get; private set; }

        public event Action<Repository> DeleteRepository;

        public RepositoryControl(Repository repository)
        {
            InitializeComponent();

            // Repository Status...
            SetupRepositoryStatus(repository);

            // Repository URL...
            SetupRepositoryURL(repository);

            // Repository Button...
            SetupRepositoryButton(repository);

            Repository = repository;
        }

        void SetupRepositoryStatus(Repository repository)
        {
            switch (repository.Avaliable)
            {
                case true:
                    statusField.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x80, 0xF0, 0x80));
                    statusField.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x10, 0x20, 0x10));
                    statusField.Text = $"OK!";
                    break;

                case false:
                    statusField.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0xF0, 0x80, 0x80));
                    statusField.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x20, 0x10, 0x10));
                    statusField.Text = $"FAIL";
                    break;
            }
        }

        void SetupRepositoryURL(Repository repository)
        {
            urlField.Text      = repository.URI;
            urlField.IsEnabled = false;
        }

        void SetupRepositoryButton(Repository repository)
        {
            deleteButton.IsEnabled  = !repository.URI.Contains("file:///StandardLibrary");
            deleteButton.Opacity    = deleteButton.IsEnabled ? 1.0 : 0.0;
        }

        /// <summary>
        /// Event Callback.<br/>
        /// </summary>
        void OnClickDelete(object sender, RoutedEventArgs e)
        {
            if (!Message.WarningYesNo("Deleting this repository will stop you from being able to access any of packages it includes.  Are you sure you want to delete it?", true))
                return;

            DeleteRepository?.Invoke(Repository);
        }
    }
}
