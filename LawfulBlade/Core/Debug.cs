using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace LawfulBlade.Core
{
    // Thanks, Pavlo K! (https://stackoverflow.com/questions/15604014/no-console-output-when-using-allocconsole-and-target-architecture-x86)
    public static class Debug
    {
        [DllImport("kernel32.dll", EntryPoint = "AllocConsole", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        static extern int AllocConsole();

        [DllImport("kernel32.dll", EntryPoint = "AttachConsole", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        static extern uint AttachConsole(uint dwProcessId);

        [DllImport("kernel32.dll", EntryPoint = "CreateFileW", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        static extern IntPtr CreateFileW(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

        [DllImport("kernel32.dll", EntryPoint = "SetConsoleMode", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

        [DllImport("kernel32.dll", EntryPoint = "GetConsoleMode", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [DllImport("kernel32.dll", EntryPoint = "SetConsoleIcon", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        static extern bool SetConsoleIcon(IntPtr hIcon);

        [DllImport("kernel32.dll", EntryPoint = "SetConsoleTitle", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        static extern bool SetConsoleTitle(string lpConsoleTitle);

        [DllImport("kernel32.dll", EntryPoint = "GetStdHandle", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        static extern IntPtr GetStdHandle(int nStdHandle);

        const int  STD_OUTPUT_HANDLE = -11;
        const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 4;
        const uint GENERIC_WRITE         = 0x40000000;
        const uint FILE_SHARE_WRITE      = 0x00000002;
        const uint OPEN_EXISTING         = 0x00000003;
        const uint FILE_ATTRIBUTE_NORMAL = 0x80;
        const uint ERROR_ACCESS_DENIED   = 5;

        public static void InitConsole()
        {
            bool consoleAttached = true;
            if (AttachConsole((uint)Environment.ProcessId) == 0 && Marshal.GetLastWin32Error() != ERROR_ACCESS_DENIED)
                consoleAttached = AllocConsole() != 0;

            if (consoleAttached)
            {
                // Initialize COUT
                SafeFileHandle file = new(CreateFileW("CONOUT$", GENERIC_WRITE, FILE_SHARE_WRITE, IntPtr.Zero, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, IntPtr.Zero), true);

                if (!file.IsInvalid)
                {
                    StreamWriter sw = new(new FileStream(file, FileAccess.Write)) { AutoFlush = true };
                    Console.SetError(sw);
                    Console.SetOut(sw);
                }

                // Enable terminal mode to get sexy ansi goodness
                nint outHandle = GetStdHandle(STD_OUTPUT_HANDLE);
                GetConsoleMode(outHandle, out uint mode);
                SetConsoleMode(outHandle, mode | ENABLE_VIRTUAL_TERMINAL_PROCESSING);

                // Set the console title to something more befitting of this sexy project
                SetConsoleTitle("Lawful Blade - Console");

                // Get and set the console icon - Can we do this without System.Drawing.* ?
                Icon icon = Icon.ExtractAssociatedIcon(Path.Combine(App.ProgramPath, "LawfulBlade.exe"));
                if (icon != null)
                    SetConsoleIcon(icon.Handle);
            }
        }

        /// <summary>
        /// Prefixes message with 'INFO'
        /// </summary>
        public static void Info(string message) =>
            Write("INFO", 0x80c0f0, message, 0xFFFFFF);

        /// <summary>
        /// Prefixes message with 'WARN'
        /// </summary>
        public static void Warn(string message) =>
            Write("WARN", 0xf0f080, message, 0xFFFFFF);

        /// <summary>
        /// Prefixes message with 'SHIT'
        /// </summary>
        public static void Error(string message) =>
            Write("SHIT", 0xF08080, message, 0xFFFFFF);

        /// <summary>
        /// Prefixes message with 'OUCH', and messages are bright yellow
        /// </summary>
        public static void Critical(string message) =>
            Write("OUCH", 0xFF2040, message, 0xF0F000);

        /// <summary>
        /// Writes a long message to the console
        /// </summary>
        static void Write(string heading, uint headingColour, string message, uint messageColour) =>
            Console.WriteLine(string.Join("",
                "\u001b[38;2;68;68;68m<\u001b[0m\u001b[38;2;170;170;170m[\u001b[0m",    // We pre build these strings to reduce a little processing time.
                heading.ANSIC(headingColour),
                "\u001b[38;2;170;170;170m]\u001b[0m\u001b[38;2;68;68;68m>:\u001b[0m ",
                message.ANSIC(messageColour)
                ));
        
        /// <summary>
        /// Wraps text in ANSI codes which add colour
        /// </summary>
        static string ANSIC(this string input, uint colour) =>
            $"\u001b[38;2;{(colour >> 16) & 0xFF};{(colour >> 8) & 0xFF};{(colour >> 0) & 0xFF}m{input}\u001b[0m";
    }
}
