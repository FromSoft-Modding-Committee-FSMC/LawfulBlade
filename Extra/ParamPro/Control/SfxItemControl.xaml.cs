using ParamPro.Format;
using System.Windows.Controls;

namespace ParamPro.Control
{
    /// <summary>
    /// Interaction logic for SfxItemControl.xaml
    /// </summary>
    public partial class SfxItemControl : UserControl
    {
        public short EffectID => short.Parse(effectField.Text);
        public byte Delay => byte.Parse(delayField.Text);
        public byte CP => byte.Parse(cpField.Text);

        PrfSfxItem Item;

        public SfxItemControl(PrfSfxItem sndItem)
        {
            InitializeComponent();

            // Load values into boxes
            effectField.Text = $"{sndItem.id}";
            delayField.Text = $"{sndItem.delay}";
            cpField.Text = $"{sndItem.controlPoint}";

            Item = sndItem;
        }

        /// <summary>
        /// Event Callback.<br/>
        /// </summary>
        void OnEditEndEffectID(object sender, System.Windows.RoutedEventArgs e) =>
            Item.id = EffectID;

        /// <summary>
        /// Event Callback.<br/>
        /// </summary>
        void OnEditEndDelay(object sender, System.Windows.RoutedEventArgs e) =>
            Item.delay = Delay;

        /// <summary>
        /// Event Callback.<br/>
        /// </summary>
        void OnEditEndCP(object sender, System.Windows.RoutedEventArgs e) =>
            Item.controlPoint = CP;
    }
}
