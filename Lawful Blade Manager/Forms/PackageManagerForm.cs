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
            foreach (Tagging.Tag tag in Tagging.Tag.TagList)
            {
                lvPackageFilter.Items.Add(tag.Text);
                lvPackageFilter.SetItemChecked(lvPackageFilter.Items.Count - 1, false);
            }

        }
    }
}
