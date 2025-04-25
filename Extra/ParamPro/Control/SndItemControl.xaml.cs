using ParamPro.Format;
using System.Windows.Controls;


namespace ParamPro.Control
{
    /// <summary>
    /// Interaction logic for SndItemControl.xaml
    /// </summary>
    public partial class SndItemControl : UserControl
    {
        public short SoundID => short.Parse(soundField.Text);
        public byte Delay    => byte.Parse(delayField.Text);
        public sbyte Pitch   => sbyte.Parse(pitchField.Text);

        PrfSndItem Item;

        public SndItemControl(PrfSndItem sndItem)
        {
            InitializeComponent();

            // Load values into boxes
            soundField.Text = $"{sndItem.id}";
            delayField.Text = $"{sndItem.delay}";
            pitchField.Text = $"{sndItem.pitch}";

            Item = sndItem;
        }

        void OnEditEndSoundID(object sender, System.Windows.RoutedEventArgs e) =>
            Item.id = SoundID;

        void OnEditEndDelay(object sender, System.Windows.RoutedEventArgs e) =>
            Item.delay = Delay;

        void OnEditEndPitch(object sender, System.Windows.RoutedEventArgs e) =>
            Item.pitch = Pitch;
    }
}
