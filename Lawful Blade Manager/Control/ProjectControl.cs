using LawfulBladeManager.Forms;
using LawfulBladeManager.Projects;
using LawfulBladeManager.Tagging;
using System.Drawing;
using static LawfulBladeManager.Control.ProjectControl;

namespace LawfulBladeManager.Control
{
    public partial class ProjectControl : UserControl
    {
        public delegate void OnProjectControlEvent(ProjectControl self);
        public event OnProjectControlEvent? OnProjectDelete;
        public event OnProjectControlEvent? OnProjectCreate;

        public ProjectControl()
        {
            InitializeComponent();
        }

        Project project;

        public ProjectControl(Project project)
        {
            InitializeComponent();

            lbTitle.Text = project.Name;
            lbDescription.Text = project.Description;

            // Try to get the icon
            if (File.Exists(Path.Combine(project.StoragePath, "icon.png")))
                pbIcon.Image = Image.FromFile(Path.Combine(project.StoragePath, "icon.png"));

            Graphics GP = Graphics.FromHwnd(Handle);

            foreach (string tag in project.Tags)
            {
                flTagList.Controls.Add(new Label
                {
                    Text = tag,
                    BackColor = Tags.MakeBackgroundColour(tag),
                    ForeColor = Tags.MakeForegroundColour(tag),
                    Size = new Size((int)GP.MeasureString(tag, lbDescription.Font).Width, 20),
                    TextAlign = ContentAlignment.MiddleCenter
                }); ;
            }

            // Disable the package manager when it's not a managed project.
            if (!project.IsManaged)
                tsFuncPackages.Enabled = false;

            // make a local copy of the project structure.
            this.project = project;
        }

        public void ClearRetainedData()
        {
            // Image Clear
            if(pbIcon.Image != null)
            {
                pbIcon.Image.Dispose();
                pbIcon.Image = null;
                pbIcon.Image = new Bitmap(Properties.Resources.defaultProjectIcon);
            }
        }

        #region Toolbar Functions
        void tsFuncDelete_Click(object sender, EventArgs e)
        {
            // Make sure to confirm the user actually wants to delete the project
            if (MessageBox.Show($"Are you sure you want to delete '{project.Name}' in '{project.StoragePath}'?\n\n" +
                "You will not be able to recover the project if you do.", "Lawful Blade", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK)
            {
                return;
            }

            ClearRetainedData();

            // Try to delete the project.
            try
            {
                Program.ProjectManager?.DeleteProject(ref project);
            } 
            catch (Exception ex)
            {
                Logger.ShortError(ex.Message);
            }
            
            // Invoke the project delete event
            OnProjectDelete?.Invoke(this);
        }

        void tsFuncPackages_Click(object sender, EventArgs e)
        {
            // Create new Package Manager Instance
            using (PackageManagerDialog pmf = new())
            {
                // Configure package destination...
                pmf.SuperFilter = new string[] { "Sample", "Project" };
                pmf.InstallationRoot = project.StoragePath;

                // Open the package manager...
                pmf.ShowDialog();
            }
        }
        #endregion
    }
}
