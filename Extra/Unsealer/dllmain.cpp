// Move these to a header battenberg
#define EXTFUNC extern "C" __declspec(dllexport)
#define WIN32_LEAN_AND_MEAN

// Third party includes
#include <Windows.h>
#include <detours.h>

// First party includes
#include "unsealconf.h"
#include "unseallog.h"
#include "unsealfilesystem.h"
#include "unsealmemory.h"
#include "unsealtime.h"
#include "unsealplayer.h"
#include "unsealwindow.h"
#include "unsealdraw.h"
#include "unsealgold.h"

// Include libraries, fuck C++ man. I love C#
#pragma comment(lib, "sdk\\detours\\lib\\detours.lib")
#pragma comment(lib, "sdk\\fmod\\lib\\fmod_vc.lib")
#pragma comment(lib, "winmm.lib")

// We're going to hook the main loop function in here...
typedef void(__cdecl* SomMainLoopFunc)(char); SomMainLoopFunc ProxiedSomMainLoopFunc = (SomMainLoopFunc)0x00402410;
void __cdecl ProxySomMainLoopFunc(char param_1)
{
    // Track our updates here...
    // l_currTime = std::chrono::duration_cast<std::chrono::milliseconds>(std::chrono::system_clock::now().time_since_epoch());
    
    // std::ostringstream out;
    // out << "Tick Time: " << (l_lastTime.count() - l_currTime.count());
    // LogFWrite(out.str(), "DllMain>SomMainLoopFun");

    // l_lastTime = l_currTime;

    // Call the original main loop function
    ProxiedSomMainLoopFunc(param_1);
}

// DLL ENTRY POINT WOOHOO
EXTFUNC BOOL APIENTRY DllMain(HMODULE module, DWORD  reason, LPVOID reserved)
{
    // We only care about process attach and detach, which are 1 and 0 respectively
    if ((reason & 0xFFFFFFFE) != 0)
        return TRUE;

    DetourTransactionBegin();
    DetourUpdateThread(GetCurrentThread());

    switch (reason)
    {
        case DLL_PROCESS_ATTACH:
            UnsealConfInit();
            UnsealLoggerInit(g_gameConfigLogLevel);
            UnsealFileSystemInit();
            UnsealMemoryInit();
            UnsealTimeInit();
            UnsealWindowInit();
            UnsealDrawInit();
            UnsealGoldInit();

            UnsealLog("Initialized Unsealer...", "DllMain", UNSEAL_LOG_LEVEL_INFO);
        break;

        case DLL_PROCESS_DETACH:
            UnsealLog("Killing Unsealer...", "DllMain", UNSEAL_LOG_LEVEL_INFO);
            
            UnsealGoldKill();
            UnsealDrawKill();
            UnsealWindowKill();
            UnsealTimeKill();
            UnsealMemoryKill();
            UnsealFileSystemKill();
            UnsealLoggerKill();
            UnsealConfKill();
        break;
    }

    // Finish detour transaction
    DetourTransactionCommit();

    return TRUE;
}

