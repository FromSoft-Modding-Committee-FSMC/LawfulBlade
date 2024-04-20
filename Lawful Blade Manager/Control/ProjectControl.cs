using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using LawfulBladeManager.Projects;

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

            foreach (int tag in project.TagIDs)
            {
                flTagList.Controls.Add(new Label
                {
                    Text = Tagging.Tag.TagList[tag].Text,
                    BackColor = Tagging.Tag.TagList[tag].BackgroundColour,
                    ForeColor = Tagging.Tag.TagList[tag].ForegroundColour,
                    Size = new Size(96, 20),
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
            if(MessageBox.Show($"Are you sure you want to delete {project.Name}?\n\n" +
                "You will not be able to recover the project if you do.", "Lawful Blade", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            {
                return;
            }

        }
        #endregion
    }
}
