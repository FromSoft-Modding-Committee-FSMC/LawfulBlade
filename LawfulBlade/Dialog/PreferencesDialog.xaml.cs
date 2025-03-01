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

namespace LawfulBlade.Dialog
{
    /// <summary>
    /// Interaction logic for PreferencesDialog.xaml
    /// </summary>
    public partial class PreferencesDialog : Window
    {
        public PreferencesDialog()
        {
            InitializeComponent();

            // Load current preferences into UI....
            checkForUpdates.IsChecked = App.Preferences.AutoCheckForUpdates;
            showDebugConsole.IsChecked = App.Preferences.ShowDebugConsole;
            useLocaleEmulator.IsChecked = App.Preferences.UseLocaleEmulator;
            localEmuPath.Text = App.Preferences.LocaleEmulatorPath;
        }

        void OnClickConfirm(object sender, RoutedEventArgs e)
        {
            App.Preferences.AutoCheckForUpdates = checkForUpdates.IsChecked ?? false;
            App.Preferences.ShowDebugConsole = showDebugConsole.IsChecked ?? false;
            App.Preferences.UseLocaleEmulator = useLocaleEmulator.IsChecked ?? false;
            App.Preferences.LocaleEmulatorPath = localEmuPath.Text ?? string.Empty;
            App.Preferences.Save();

            Close();
        }

        void OnClickCancel(object sender, RoutedEventArgs e) =>
            Close();
    }
}
