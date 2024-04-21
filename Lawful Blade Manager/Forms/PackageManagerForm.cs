namespace LawfulBladeManager.Forms
{
    public partial class PackageManagerForm : System.Windows.Forms.Form
    {
        public PackageManagerForm()
        {
            InitializeComponent();
        }

        private void PackageManagerForm_Load(object sender, EventArgs e)
        {
            // Load Filters
            lvPackageFilter.Items.Clear();

        }
    }
}
