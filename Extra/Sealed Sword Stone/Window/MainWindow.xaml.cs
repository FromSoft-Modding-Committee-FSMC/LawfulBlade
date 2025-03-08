using System.Diagnostics;
using System.IO;
using System.Media;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace Sealed_Sword_Stone;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    SoundPlayer clickSound;
    SoundPlayer moveSound;

    public MainWindow()
    {
        InitializeComponent();

        btnPlay.MouseEnter += OnMouseEnterButton;
        btnPlay.MouseLeave += OnMouseLeaveButton;
        btnPlay.MouseDown  += OnClickPlay;
        btnConf.MouseEnter += OnMouseEnterButton;
        btnConf.MouseLeave += OnMouseLeaveButton;
        btnConf.MouseDown  += OnClickConf;
        btnExit.MouseEnter += OnMouseEnterButton;
        btnExit.MouseLeave += OnMouseLeaveButton;
        btnExit.MouseDown  += OnClickExit;

        // Initialize Sound FX
        clickSound = new SoundPlayer(Path.Combine(App.ProgramPath, "LAUNCHER", "click.wav"));
        moveSound  = new SoundPlayer(Path.Combine(App.ProgramPath, "LAUNCHER", "hover.wav"));

        // Load and play launcher music
        if (App.LauncherConfig.LauncherMusic != string.Empty)
        {
            MediaPlayer mp = new MediaPlayer();
            mp.Open(new Uri(Path.Combine(App.ProgramPath, "LAUNCHER", App.LauncherConfig.LauncherMusic)));
            mp.Volume = 0.25f;
            mp.Play();
        }

        // Set Window Title
        Title = $"Sealed Sword Stone - {App.LauncherConfig.GameName}";
        Icon = new BitmapImage(new Uri(Path.Combine(App.ProgramPath, "LAUNCHER", "icon.png")));
    }

    /// <summary>
    /// Used to set the opacity of buttons to full when hovered
    /// </summary>
    void OnMouseEnterButton(object sender, MouseEventArgs e)
    {
        Image senderAsImage = (sender as Image);

        if (senderAsImage == null)
            return;

        // Play the move sound...
        moveSound.Play();

        senderAsImage.Opacity = 1.0;
    }

    /// <summary>
    /// Used to set the opacity of buttons to half when unhovered
    /// </summary>
    void OnMouseLeaveButton(object sender, MouseEventArgs e)
    {
        Image senderAsImage = (sender as Image);

        if (senderAsImage == null)
            return;

        senderAsImage.Opacity = 0.5;
    }

    void OnClickPlay(object sender, MouseButtonEventArgs e)
    {
        clickSound.Play();

        App.StartGame();
    }

    void OnClickConf(object sender, MouseButtonEventArgs e)
    {
        clickSound.Play();
    }

    /// <summary>
    /// Exits the application when pressed
    /// </summary>
    void OnClickExit(object sender, MouseButtonEventArgs e)
    {
        clickSound.Play();

        App.Current.Shutdown(0);
    }

    void OnClickSocialDiscord(object sender, MouseButtonEventArgs e) =>
        Process.Start(new ProcessStartInfo { FileName = new Uri("https://discord.gg/66CQdB738d").AbsoluteUri, UseShellExecute = true });

    void OnClickSocialReddit(object sender, MouseButtonEventArgs e) =>
        Process.Start(new ProcessStartInfo { FileName = new Uri("https://www.reddit.com/r/SwordOfMoonlight").AbsoluteUri, UseShellExecute = true });

    void OnClickSocialYoutube(object sender, MouseButtonEventArgs e) =>
        Process.Start(new ProcessStartInfo { FileName = new Uri("https://www.youtube.com/@SwordofMoonlightCommunity").AbsoluteUri, UseShellExecute = true });
}
