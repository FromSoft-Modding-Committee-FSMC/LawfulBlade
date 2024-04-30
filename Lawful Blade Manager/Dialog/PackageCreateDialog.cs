using LawfulBladeManager.Tagging;
using LawfulBladeManager.Type;
using System.Drawing.Imaging;
using System.Security.Cryptography;

namespace LawfulBladeManager.Dialog
{
    public partial class PackageCreateDialog : Form
    {
        // These properties will return the package information
        public string PackageName => tbName.Text;
        public string[] PackageAuthors
        {
            get
            {
                string[] strings = tbAuthors.Text.Split(';');
                for (int i = 0; i < strings.Length; ++i)
                    strings[i] = strings[i].Trim();
                return strings;
            }
        }
        public string[] PackageTags
        {
            get
            {
                string[] strings = tbTags.Text.Split(';');

                for (int i = 0; i < strings.Length; ++i)
                    strings[i] = strings[i].Trim();

                return strings;
            }
        }
        public string PackageVersion => tbVersion.Text;
        public string PackageSource => tbSource.Text;
        public string PackageDescription => tbDescription.Text;
        public Bitmap PackageIcon => (Bitmap)pbIcon.Image;
        public string PackageOutput = string.Empty;


        public PackageCreateDialog()
        {
            InitializeComponent();
        }

        private void btCreate_Click(object sender, EventArgs e)
        {
            // Data Validation
            try
            {
                if (tbName.Text == string.Empty)
                    throw new Exception("The package field 'Name' must be filled!");
                if (tbAuthors.Text == string.Empty)
                    throw new Exception("The package field 'Author(s)' must be filled!");
                if (tbTags.Text == string.Empty)
                    throw new Exception("The package field 'Tag(s)' must be filled!");
                if (tbVersion.Text == string.Empty)
                    throw new Exception("The package field 'Version' must be filled!");
                if (tbSource.Text == string.Empty)
                    throw new Exception("The package field 'Source' must be filled!");
                if (!PathExtensions.IsValid(tbSource.Text))
                    throw new Exception("The package source is an invalid path!");

                // Validate Each Tag
                foreach (string tag in tbTags.Text.Split(';'))
                {
                    string fixedTag = tag.Trim();
                    if (fixedTag == string.Empty || !fixedTag.IsAlphanumeric())
                        throw new Exception($"The tag '{fixedTag}' is invalid (tags must only contain letters/numbers!)");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Lawful Blade", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            // Ask for the output directory
            using (SaveFileDialog sfd = new())
            {
                // Configure Dialog
                sfd.Filter = $"Lawful Blade Package (*.LBP)|*.LBP";
                sfd.InitialDirectory = tbSource.Text;

                // Do Dialog (with logic inversion)
                if (sfd.ShowDialog() != DialogResult.OK)
                    return;

                PackageOutput = sfd.FileName;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btSourceSelect_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new())
            {
                switch (fbd.ShowDialog())
                {
                    case DialogResult.OK:
                        tbSource.Text = fbd.SelectedPath;
                        break;
                }
            }
        }

        private void pbIcon_DoubleClick(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new())
            {
                // Configure the dialog
                ofd.Filter = $"All Files (*.*)|*.*";
                foreach (ImageCodecInfo codec in ImageCodecInfo.GetImageDecoders())
                    ofd.Filter = $"{ofd.Filter}|{codec.FormatDescription} Files ({codec.FilenameExtension})|{codec.FilenameExtension}";

                switch (ofd.ShowDialog())
                {
                    case DialogResult.OK:
                        try
                        {
                            pbIcon.Image = Image.FromFile(ofd.FileName);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, @"Lawful Blade", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        break;
                }
            }
        }
    }
}
