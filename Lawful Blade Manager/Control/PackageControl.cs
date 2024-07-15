using LawfulBladeManager.Packages;
using System.Data;
using System.IO.Compression;

namespace LawfulBladeManager.Control
{
    public enum PackageStatus
    {
        Uninstalled,
        Installed,
        Conflicts
    }

    public partial class PackageControl : UserControl
    {
        // Event is used for the selection.
        public delegate void OnSelectEvent(PackageControl packageControl);
        public event OnSelectEvent? OnSelect;

        // Information properties about the package
        public PackageStatus Status { get; private set; } = PackageStatus.Uninstalled;
        public PackageFile[] Files { get; set; }

        public Package package;
        public Image? icon => pbIcon.Image;
        public ZipArchive? PackageZip { get; set; }


        // Default Constructor
        public PackageControl(Package package)
        {
            InitializeComponent();

            // Load Package Icon
            pbIcon.Image = Package.DecodeIcon(package.IconBase64);

            // Load Package Info
            lbName.Text = package.Name;

            // Store package reference
            this.package = package;

            // Default Files...
            Files = Array.Empty<PackageFile>();

            // Set Initial Status
            UpdateStatus(Status);

            // Bind to the various clicks we need to capture...
            pbIcon.Click += CommonClick;
            lbName.Click += CommonClick;
            pcMain.Click += CommonClick;
            pcStatus.Click += CommonClick;
            lbStatus.Click += CommonClick;
        }

        void CommonClick(object? fuck, EventArgs? off) =>
            OnSelect?.Invoke(this);

        /// <summary>
        /// Updates the status of a package
        /// </summary>
        /// <param name="status">The new status</param>
        public void UpdateStatus(PackageStatus status)
        {
            // Status Text
            lbStatus.Text = status switch
            {
                PackageStatus.Uninstalled => "Uninstalled",
                PackageStatus.Installed => "Installed",
                PackageStatus.Conflicts => "Conflict(s)!",
                _ => "Unknown"
            };

            // Status Colour
            lbStatus.BackColor = status switch
            {
                PackageStatus.Uninstalled => Color.Tomato,
                PackageStatus.Installed => Color.Lime,
                PackageStatus.Conflicts => Color.DarkOrange,
                _ => Color.Black
            };

            Status = status;
        }
    }
}
