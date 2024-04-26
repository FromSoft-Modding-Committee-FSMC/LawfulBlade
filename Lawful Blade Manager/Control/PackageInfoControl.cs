using LawfulBladeManager.Packages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LawfulBladeManager.Control
{
    public partial class PackageInfoControl : UserControl
    {
        public PackageInfoControl()
        {
            InitializeComponent();
        }

        public void LoadMetadata(Package package)
        {
            lbName.Text = package.Name;
            lbUUID.Text = package.UUID;
            tbDescription.Text = package.Description;
            lbAuthorsVal.Text = string.Join(", ", package.Authors);
            lbVersionVal.Text = package.Version;
            lbTagsVal.Text = string.Join(", ", package.Tags);
        }

        public void LoadIcon(Image? icon)
        {
            if (icon == null)
                return;

            pbIcon.Image = icon;
        }
    }
}
