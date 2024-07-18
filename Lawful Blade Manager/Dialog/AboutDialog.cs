using System.Diagnostics;
using System.Net.NetworkInformation;

namespace LawfulBladeManager.Dialog
{
    public partial class AboutDialog : Form
    {
        /// <summary>
        /// Default Constructor.<br/> WinForms fuckery.
        /// </summary>
        public AboutDialog() => InitializeComponent();


        /// <summary>
        /// Called when the confirm button is pressed in the dialog
        /// </summary>
        void OnDialogConfirm(object sender, EventArgs e) =>
            Close();

        void OnDialogLoad(object sender, EventArgs e)
        {
            rtAbout.SelectAll();
            rtAbout.SelectedRtf = Properties.Resources.about;
        }

        void OnLinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                if (!NetworkInterface.GetIsNetworkAvailable())
                    throw new Exception("No network connection!");

                if (Process.Start(new ProcessStartInfo() { FileName = e.LinkText, UseShellExecute = true }) == null)
                    throw new Exception($"Couldn't start browser process with URL:\n'{e.LinkText}'");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lawful Blade", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
