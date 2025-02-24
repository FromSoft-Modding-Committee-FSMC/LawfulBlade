using System.Reflection;
using System.Windows;
using System.ComponentModel;

namespace LawfulBlade.Core
{
    internal class Winblows
    {
        // These fixes are curtosy of the following fine gents:
        // https://www.red-gate.com/simple-talk/blogs/wpf-menu-displays-to-the-left-of-the-window/
        //      ^ Thanks Damon

        readonly static FieldInfo menuDropAlignment = typeof(SystemParameters).GetField("_menuDropAlignment", BindingFlags.NonPublic | BindingFlags.Static)!;

        public static void ApplyFixes()
        {
            SystemParameters.StaticPropertyChanged += OnSystemStaticPropertyChanged;
            OnSystemStaticPropertyChanged(null, null!);
        }

        static void OnSystemStaticPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (SystemParameters.MenuDropAlignment)
                menuDropAlignment.SetValue(null, false); 
        }
    }
}
