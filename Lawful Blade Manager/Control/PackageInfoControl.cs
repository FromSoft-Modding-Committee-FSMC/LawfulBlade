using LawfulBladeManager.Packages;
using System;

namespace LawfulBladeManager.Control
{
    public partial class PackageInfoControl : UserControl
    {
        public delegate void OnVoidEvent();
        public event OnVoidEvent? OnAction;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public PackageInfoControl() =>
            InitializeComponent();

        public void LoadPackageControl(PackageControl packageControl)
        {
            Package package = packageControl.PackageReference;

            // Copy basic info...
            lbName.Text = package.Name;
            lbUUID.Text = package.UUID;
            tbDescription.Text = package.Description;
            lbAuthorsVal.Text = string.Join(", ", package.Authors);
            lbVersionVal.Text = package.Version;
            lbTagsVal.Text = string.Join(", ", package.Tags);
            pbIcon.Image = packageControl.Icon;

            // Flags have a specific priority.
            if (packageControl.GetFlag(PackageStatusFlag.Installed) && packageControl.GetFlag(PackageStatusFlag.OutOfDate))
                btAction.Text = "Update";
            else
            if (packageControl.GetFlag(PackageStatusFlag.Installed))
                btAction.Text = "Uninstall";
            else
            if (packageControl.GetFlag(PackageStatusFlag.Cached))
                btAction.Text = "Install";
            else
                btAction.Text = "Download";
        }

        /// <summary>
        /// When the action button is pressed...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClickAction(object sender, EventArgs e) =>
            OnAction?.Invoke();
    }
}
