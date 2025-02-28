using ImageMagick;
using LawfulBlade.Core;
using LawfulBlade.Core.Package;
using System.IO;
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
        RepositoryPackage Package;
        PackageTarget Target;

        public PackageControl(RepositoryPackage package, PackageTarget target)
        {
            InitializeComponent();

            iconField.Source = MagickImage.FromBase64(package.Icon).ToBitmapSource();
            nameField.Text   = package.Name;

            if (target.HasPackageByUUID(package.UUID))
            {
                // Package is cached put not installed...
                tagField.Text       = "Installed";
                tagField.Background = new SolidColorBrush(Color.FromRgb(48, 96, 160));
            } 
            else
            {
                // Adjust Tag Field
                if (File.Exists(Path.Combine(App.PackageCachePath, $"{package.UUID}.IAZ")))
                {
                    // Package is cached put not installed...
                    tagField.Text       = "Ready to Install";
                    tagField.Background = new SolidColorBrush(Color.FromRgb(96, 160, 48));
                }
                else
                {
                    // Package is not cached, and not installed
                    tagField.Text       = "Downloadable";
                    tagField.Background = new SolidColorBrush(Color.FromRgb(160, 96, 48));
                }
            }

            Package = package;
            Target  = target;
        }

        #region Highlighting
        void OnMouseEnter(object sender, MouseEventArgs e) =>
            Background = new SolidColorBrush(Color.FromRgb(32, 48, 64));

        void OnMouseLeave(object sender, MouseEventArgs e) =>
            Background = new SolidColorBrush(Color.FromRgb(32, 32, 32));
        #endregion
    }
}
