using System;
using System.Collections.Generic;
using System.Formats.Nrbf;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Linearstar.Windows.RawInput;
using Linearstar.Windows.RawInput.Native;
using Sealed_Sword_Stone.Core;
using Sealed_Sword_Stone.Windows;

namespace Sealed_Sword_Stone.Controls
{
    /// <summary>
    /// Interaction logic for ControllerTab.xaml
    /// </summary>
    public partial class ControllerTab : UserControl
    {
        ConfigurationDialog tab;

        WindowInteropHelper windowInteropHelper;
        HwndSource windowSource;
        nint windowHwnd;

        public ControllerTab(ConfigurationDialog confDialog)
        {
            InitializeComponent();

            tab = confDialog;

            mappingBox.Children.Add(new ControllerBinding(tab.PlayerMoveForward, "Move Fowards"));
            mappingBox.Children.Add(new ControllerBinding(tab.PlayerMoveBackward, "Move Backwards"));
            mappingBox.Children.Add(new ControllerBinding(tab.PlayerStrafeLeft, "Strafe Left"));
            mappingBox.Children.Add(new ControllerBinding(tab.PlayerStrafeRight, "Strafe Right"));
            mappingBox.Children.Add(new ControllerBinding(tab.PlayerTurnLeft, "Turn Left"));
            mappingBox.Children.Add(new ControllerBinding(tab.PlayerTurnRight, "Turn Right"));
            mappingBox.Children.Add(new ControllerBinding(tab.PlayerLookUp, "Look Up"));
            mappingBox.Children.Add(new ControllerBinding(tab.PlayerLookDown, "Look Down"));
            mappingBox.Children.Add(new ControllerBinding(tab.PlayerAttack, "Attack"));
            mappingBox.Children.Add(new ControllerBinding(tab.PlayerCast, "Cast Magic"));
            mappingBox.Children.Add(new ControllerBinding(tab.PlayerInspect, "Inspect"));
            mappingBox.Children.Add(new ControllerBinding(tab.PlayerSprint, "Sprint"));
            mappingBox.Children.Add(new ControllerBinding(tab.MenuOpen, "Open Menu"));
            mappingBox.Children.Add(new ControllerBinding(tab.MenuConfirm, "Confirm Menu"));
            mappingBox.Children.Add(new ControllerBinding(tab.MenuCancel, "Cancel Menu"));
            mappingBox.Children.Add(new ControllerBinding(tab.MenuUp, "Cursor Up"));
            mappingBox.Children.Add(new ControllerBinding(tab.MenuDown, "Cursor Down"));
            mappingBox.Children.Add(new ControllerBinding(tab.MenuLeft, "Cursor Left"));
            mappingBox.Children.Add(new ControllerBinding(tab.MenuRight, "Cursor Right"));

            Loaded += OnLoaded;
        }

        /// <summary>
        /// Event Callback.<br/>
        /// Registers Raw Input devices
        /// </summary>
        void OnLoaded(object sender, RoutedEventArgs e)
        {
            // Get the window handle
            windowInteropHelper = new WindowInteropHelper(Window.GetWindow(this));
            windowHwnd = windowInteropHelper.Handle;

            // Get a list of Raw Input Devices
            RawInputDevice[] devices = RawInputDevice.GetDevices();


            // Listen for DINPUT type controllers...
            RawInputDevice.RegisterDevice(HidUsageAndPage.Joystick, RawInputDeviceFlags.ExInputSink, windowHwnd);

            // Listen for XINPUT type controllers...
            RawInputDevice.RegisterDevice(HidUsageAndPage.GamePad, RawInputDeviceFlags.ExInputSink, windowHwnd);

            windowSource = HwndSource.FromHwnd(windowHwnd);
            windowSource.AddHook(OnWindowMessage);
        }

        IntPtr OnWindowMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam, ref bool handled)
        {
            // The WM_INPUT event is pushed when input is avaliable to be processed
            const int WM_INPUT = 0x00FF;

            switch (msg)
            {
                // We must parse our raw input messages
                case WM_INPUT:
                    // Grab the raw input data...
                    RawInputData riData = RawInputData.FromHandle(lparam);

                    // Grab the device handle and device data
                    RawInputDeviceHandle riDeviceHandle = riData.Header.DeviceHandle;
                    RawInputDevice riDevice = riData.Device;

                    // Return early when the device is null
                    if (riDevice == null)
                        return IntPtr.Zero;

                    switch (riData)
                    {
                        case RawInputHidData hidData:
                            ParseRawInput(riDevice, riDeviceHandle, hidData);
                            break;
                    }

                    break;
            }

            return IntPtr.Zero;
        }

        void ParseRawInput(RawInputDevice device, RawInputDeviceHandle deviceHandle, RawInputHidData deviceData)
        {
            // Clear the Controller State
            for (int i = 0; i < App.ControllerState.ButtonStates.Length; ++i)
                App.ControllerState.ButtonStates[i] = false;

            for (int i = 0; i < App.ControllerState.HatValues.Length; ++i)
                App.ControllerState.HatValues[i] = false;

            // Get Buttons
            foreach (HidButtonSetState buttonStates in deviceData.ButtonSetStates)
            {
                foreach (ushort pressedButton in buttonStates.ActiveUsages)
                    App.ControllerState.ButtonStates[pressedButton - buttonStates.ButtonSet.UsageMin] = true;
            }

            foreach (HidValueSetState valueStates in deviceData.ValueSetStates)
            {
                foreach (int currentValue in valueStates.CurrentValues)
                {
                    switch (valueStates.ValueSet.UsageMin)
                    {
                        // HAT SWITCH TYPE (it's a fuckin dpad eh?)
                        case 0x39:
                            if (currentValue == 0xF)
                                break;
                            App.ControllerState.HatValues[currentValue] = true;
                        break;
                    }
                }
            }
        }
    }
}
