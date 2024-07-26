using LawfulBladeManager.Core;
using System.Security.Permissions;

namespace LawfulBladeManager.Dialog
{
    public partial class PackageDependentsWarning : Form
    {
        public string[] Dependents { get; set; } = Array.Empty<string>();
        public string Brief { get; set; } = string.Empty;
        public string Warning { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public bool AllowContinue { get; set; } = false;

        /// <summary>
        /// Default Constructor.<br/> WinForms fuckery.
        /// </summary>
        public PackageDependentsWarning() =>
            InitializeComponent();

        /// <summary>
        /// Called when the continue button is pressed in the dialog
        /// </summary>
        void OnDialogContinue(object sender, EventArgs e)
        {
            tmContinueEnable.Stop();

            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// Called when the cancel button is pressed in the dialog
        /// </summary>
        void OnDialogCancel(object sender, EventArgs e)
        {
            tmContinueEnable.Stop();

            DialogResult = DialogResult.Cancel;
            Close();
        }

        void OnDialogShown(object sender, EventArgs e)
        {
            // Load the dependents list
            lbDependents.Items.Clear();

            foreach (string dependent in Dependents)
                lbDependents.Items.Add(dependent);

            // Set the title
            Text = Title;
            lbWarningBrief.Text = Brief;
            lbWarningDescription.Text = Warning;

            // Hide the continue button sometimes...
            if (!AllowContinue)
            {
                // Move the cancel button to the continue button...
                btCancel.Location = btConfirm.Location;
                btConfirm.Location = new Point(-8192, -8192);
            }
            else
            {
                // Start the timer...
                tmContinueEnable.Tick += (object? sender, EventArgs e) =>
                {
                    Invoke(() =>
                    {
                        btConfirm.Enabled = true;

                        // Stop the timer again...
                        tmContinueEnable.Stop();
                    });
                };
                tmContinueEnable.Start();
            }
        }
    }
}
