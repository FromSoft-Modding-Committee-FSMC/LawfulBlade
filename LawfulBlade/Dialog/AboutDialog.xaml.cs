using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Navigation;
using System.Diagnostics;
using System.Net.NetworkInformation;

namespace LawfulBlade.Dialog
{
    /// <summary>
    /// Interaction logic for AboutDialog.xaml
    /// </summary>
    public partial class AboutDialog : Window
    {
        public AboutDialog()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// Event Handler<br/>
        /// Deals with links being clicked
        /// </summary>
        void OnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            try
            {
                if (!NetworkInterface.GetIsNetworkAvailable())
                    throw new Exception("No network connection!");

                if (Process.Start(new ProcessStartInfo() { FileName = "explorer.exe", Arguments = e.Uri.ToString() }) == null)
                    throw new Exception($"Couldn't start browser process with URL: '{e.Uri.ToString()}'");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lawful Blade", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            // Try to open the hyperlink in the users default browser
            e.Handled = true;
        }

        /// <summary>
        /// Event Handler<br/>
        /// Used by the 'Okidoki' button to close the dialog
        /// </summary>
        void OnConfirmDialog(object sender, RoutedEventArgs e) =>
            Close();
    }
}
