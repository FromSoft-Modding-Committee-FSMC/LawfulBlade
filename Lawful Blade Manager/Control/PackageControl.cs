using LawfulBladeManager.Packages;

namespace LawfulBladeManager.Control
{
    public partial class PackageControl : UserControl
    {
        public Package package;
        public Image? icon => pbIcon.Image;

        public PackageControl(Package package, Image? icon)
        {
            InitializeComponent();

            // Load Package Icon
            if (icon != null)
                pbIcon.Image = icon;

            // Load Package Info
            lbName.Text = package.Name;

            // Store package reference
            this.package = package;
        }
    }
}
