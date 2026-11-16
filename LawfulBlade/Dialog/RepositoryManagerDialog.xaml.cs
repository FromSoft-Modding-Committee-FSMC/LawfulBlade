using LawfulBlade.Control;
using LawfulBlade.Core;
using LawfulBlade.Core.Package;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for RepositoryManagerDialog.xaml
    /// </summary>
    public partial class RepositoryManagerDialog : Window
    {
        public RepositoryManagerDialog()
        {
            InitializeComponent();

            // Populate initial values
            PopulateRepositories();
        }

        void PopulateRepositories()
        {
            // Delete any old items in the UI
            foreach (object item in repositoryList.Children)
                (item as RepositoryControl).DeleteRepository -= OnDeleteRepository;

            repositoryList.Children.Clear();

            // Add new repositories
            foreach (Repository repository in PackageManager.Repositories)
            {
                if (repository == null)
                    continue;

                RepositoryControl repositoryControl = new RepositoryControl(repository);
                repositoryControl.DeleteRepository += OnDeleteRepository;

                repositoryList.Children.Add(repositoryControl);
            }
        }

        void OnDeleteRepository(Repository repository)
        {
            // Search out and remove the target repository...
            PackageManager.Repositories.Remove(repository);

            // Repopulate the list...
            PopulateRepositories();
        }

        void OnAddRepository(string uri)
        {
            if (Message.Warning("Repository does not exist, or is not avaliable.", !DownloadManager.DownloadExists(new Uri(uri))))
                return;

            // Load in the new repository...
            PackageManager.LoadRepository(uri);

            // Repopulate the list...
            PopulateRepositories();
        }

        void OnClickClose(object sender, RoutedEventArgs e)
        {
            PackageManager.Shutdown();
            PackageManager.Initialize();
            Debug.Info($"There are {PackageManager.PackageCount} packages avaliable across {PackageManager.RepositoryCount} repositories...");

            Close();
        }

        void OnClickAdd(object sender, RoutedEventArgs e)
        {
            try
            {
                OnAddRepository(urlField.Text);
            }
            catch (Exception ex)
            {
                Debug.Error($"Failed to add repository '{urlField.Text}': {ex.Message}");
                Debug.Critical($"\n{ex.StackTrace}");
            }
        }
    }
}
