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
    public partial class ProjectControl : UserControl
    {
        public ProjectControl()
        {
            InitializeComponent();
        }

        public ProjectControl(Project.Project project)
        {
            InitializeComponent();

            lbTitle.Text = project.Name;
            lbDescription.Text = project.Description;
            pbIcon.Image = Image.FromFile(Path.Combine(project.StoragePath, "icon.png"));

            foreach (int tag in project.TagIDs)
            {
                flTagList.Controls.Add(new Label
                {
                    Text = Tagging.Tag.TagList[tag].Text,
                    BackColor = Tagging.Tag.TagList[tag].BackgroundColour,
                    Size = new Size(96, 20),
                    TextAlign = ContentAlignment.MiddleCenter
                }); ;
            }
        }
    }
}
