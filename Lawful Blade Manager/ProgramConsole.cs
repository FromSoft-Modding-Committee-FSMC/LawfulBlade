using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Win32.SafeHandles;

namespace LawfulBladeManager
{
    // Thanks, Pavlo K! (https://stackoverflow.com/questions/15604014/no-console-output-when-using-allocconsole-and-target-architecture-x86)

    public static class ProgramConsole
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

        [DllImport("kernel32.dll", EntryPoint = "GetStdHandle", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        static extern IntPtr GetStdHandle(int nStdHandle);


        const int  STD_OUTPUT_HANDLE                  = -11;
        const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 4;
        const uint GENERIC_WRITE                      = 0x40000000;
        const uint FILE_SHARE_WRITE                   = 0x00000002;
        const uint OPEN_EXISTING                      = 0x00000003;
        const uint FILE_ATTRIBUTE_NORMAL              = 0x80;
        const uint ERROR_ACCESS_DENIED                = 5;


        public static void InitConsole()
        {
            bool consoleAttached = true;
            if (AttachConsole((uint)Environment.ProcessId) == 0 && Marshal.GetLastWin32Error() != ERROR_ACCESS_DENIED)
                consoleAttached = AllocConsole() != 0;

            if (consoleAttached)
            {
                // Initialize COUT
                InitializeOutStream();

                // Enable terminal mode to get sexy ansi goodness
                nint outHandle = GetStdHandle(STD_OUTPUT_HANDLE);
                GetConsoleMode(outHandle, out uint mode);
                SetConsoleMode(outHandle, mode | ENABLE_VIRTUAL_TERMINAL_PROCESSING);
            }
        }

        static void InitializeOutStream()
        {
            SafeFileHandle file = new(CreateFileW("CONOUT$", GENERIC_WRITE, FILE_SHARE_WRITE, IntPtr.Zero, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, IntPtr.Zero), true);

            if (!file.IsInvalid)
            {
                StreamWriter sw = new (new FileStream(file, FileAccess.Write)) { AutoFlush = true };
                Console.SetError(sw);
                Console.SetOut(sw);
            }
        }
    }
}
