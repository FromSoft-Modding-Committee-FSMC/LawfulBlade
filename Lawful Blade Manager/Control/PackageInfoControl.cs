using LawfulBladeManager.Packages;

namespace LawfulBladeManager.Control
{
    public partial class PackageInfoControl : UserControl
    {
        public delegate void OnVoidEvent();
        public event OnVoidEvent? OnInstallPressed;

        public PackageInfoControl()
        {
            InitializeComponent();
        }

        public void LoadPackageControl(PackageControl packageControl)
        {
            Package package = packageControl.PackageReference;

            lbName.Text        = package.Name;
            lbUUID.Text        = package.UUID;
            tbDescription.Text = package.Description;
            lbAuthorsVal.Text  = string.Join(", ", package.Authors);
            lbVersionVal.Text  = package.Version;
            lbTagsVal.Text     = string.Join(", ", package.Tags);
            pbIcon.Image       = packageControl.Icon;
        }

        private void Install_Click(object sender, EventArgs e) =>
            OnInstallPressed?.Invoke();
    }
}
