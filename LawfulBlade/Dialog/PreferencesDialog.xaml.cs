using System.Windows;

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
            hideOpenInstance.IsChecked = App.Preferences.HideLawfulBladeOnOpenInstance;
            localEmuPath.Text = App.Preferences.LocaleEmulatorPath;
        }

        void OnClickConfirm(object sender, RoutedEventArgs e)
        {
            App.Preferences.AutoCheckForUpdates = checkForUpdates.IsChecked ?? false;
            App.Preferences.ShowDebugConsole = showDebugConsole.IsChecked ?? false;
            App.Preferences.UseLocaleEmulator = useLocaleEmulator.IsChecked ?? false;
            App.Preferences.LocaleEmulatorPath = localEmuPath.Text ?? string.Empty;
            App.Preferences.HideLawfulBladeOnOpenInstance = hideOpenInstance.IsChecked ?? true;
            App.Preferences.Save();

            Close();
        }

        void OnClickCancel(object sender, RoutedEventArgs e) =>
            Close();
    }
}
