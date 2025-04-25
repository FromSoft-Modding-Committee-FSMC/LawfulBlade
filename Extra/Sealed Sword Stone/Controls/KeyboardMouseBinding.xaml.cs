using Sealed_Sword_Stone.Core;
using Sealed_Sword_Stone.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media;


namespace Sealed_Sword_Stone.Controls
{
    /// <summary>
    /// Interaction logic for KeyboardMouseBinding.xaml
    /// </summary>
    public partial class KeyboardMouseBinding : UserControl
    {
        ControlBinding binding;

        public KeyboardMouseBinding(ControlBinding binding, string name)
        {
            InitializeComponent();

            this.binding = binding;

            nameField.Text = name;

            LoadTextFromBinding();
        }

        /// <summary>
        /// Event Callback.<br/>
        /// </summary>
        void OnMainCtrlClick(object sender, RoutedEventArgs e)
        {
            ControlBindingDialog bindingDialog = new ControlBindingDialog(binding, ControlBindingDeviceType.Keyboard | ControlBindingDeviceType.Mouse, false);
            bindingDialog.Owner = Window.GetWindow(this);
            bindingDialog.ShowDialog();

            LoadTextFromBinding();
        }
        
        /// <summary>
        /// Event Callback.<br/>
        /// </summary>
        void OnAltCtrlClick(object sender, RoutedEventArgs e)
        {
            ControlBindingDialog bindingDialog = new ControlBindingDialog(binding, ControlBindingDeviceType.Keyboard | ControlBindingDeviceType.Mouse, true);
            bindingDialog.Owner = Window.GetWindow(this);
            bindingDialog.ShowDialog();

            LoadTextFromBinding();
        }

        void LoadTextFromBinding()
        {
            // Main Key Text
            switch (binding.DevicesBindingType)
            {
                case 0x01:
                    // Keyboard Type...
                    mainCtrlTextField.Text       = $"{KeyInterop.KeyFromVirtualKey((int)binding.DevicesInputID)}";
                    mainCtrlTextField.Foreground = Brushes.White;
                    break;

                case 0x02:
                    mainCtrlTextField.Text = $"Mouse {binding.DevicesInputID}";
                    mainCtrlTextField.Foreground = Brushes.White;
                    break;

                default:
                    mainCtrlTextField.Text = "Unbound";
                    mainCtrlTextField.Foreground = Brushes.DarkGray;
                    break;
            }

            // Alt Key Text
            switch (binding.DevicesBindingAltType)
            {
                case 0x01:
                    // Keyboard Type...
                    altCtrlTextField.Text = $"{KeyInterop.KeyFromVirtualKey((int)binding.DevicesInputAltID)}";
                    altCtrlTextField.Foreground = Brushes.White;
                    break;

                case 0x02:
                    altCtrlTextField.Text = $"Mouse {binding.DevicesInputAltID}";
                    altCtrlTextField.Foreground = Brushes.White;
                    break;

                default:
                    altCtrlTextField.Text = "Unbound";
                    altCtrlTextField.Foreground = Brushes.DarkGray;
                    break;
            }
        }
    }
}
