using Microsoft.Win32.SafeHandles;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Windows.Win32.Foundation;
using Windows.Win32.System.Threading;
using PInvokeDetours = Microsoft.Detours.PInvoke;
using PInvokeWin32 = Windows.Win32.PInvoke;

namespace Sealed_Sword_Stone
{
    // All of this is gracefully stolem from here:
    // https://github.com/lowleveldesign/withdll/tree/main


    sealed record ProcessStartupConfig(List<string> CmdlineArgs, bool Debug, bool NewConsole, bool WaitForExit);

    static class Injector
    {
        public static void StartProcessWithDlls(ProcessStartupConfig config, List<string> dllPaths)
        {
            unsafe
            {
                var startupInfo = new STARTUPINFOW() { cb = (uint)sizeof(STARTUPINFOW) };

                var cmdline = new Span<char>(ConvertStringListToNullTerminatedArray(config.CmdlineArgs));
                uint createFlags = (config.Debug ? (uint)PROCESS_CREATION_FLAGS.DEBUG_ONLY_THIS_PROCESS : 0) |
                    (config.NewConsole ? (uint)PROCESS_CREATION_FLAGS.CREATE_NEW_CONSOLE : 0);

                var pcstrs = dllPaths.Select(p => new PCSTR((byte*)Marshal.StringToHGlobalAnsi(p))).ToArray();

                try
                {
                    if (!PInvokeDetours.DetourCreateProcessWithDlls(null, ref cmdline, null, null, false,
                        createFlags, null, null, startupInfo, out var processInfo,
                        pcstrs, null))
                    {
                        throw new Win32Exception();
                    }

                    PInvokeWin32.CloseHandle(processInfo.hThread);

                    if (config.Debug)
                    {
                        PInvokeWin32.DebugActiveProcessStop(processInfo.dwProcessId);
                    }

                    if (config.WaitForExit)
                    {
                        PInvokeWin32.WaitForSingleObject(processInfo.hProcess, PInvokeWin32.INFINITE);
                    }

                    PInvokeWin32.CloseHandle(processInfo.hProcess);
                }
                finally
                {
                    Array.ForEach(pcstrs, pcstr => Marshal.FreeHGlobal((nint)pcstr.Value));
                }
            }

            static char[] ConvertStringListToNullTerminatedArray(IList<string> strings)
            {
                var chars = new List<char>(strings.Select(a => a.Length + 3 /* two apostrophes and space */).Sum());
                foreach (var s in strings)
                {
                    chars.Add('\"');
                    chars.AddRange(s);
                    chars.Add('\"');
                    chars.Add(' ');
                }
                chars[^1] = '\0';
                return [.. chars];
            }
        }
    }

    /// <summary>
    /// NT function definitions are modified (or not) versions
    /// from https://github.com/googleprojectzero/sandbox-attacksurface-analysis-tools
    /// 
    /// //  Copyright 2019 Google Inc. All Rights Reserved.
    ///
    ///  Licensed under the Apache License, Version 2.0 (the "License");
    ///  you may not use this file except in compliance with the License.
    ///  You may obtain a copy of the License at
    ///
    ///  http://www.apache.org/licenses/LICENSE-2.0
    ///
    ///  Unless required by applicable law or agreed to in writing, software
    ///  distributed under the License is distributed on an "AS IS" BASIS,
    ///  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    ///  See the License for the specific language governing permissions and
    ///  limitations under the License.
    /// </summary>
    internal static partial class Imports
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct CLIENT_ID
        {
            public IntPtr UniqueProcess;
            public IntPtr UniqueThread;
        }

        [LibraryImport("ntdll.dll")]
        internal static partial int RtlCreateUserThread(
            nint ProcessHandle,
            nint ThreadSecurityDescriptor,
            [MarshalAs(UnmanagedType.Bool)]
        bool CreateSuspended,
            uint ZeroBits,
            nuint MaximumStackSize,
            nuint CommittedStackSize,
            nint StartAddress,
            nint Parameter,
            out SafeFileHandle ThreadHandle,
            out CLIENT_ID ClientId
        );

        [LibraryImport("ntdll.dll")]
        public static partial int NtQueueApcThread(
             nint ThreadHandle,
             nint ApcRoutine,
             nint ApcArgument1,
             nint ApcArgument2,
             nint ApcArgument3
        );

        [LibraryImport("ntdll.dll")]
        internal static partial int RtlQueueApcWow64Thread(
            nint ThreadHandle,
            nint ApcRoutine,
            nint ApcArgument1,
            nint ApcArgument2,
            nint ApcArgument3
        );
    }
}
