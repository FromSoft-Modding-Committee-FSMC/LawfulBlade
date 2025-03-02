using System.Reflection;
using System.Windows;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace LawfulBlade.Core
{
    internal class Winblows
    {
        // Most of these fixes are curtosy of the following fine gents:
        // https://www.red-gate.com/simple-talk/blogs/wpf-menu-displays-to-the-left-of-the-window/
        //      ^ Thanks Damon
        // https://stackoverflow.com/a/958980
        //      ^ Thanks Joe White

        const int GWL_STYLE  = -16;

        [DllImport("user32.dll", EntryPoint = "GetWindowLong", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        static extern uint GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        static extern uint SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

        /// <summary>
        /// Stores the menu drop alignment field
        /// </summary>
        readonly static FieldInfo menuDropAlignment = typeof(SystemParameters).GetField("_menuDropAlignment", BindingFlags.NonPublic | BindingFlags.Static)!;

        public static void WindowDisableSysMenu(Window window)
        {
            // Get the native window handle
            nint hwnd = new WindowInteropHelper(window).Handle;

            // Get the current Window Style
            SetWindowLong(hwnd, GWL_STYLE, (GetWindowLong(hwnd, GWL_STYLE) & 0xFFF7FFFF));
        }

        /// <summary>
        /// Applies global application fixes
        /// </summary>
        public static void ApplyFixes()
        {
            // Fucking idiotic
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            SystemParameters.StaticPropertyChanged += OnSystemStaticPropertyChanged;
            OnSystemStaticPropertyChanged(null, null!);
        }

        /// <summary>
        /// Listens for a static property being changed
        /// </summary>
        static void OnSystemStaticPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (SystemParameters.MenuDropAlignment)
                menuDropAlignment.SetValue(null, false); 
        }
    }
}
