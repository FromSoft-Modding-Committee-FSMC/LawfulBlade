using Sealed_Sword_Stone.Core;
using Sealed_Sword_Stone.Windows;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sealed_Sword_Stone.Controls
{
    /// <summary>
    /// Interaction logic for ControllerBinding.xaml
    /// </summary>
    public partial class ControllerBinding : UserControl
    {
        ControlBinding binding;

        public ControllerBinding(ControlBinding binding, string name)
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

        void LoadTextFromBinding()
        {
            // Main Key Text
            switch (binding.GamePadBindingType)
            {
                // (0 = None, 1 = Button, 2 = Axis, 3 = Hat)
                case 0x01:
                    mainCtrlTextField.Text = $"Button #{binding.GamePadInputID}";
                    mainCtrlTextField.Foreground = Brushes.White;
                    break;

                case 0x02:
                    mainCtrlTextField.Text = $"Axis #{binding.GamePadInputID}";
                    mainCtrlTextField.Foreground = Brushes.White;
                    break;

                case 0x03:
                    mainCtrlTextField.Text = $"Hat Switch #{binding.GamePadInputID}";
                    mainCtrlTextField.Foreground = Brushes.White;
                    break;

                default:
                    mainCtrlTextField.Text = "Unbound";
                    mainCtrlTextField.Foreground = Brushes.DarkGray;
                    break;
            }
        }
    }
}
