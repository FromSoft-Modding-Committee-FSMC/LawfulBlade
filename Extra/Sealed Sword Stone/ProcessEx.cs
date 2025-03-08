using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Sealed_Sword_Stone
{
    public class ProcessEx
    {
        // Constants
        const uint CREATE_SUSPENDED = 0x00000004;
        const uint MEM_RESERVE = 0x00002000;
        const uint MEM_COMMIT  = 0x00001000;
        const uint PAGE_EXECUTE_READWRITE = 0x00000040;
        const uint PAGE_READWRITE = 0x04;


        // Native Imports
        [DllImport("kernel32.dll", EntryPoint = "CreateProcess", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        static extern bool CreateProcess(string lpApplicationName, string lpCommandLine, IntPtr lpProcessAttributes, IntPtr lpThreadAttributes, bool bInheritHandles, uint dwCreationFlags, IntPtr lpEnvironment, string lpCurrentDirectory, ref STARTUPINFO lpStartupInfo, out PROCESS_INFORMATION lpProcessInformation);

        [DllImport("kernel32.dll", EntryPoint = "GetProcAddress", CharSet = CharSet.Auto, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        [DllImport("kernel32.dll", EntryPoint = "GetModuleHandle", CharSet = CharSet.Auto, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32.dll", EntryPoint = "VirtualAllocEx", CharSet = CharSet.Auto, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32.dll", EntryPoint = "WriteProcessMemory", CharSet = CharSet.Auto, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out uint lpNumberOfBytesWritten);

        [DllImport("kernel32.dll", EntryPoint = "CreateRemoteThread", CharSet = CharSet.Auto, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        static extern bool CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

        [DllImport("kernel32.dll", EntryPoint = "ResumeThread", CharSet = CharSet.Auto, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        static extern uint ResumeThread(IntPtr hThread);

        // My Shit
        PROCESS_INFORMATION processInfo;
        STARTUPINFO startupInfo;

        /// <summary>
        /// Starts a suspended process
        /// </summary>
        public static ProcessEx StartSuspendedProcess(string filepath, string arguments = null)
        {
            ProcessEx newProcess = new();

            newProcess.startupInfo = new();
            newProcess.startupInfo.cb = (uint)Marshal.SizeOf(newProcess.startupInfo);

            newProcess.processInfo = new();
            bool success = CreateProcess(
                filepath,
                arguments,
                IntPtr.Zero,
                IntPtr.Zero,
                false,
                CREATE_SUSPENDED,
                IntPtr.Zero,
                Directory.GetCurrentDirectory(),
                ref newProcess.startupInfo,
                out newProcess.processInfo
            );

            if (success)
                return newProcess;

            throw new InvalidOperationException("Failed to create suspended process.");
        }

        public void InjectPayload(string filepath)
        {
            // Allocate memory for the DLL path in the target process
            IntPtr allocMemAddress = VirtualAllocEx(processInfo.hProcess, IntPtr.Zero, (uint)(filepath.Length + 1), MEM_RESERVE | MEM_COMMIT, PAGE_READWRITE);

            // Write the DLL path into the allocated memory
            byte[] dllBytes = Encoding.ASCII.GetBytes(filepath);
            WriteProcessMemory(processInfo.hProcess, allocMemAddress, dllBytes, (uint)dllBytes.Length, out uint bytesWritten);

            // Get address of LoadLibraryA in kernel32.dll
            IntPtr loadLibraryAddr = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");

            // Create a remote thread that calls LoadLibraryA with the DLL path as argument
            CreateRemoteThread(processInfo.hProcess, IntPtr.Zero, 0, loadLibraryAddr, allocMemAddress, 0, IntPtr.Zero);
        }

        public void ResumeProcess() =>
            ResumeThread(processInfo.hThread);
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct STARTUPINFO
    {
        public uint cb;
        public string lpReserved;
        public string lpDesktop;
        public string lpTitle;
        public uint dwX;
        public uint dwY;
        public uint dwXSize;
        public uint dwYSize;
        public uint dwXCountChars;
        public uint dwYCountChars;
        public uint dwFillAttribute;
        public uint dwFlags;
        public ushort wShowWindow;
        public ushort cbReserved2;
        public IntPtr lpReserved2;
        public IntPtr hStdInput;
        public IntPtr hStdOutput;
        public IntPtr hStdError;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PROCESS_INFORMATION
    {
        public IntPtr hProcess;
        public IntPtr hThread;
        public uint dwProcessId;
        public uint dwThreadId;
    }
}
