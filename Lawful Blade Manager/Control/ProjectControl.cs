using LawfulBladeManager.Forms;
using LawfulBladeManager.Projects;
using LawfulBladeManager.Tagging;

namespace LawfulBladeManager.Control
{
    public partial class ProjectControl : UserControl
    {
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
            if(File.Exists(Path.Combine(project.StoragePath, "icon.png")))
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
            if(!project.IsManaged)
                tsFuncPackages.Enabled = false;

            // make a local copy of the project structure.
            this.project = project;
        }

        #region Toolbar Functions
        private void tsFuncDelete_Click(object sender, EventArgs e)
        {
            // Make sure to confirm the user actually wants to delete the project
            if(MessageBox.Show($"Are you sure you want to delete '{project.Name}' in '{project.StoragePath}'?\n\n" +
                "You will not be able to recover the project if you do.", "Lawful Blade", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK)
            {
                return;
            }

            // Delete the project.
            Program.ProjectManager?.DeleteProject(project.StoragePath);
        }
        #endregion
    }
}
