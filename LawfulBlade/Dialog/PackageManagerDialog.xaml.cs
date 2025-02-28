using LawfulBlade.Control;
using LawfulBlade.Core;
using LawfulBlade.Core.Package;
using System.Security.Policy;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LawfulBlade.Dialog
{
    /// <summary>
    /// Interaction logic for PackageManagerDialog.xaml
    /// </summary>
    public partial class PackageManagerDialog : Window
    {
        /// <summary>
        /// Currently avaliable repository packages
        /// </summary>
        readonly RepositoryPackage[] avaliablePackages;
        readonly List<string>   activeTags;
        readonly List<CheckBox> enabledTags;

        /// <summary>
        /// The target for package installation
        /// </summary>
        PackageTarget Target;

        /// <summary>
        /// Default Constructor.<br/>
        /// </summary>
        public PackageManagerDialog(PackageTarget target)
        {
            InitializeComponent();

            // Get every single package...
            avaliablePackages = PackageManager.GetRepositoryPackages();

            activeTags = [];

            foreach (RepositoryPackage package in avaliablePackages)
            {
                // Find unique tags across the packages...
                foreach (string tag in package.Tags)
                {
                    // Hardcoding skips for these for now
                    switch (tag.ToUpperInvariant())
                    {
                        case "CORE":
                        case "EDITOR":
                            continue;
                    }

                    if (activeTags.Contains(tag))
                        continue;

                    activeTags.Add(tag);
                }
            }

            // Place unique tags in the tag list...
            filterList.Children.Clear();
            filterList.Children.Add(new TextBlock
            {
                Margin  = new Thickness(0, 0, 0, 4),
                Padding = new Thickness(6, 2, 6, 2),
                Background = Brushes.Black,
                Foreground = Brushes.WhiteSmoke,
                Text = "Filters"
            });

            enabledTags = [];

            foreach (string uniqueTag in activeTags)
            {
                // Horizontal stack panel for each tag
                StackPanel tagPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(4, 0, 0, 0),
                };

                CheckBox tagCheckbox = new CheckBox
                {
                    IsChecked = true,
                    Tag = activeTags.IndexOf(uniqueTag),
                };
                tagCheckbox.Click += OnTagChecked;
                tagPanel.Children.Add(tagCheckbox);

                enabledTags.Add(tagCheckbox);

                tagPanel.Children.Add(new TextBlock
                {
                    Margin     = new Thickness(2, 0, 0, 0),
                    Foreground = Brushes.WhiteSmoke,
                    Text       = uniqueTag
                });

                filterList.Children.Add(tagPanel);
            }

            Target = target;

            PopulatePackages();
        }

        void OnTagChecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkbox = ((CheckBox)sender);

            if (checkbox == null)
                return;

            PopulatePackages();
        }

        void PopulatePackages()
        {
            List<RepositoryPackage> repoPackage = [];

            foreach (CheckBox cb in enabledTags)
            {
                // Is this checkbox enabled?
                if (!(cb.IsChecked?? false))
                    continue;

                // Get the tag name...
                string tagName = activeTags[(int)cb.Tag];

                // Scan each package...
                foreach (RepositoryPackage package in avaliablePackages)
                {
                    // Skip if this package was already included
                    if (repoPackage.Contains(package) || !package.Tags.Contains(tagName))
                        continue;

                    // Add the package...
                    repoPackage.Add(package);
                }
            }

            // Now we can create controls for each filtered package.
            packageList.Children.Clear();

            foreach (RepositoryPackage package in repoPackage)
            {
                packageList.Children.Add(new PackageControl(package, Target)
                {
                    Margin = new Thickness(1, 1, 1, 0),
                    Height = 48
                });
            }
        }

        /// <summary>
        /// Event Handler.<br/>
        /// </summary>
        void OnClickDone(object sender, RoutedEventArgs e) =>
            Close();
    }
}
