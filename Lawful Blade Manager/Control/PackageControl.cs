using LawfulBladeManager.Packages;

namespace LawfulBladeManager.Control
{
    public partial class PackageControl : UserControl
    {
        // Event is used for the selection.
        public delegate void OnSelectEvent(PackageControl packageControl);
        public event OnSelectEvent? OnSelect;

        /// <summary>
        /// Flags store basic information about a package
        /// </summary>
        public PackageStatusFlag Flags { get; private set; } = PackageStatusFlag.None;

        /// <summary>
        /// PackageReference stores a reference to the package this control is representing.
        /// </summary>
        public Package PackageReference { get; private set; }

        // Super Properties
        public Image Icon => pbIcon.Image;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="package">A package to represent with the control.</param>
        public PackageControl(Package packageReference, PackageStatusFlag flags)
        {
            // Winforms shit
            InitializeComponent();

            pbIcon.Click += OnClick;
            lbName.Click += OnClick;
            pcMain.Click += OnClick;
            pcStatus.Click += OnClick;
            lbStatus.Click += OnClick;

            // Set initial flags
            Flags = flags;

            // Load the package
            LoadPackage(PackageReference = packageReference);
        }

        /// <summary>
        /// Loads information from the package.
        /// </summary>
        /// <param name="package"></param>
        void LoadPackage(Package packageReference)
        {
            // Load Icon
            pbIcon.Image = Package.DecodeIcon(packageReference.IconBase64);

            // Load basic information
            lbName.Text = packageReference.Name;

            // Initial Update of the status label
            UpdateStatusLabel(Flags);
        }

        /// <summary>
        /// Event handler for clicking on parts of the package.
        /// </summary>
        void OnClick(object? ignore, EventArgs? these) =>
            OnSelect?.Invoke(this);

        /// <summary>
        /// Set flag(s)
        /// </summary>
        /// <param name="status">Flag(s) to set</param>
        public void SetFlags(PackageStatusFlag flags) =>
            UpdateStatusLabel(Flags |= flags);

        /// <summary>
        /// Clear flag(s)
        /// </summary>
        /// <param name="flags">Flag(s) to clear</param>
        public void ClearFlags(PackageStatusFlag flags) =>
            UpdateStatusLabel(Flags &= ~flags);

        /// <summary>
        /// Check if flag(s) is set
        /// </summary>
        /// <param name="flag">Flag(s)</param>
        /// <returns>True if the flag is set, False otherwise</returns>
        public bool GetFlag(PackageStatusFlag flag) =>
            (Flags & flag) != 0;

        /// <summary>
        /// Update the status text and colour based on flags
        /// </summary>
        /// <param name="flags">The flags to update with</param>
        void UpdateStatusLabel(PackageStatusFlag flags)
        {
            // Flags have a specific priority.
            if (GetFlag(PackageStatusFlag.Installed) && GetFlag(PackageStatusFlag.OutOfDate))
            {
                // First when a package is installed but out of date...
                lbStatus.Text = "Update Available";
                lbStatus.BackColor = Color.Yellow;
            }
            else
            if (GetFlag(PackageStatusFlag.Installed))
            {
                // Second when the package is installed
                lbStatus.Text = "Installed";
                lbStatus.BackColor = Color.DarkSeaGreen;
            }
            else
            if (GetFlag(PackageStatusFlag.Cached) && !GetFlag(PackageStatusFlag.Conflicting))
            {
                // Third when the package is ready to install, and not conflicting
                lbStatus.Text = "Ready to Install";
                lbStatus.BackColor = Color.YellowGreen;
            }
            else
            if (GetFlag(PackageStatusFlag.Cached) && GetFlag(PackageStatusFlag.Conflicting))
            {
                // Forth when the package is ready to install, but is conflicting
                lbStatus.Text = "Conflict(s)!";
                lbStatus.BackColor = Color.OrangeRed;
            }
            else
            {
                // Otherwise, the package is not ready to be installed and must be downloaded
                lbStatus.Text = "Avaliable";
                lbStatus.BackColor = Color.Navy;
            }
        }
    }
}
