using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LawfulBladeManager.Dialog
{
    public partial class ProjectCreateDialog : Form
    {
        public string ProjectName => tbProjectName.Text;
        public string ProjectDescription => tbDescription.Text;
        public string TargetInstance => cbTargetInstance.Text;
        public string TargetPath => tbTargetPath.Text;

        public ProjectCreateDialog()
        {
            InitializeComponent();
        }

        private void btCreate_Click(object sender, EventArgs e)
        {
            // Validate Input Date
            try
            {
                if (tbProjectName.Text == string.Empty)
                    throw new Exception("You must enter a project name!");
                if (tbTargetPath.Text == string.Empty || !Directory.Exists(tbTargetPath.Text))
                    throw new Exception("The target path is either invalid or doesn't exist!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Lawful Blade", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btTargetSelect_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                // Configure Dialog

                // Do Dialog
                switch (fbd.ShowDialog())
                {
                    case DialogResult.OK:
                        tbTargetPath.Text = fbd.SelectedPath;
                        break;

                    default:
                        break;
                }
            }
        }

    }
}
