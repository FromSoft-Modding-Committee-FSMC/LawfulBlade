using LawfulBladeManager.Packages;
using LawfulBladeManager.Type;
using System.Drawing.Imaging;

namespace LawfulBladeManager.Dialog
{
    public partial class PackageCreateDialog : Form
    {
        /// <summary>
        /// Arguments for the create package call.
        /// </summary>
        public PackageCreateArgs? CreationArguments { get; private set; }

        /// <summary>
        /// Default Constructor.<br/>
        /// Can take an optional package as a parameter, 
        /// </summary>
        /// <param name="package">Optional. Loads information from this package as the defaults.</param>
        public PackageCreateDialog(Package? package = null)
        {
            InitializeComponent();

            // If package is null, we are creating one from scratch.
            if (package == null)
                return;

            // Load initial data from the supplied package...
            tbName.Text        = package.Name;
            tbDescription.Text = package.Description;
            tbVersion.Text     = package.Version;
            tbAuthors.Text     = MergeStringSpecial(package.Authors);
            tbTags.Text        = MergeStringSpecial(package.Tags);
            tbSource.Text      = string.Empty;  // Source will not be copied - it is assumed a new source will be provided.
            pbIcon.Image       = Package.DecodeIcon(package.IconBase64);
        }

        /// <summary>
        /// Splits the tags or authors string into an array
        /// </summary>
        /// <param name="str">String, seperated with colons</param>
        /// <returns>String array</returns>
        static string[] SplitStringSpecial(string str)
        {
            // Basic sanity: if the last character is a semi colon, the user didn't know any better. is oki.
            str = str.TrimEnd(';');

            // Split the string with semi colon
            string[] splits = str.Split(';');

            // Clean up each string of spaces
            for(int i = 0; i < splits.Length; i++)
                splits[i] = splits[i].Trim();

            return splits;
        }

        /// <summary>
        /// Merges the tags or authors string array into a single string
        /// </summary>
        /// <param name="strs">String array</param>
        /// <returns>String, seperated with colons</returns>
        static string MergeStringSpecial(string[] strs) =>
            string.Join(';', strs);

        /// <summary>
        /// Callback event for when the user clicks the "Create" button
        /// </summary>
        void OnClickCreate(object sender, EventArgs e)
        {
            // Data Validation
            try
            {
                // Validate simple properties
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

                // Validate Tags
                string[] tempTags = SplitStringSpecial(tbTags.Text);

                if(tempTags.Length == 0)
                    throw new Exception($"You must specify at least one tag! ('Runtime', 'Project', 'Editor')\n\nCheck the documentation for a list of default tags!");

                foreach (string tag in tempTags)
                    if (tag == string.Empty || !tag.IsAlphanumeric())
                        throw new Exception($"The tag '{tag}' is invalid (tags must only contain letters/numbers!)");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lawful Blade", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            // Lets find the output directory
            using SaveFileDialog sfd = new()
            {
                Filter           = "Possibly A Zip (*.paz)|*.paz",
                DefaultExt       = "paz",
                FileName         = "package.paz",

                InitialDirectory = tbSource.Text,
                CheckPathExists  = true,
                CheckWriteAccess = true,
            };
                
            if (sfd.ShowDialog() != DialogResult.OK)
                return;

            // Set the creation arguments
            CreationArguments = new PackageCreateArgs
            {
                SourceDirectory  = tbSource.Text,
                TargetFile       = sfd.FileName,
                Name             = tbName.Text,
                Description      = tbDescription.Text,
                Version          = tbVersion.Text,
                Authors          = SplitStringSpecial(tbAuthors.Text),
                Tags             = SplitStringSpecial(tbTags.Text),
                IconSource       = (Bitmap)pbIcon.Image,
                ExpectOverwrites = xbExpectOverwrites.CheckState == CheckState.Checked
            };

            // Finish with the dialog by setting the result to OK and closing it.
            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// Callback event for when the user clicks the "Cancel" button
        /// </summary>
        void OnClickCancel(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        /// <summary>
        /// Callback event for when the user clicks the "Select" button
        /// </summary>
        void OnClickSelect(object sender, EventArgs e)
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

        /// <summary>
        /// Callback event for when the user double clicks the icon
        /// </summary>
        void OnDoubleClickIcon(object sender, EventArgs e)
        {
            // Create the open file dialog
            using OpenFileDialog ofd = new()
            {
                Filter = $"All Files (*.*)|*.*"
            };

            // Add any additional supported image codec...
            foreach(ImageCodecInfo imageCodec in ImageCodecInfo.GetImageDecoders())
                ofd.Filter = $"{ofd.Filter}|{imageCodec.FormatDescription} Files ({imageCodec.FilenameExtension})|{imageCodec.FilenameExtension}";

            // Make sure a file was selected...
            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            // Try to create the icon
            try
            {
                pbIcon.Image = Image.FromFile(ofd.FileName);
            }
            catch (Exception ex)
            {
                // we failed - Set back to default
                pbIcon.Image = Properties.Resources._128x_package;

                // tell the user something fucked up.
                MessageBox.Show(ex.Message, "Lawful Blade", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
