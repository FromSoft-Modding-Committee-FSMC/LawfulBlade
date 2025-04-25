using ParamPro.Format;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;

using Microsoft.Win32;

namespace ParamPro.Control
{
    /// <summary>
    /// Interaction logic for PRMMapControl.xaml
    /// </summary>
    public partial class PRMMapControl : UserControl
    {
        // The currently bound asset...
        public EditorMapPRT Asset { get; private set; }

        public PRMMapControl()
        {
            InitializeComponent();

            // Piece ID Field Events
            pieceIdField.PreviewTextInput += OnPreviewInputDigitsOnly;
            pieceIdField.LostFocus += OnPieceIDLostFocus;

            // Automap Icon ID Events
            automapIconField.PreviewTextInput += OnPreviewInputDigitsOnly;

            // Enumerate all MSM, MHM and BMP files
            EnumerateMSMFiles();
            EnumerateMHMFiles();
            EnumerateBMPFiles();
        }

        /// <summary>Enumerates the avaliable MSM files into the combo box</summary>
        void EnumerateMSMFiles()
        {
            // First clear the combo box...
            msmFileField.Items.Clear();

            // Now we must scan for MSM files...
            DirectoryInfo msmDirectory = new DirectoryInfo(Path.Combine(App.TargetPath, "DATA", "MAP", "MSM"));

            foreach(FileInfo file in msmDirectory.GetFiles())
                msmFileField.Items.Add(file.Name);
        }

        /// <summary>Enumerates the avaliable MHM files into the combo box</summary>
        void EnumerateMHMFiles()
        {
            // First clear the combo box...
            mhmFileField.Items.Clear();

            // Now we must scan for MHM files...
            DirectoryInfo mhmDirectory = new DirectoryInfo(Path.Combine(App.TargetPath, "DATA", "MAP", "MHM"));

            foreach (FileInfo file in mhmDirectory.GetFiles())
                mhmFileField.Items.Add(file.Name);
        }

        /// <summary>Enumerates the avaliable BMP files into the combo box</summary>
        void EnumerateBMPFiles()
        {
            bmpFileField.Items.Clear();

            // Now we must scan for BMP files...
            DirectoryInfo bmpDirectory = new DirectoryInfo(Path.Combine(App.TargetPath, "DATA", "MAP", "ICON"));

            foreach (FileInfo file in bmpDirectory.GetFiles())
                bmpFileField.Items.Add(file.Name);
        }

        /// <summary>
        /// Loads an asset...
        /// </summary>
        public void LoadAsset(string filepath)
        {
            // Load the file
            Asset = EditorMapPRT.LoadFromFile(filepath);

            // Apply its data
            pieceIdField.Text             = $"{int.Parse(Path.GetFileNameWithoutExtension(filepath))}";   // ID is the file name...
            pieceNameField.Text           = Asset.Name;
            msmFileField.SelectedIndex    = msmFileField.Items.IndexOf(Asset.MSMFileName);
            mhmFileField.SelectedIndex    = mhmFileField.Items.IndexOf(Asset.MHMFileName);
            northOcclusionField.IsChecked = ((Asset.OccludingCardinals & 1) != 0);
            eastOcclusionField.IsChecked  = ((Asset.OccludingCardinals & 2) != 0);
            southOcclusionField.IsChecked = ((Asset.OccludingCardinals & 4) != 0);
            westOcclusionField.IsChecked  = ((Asset.OccludingCardinals & 8) != 0);
            trapDamageField.IsChecked     = ((Asset.Flags & 1) != 0);
            trapPoisonField.IsChecked     = ((Asset.Flags & 2) != 0);
            bmpFileField.SelectedIndex    = bmpFileField.Items.IndexOf(Asset.EditorIconFileName);
            automapIconField.Text         = $"{Asset.AutomapIconID}";
        }

        /// <summary>
        /// Event Callback.<br/>
        /// Called when text is entered into a box, allowing only decimal input
        /// </summary>
        void OnPreviewInputDigitsOnly(object sender, TextCompositionEventArgs e)
        {
            // Check if the added character is decimal...
            e.Handled = !char.IsDigit(e.Text[0]);
        }

        /// <summary>
        /// Event Callback.<br/>
        /// Called when focus is lost from the piece ID, to clamp it to the correct range...
        /// </summary>
        void OnPieceIDLostFocus(object sender, RoutedEventArgs e)
        {
            // Only allow 4 characters...
            if (pieceIdField.Text.Length > 3)
                pieceIdField.Text = pieceIdField.Text[..4];

            pieceIdField.Text = $"{Math.Clamp(long.Parse(pieceIdField.Text), 0, 1023)}";
        }

        /// <summary>
        /// Event Callback.<br/>
        /// Called when the Import BMP button is clicked.
        /// </summary>
        void OnClickImportBMP(object sender, RoutedEventArgs e)
        {
            // We must open a browser and configure it for images.
            // https://learn.microsoft.com/en-us/windows/win32/wic/-wic-about-windows-imaging-codec?redirectedfrom=MSDN
            // Only allowing BMP, JPG and PNG for no reason really. Can't be bothered with the filter string.
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Microsoft [B]it[M]a[P] (*.bmp)|*.bmp;*.dib|[J]oint [P]hotographic [E]xperts [G]roup (*.jpg)|*.jpg;*.jpeg|[P]ortable [N]etwork [G]raphics (*.png)|*.png"
            };

            if (ofd.ShowDialog().Value == true)
            {
                // Make sure the file exists... if it doesn't, return.
                if (!File.Exists(ofd.FileName))
                    return;

                // We should get just the name of this file...
                string targetBmp = Path.Combine(App.TargetPath, "DATA", "MAP", "ICON", Path.ChangeExtension(Path.GetFileName(ofd.FileName), "bmp"));

                // Open source image
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(ofd.FileName);
                image.DecodePixelWidth  = 20;
                image.DecodePixelHeight = 20;
                image.EndInit();

                // Convert image to 24bpp
                FormatConvertedBitmap fmbm = new FormatConvertedBitmap();
                fmbm.BeginInit();
                fmbm.Source = image;
                fmbm.DestinationFormat = PixelFormats.Rgb24;
                fmbm.EndInit();

                // Create an image encoder and add our converted image
                BitmapEncoder imageEncoder = new BmpBitmapEncoder();
                imageEncoder.Frames.Add(BitmapFrame.Create(fmbm));

                // Save the converted image to the destination file
                using FileStream fs = new FileStream(targetBmp, FileMode.Create);
                imageEncoder.Save(fs);

                // Re-emumerate BMP files
                EnumerateBMPFiles();

                // Change the BMP of the source...
                bmpFileField.SelectedIndex = bmpFileField.Items.IndexOf(Path.GetFileName(targetBmp));
            }
        }

        /// <summary>
        /// Event Callback.<br/>
        /// Called when the Import MHM button is clicked.
        /// </summary>
        void OnClickImportMHM(object sender, RoutedEventArgs e)
        {
            string[] mhmFilter =
                [
                // General Filters
                "All Supported Files|*.obj;*.fbx;*.gltf;*.glb;*.mm3d;*.ms3d;*.x;*.mhm;*.tmd;*.mo",
                "All Files|*.*",

                // Common 3D Formats
                "Wavefront [OBJ]ect|*.obj",
                "Kaydara [F]ilm[B]o[X]|*.fbx",
                "[GL] [T]ransmission [F]ormat|*.gltf;*.glb",

                // Extra 3D Formats
                "[M]ulti[M]edia [3D]|*.mm3d",
                "[M]ilk[S]hape [3D]|*.ms3d",
                "Direct [X] Retained Mode|*.x",

                // SoM Formats
                "Sword of Moonlight [???]|*.mhm",

                // Special 3D Formats
                "PlayStation [?] [M]o[D]el|*.tmd",
                "FromSoftware [MO]tion|*.mo",
            ];

            // We must open a browser and configure it for images.
            // https://learn.microsoft.com/en-us/windows/win32/wic/-wic-about-windows-imaging-codec?redirectedfrom=MSDN
            // Only allowing BMP, JPG and PNG for no reason really. Can't be bothered with the filter string.
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter      = string.Join("|", mhmFilter),
                Multiselect = false,
            };

            // Open the dialog and get the result....
            if (ofd.ShowDialog() != false)
            {
                // Get the target file name...
                string sourceFile = ofd.FileName;
                string sourceExtn = Path.GetExtension(ofd.FileName)[1..].ToUpperInvariant();
                string targetFile = Path.ChangeExtension(ofd.FileName, "MHM");

                // Check the file extension - if it's not MHM it needs converting with x2y
                switch (sourceExtn)
                {
                    // Don't do any conversion - just copy the file.
                    case "MHM":
                        
                        break;
                    
                    // Use legacy tool if X, maybe?..
                    case "X":

                        break;

                    // Anything else goes through x2y - aka x2mdl.
                    default:

                        break;

                }
            }
        }
    }
}
