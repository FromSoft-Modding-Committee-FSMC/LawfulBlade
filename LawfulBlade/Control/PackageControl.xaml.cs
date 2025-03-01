using ImageMagick;
using LawfulBlade.Core;
using LawfulBlade.Core.Package;
using LawfulBlade.Dialog;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


namespace LawfulBlade.Control
{
    /// <summary>
    /// Interaction logic for PackageControl.xaml
    /// </summary>
    public partial class PackageControl : UserControl
    {
        enum Status
        {
            NeedUpdate,
            Installed,
            Downloaded,
            NotDownloaded
        }

        PackageTarget Target;
        Status status;

        public RepositoryPackage Package { get; private set; }
        
        public PackageControl(RepositoryPackage package, PackageTarget target)
        {
            InitializeComponent();

            iconField.Source = MagickImage.FromBase64(package.Icon).ToBitmapSource();
            nameField.Text   = package.Name;

            Package = package;
            Target  = target;

            UpdateStatus();
        }

        /// <summary>
        /// Updates the status of the package control according to currenlty installed packages...
        /// </summary>
        public void UpdateStatus()
        {
            if (Target.HasPackageByUUID(Package.UUID))
            {
                // Package is cached put not installed...
                buttonText.Text     = "Installed!";
                buttonBG.Background = new SolidColorBrush(Color.FromRgb(48, 96, 160));

                status = Status.Installed;
            }
            else
            {
                // Adjust Tag Field
                if (File.Exists(Path.Combine(App.PackageCachePath, $"{Package.UUID}.IAZ")))
                {
                    // Package is cached put not installed...
                    buttonText.Text     = "Install";
                    buttonBG.Background = new SolidColorBrush(Color.FromRgb(96, 160, 48));

                    status = Status.Downloaded;
                }
                else
                {
                    // Package is not cached, and not installed
                    buttonText.Text     = "Download";
                    buttonBG.Background = new SolidColorBrush(Color.FromRgb(160, 96, 48));

                    status = Status.NotDownloaded;
                }
            }
        }

        #region Highlighting
        void OnMouseEnter(object sender, MouseEventArgs e) =>
            Background = new SolidColorBrush(Color.FromRgb(32, 48, 64));

        void OnMouseLeave(object sender, MouseEventArgs e) =>
            Background = new SolidColorBrush(Color.FromRgb(32, 32, 32));
        #endregion

        /// <summary>
        /// Event Handler.<br/>
        /// Called when the mouse clicks the action button...
        /// </summary>
        void OnClickAction(object sender, RoutedEventArgs e)
        {
            switch (status)
            {
                case Status.Installed:
                    break;

                    // No = True prompt. Make sure the user wants to do this...
                    if (MessageBox.Show($"Are you sure you want to uninstall {Package.Name} from this instance?", "Lawful Blade", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                        return;

                    //

                    break;

                case Status.Downloaded:
                    // No = True prompt. Make sure the user wants to do this...
                    if (MessageBox.Show($"Are you sure you want to install {Package.Name} to this instance? You will not be able to uninstall it afterwards!", "Lawful Blade", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                        return;

                    // 
                    BusyDialog.ShowBusy();
                    Target.InstallPackage(PackageManager.GetPackageByUUID(Package.UUID));
                    BusyDialog.HideBusy();

                    break;

                case Status.NotDownloaded:
                    BusyDialog.ShowBusy();
                    PackageManager.GetPackageByUUID(Package.UUID);
                    BusyDialog.HideBusy();
                    break;
            }

            UpdateStatus();
        }
    }
}
