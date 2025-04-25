using Sealed_Sword_Stone.Core;
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

namespace Sealed_Sword_Stone.Windows
{
    [Flags]
    public enum ControlBindingDeviceType
    {
        GamePad  = (1 << 0),
        Keyboard = (1 << 1),
        Mouse    = (1 << 2)
    }

    public partial class ControlBindingDialog : Window
    {
        public ControlBindingDeviceType DeviceType { get; private set; }
        public bool SetAlternativeBinding { get; private set; }
        public ControlBinding Binding { get; private set; }

        public ControlBindingDialog(ControlBinding bindingRef, ControlBindingDeviceType deviceType, bool alternate = false)
        {
            InitializeComponent();

            // Register to events
            PreviewKeyDown   += OnPreviewKeyDown;
            PreviewMouseDown += OnPreviewMouseDown;

            DeviceType = deviceType;
            SetAlternativeBinding = alternate;
            Binding = bindingRef;

            // Set the hint text based on the types provided
            if ((DeviceType & ControlBindingDeviceType.Keyboard) != 0 || (DeviceType & ControlBindingDeviceType.Mouse) != 0)
                hintText.Text = "Press any Key or Mouse button.";
            else
                hintText.Text = "Press any Controller input.";
        }

        void OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if ((DeviceType & ControlBindingDeviceType.Mouse) == 0)
                return;

            // Get the virual key ID from whatever this one is...
            int virtualKeyID = (int)e.ChangedButton & 0xFF;

            if (!SetAlternativeBinding)
            {
                Binding.DevicesInputID     = (uint)virtualKeyID;
                Binding.DevicesBindingType = 0x2;   // Mouse Type
            }           
            else
            {
                Binding.DevicesInputAltID     = (uint)virtualKeyID;
                Binding.DevicesBindingAltType = 0x2;   // Mouse Type
            }
                
            e.Handled = true;

            Close();
        }

        void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((DeviceType & ControlBindingDeviceType.Keyboard) == 0)
                return;

            // Get the virual key ID from whatever this one is...
            int virtualKeyID = KeyInterop.VirtualKeyFromKey(e.Key) & 0xFF;

            if (!SetAlternativeBinding)
            {
                Binding.DevicesInputID     = (uint)virtualKeyID;
                Binding.DevicesBindingType = 0x1;   // Key Type
            }           
            else
            {
                Binding.DevicesInputAltID     = (uint)virtualKeyID;
                Binding.DevicesBindingAltType = 0x1;   // Key Type
            }

            e.Handled = true;

            Close();
        }
    }
}
