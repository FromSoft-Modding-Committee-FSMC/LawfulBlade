// Move these to a header battenberg
#define EXTFUNC extern "C" __declspec(dllexport)
#define WIN32_LEAN_AND_MEAN

// Third party includes
#include <Windows.h>
#include <fstream>
#include <iostream>
#include <sstream>
#include <string>
#include <chrono>

// First party includes
#include "sdk/detours/inc/detours.h"
#include "logf.h"
#include "somwindow.h"
#include "sominput.h"
#include "somsound.h"
#include "somgdi.h"
#include "somconf.h"
#include "somscreeneffect.h"
#include "somdraw.h"
#include "somgold.h"

// Include libraries, fuck C++ man. I love C#
#pragma comment(lib, "sdk\\detours\\lib\\detours.lib")
#pragma comment(lib, "sdk\\fmod\\lib\\fmod_vc.lib")
#pragma comment(lib, "winmm.lib")

// Some globals
std::chrono::milliseconds l_lastTime;
std::chrono::milliseconds l_currTime;

// We're going to hook the main loop function in here...
typedef void(__cdecl* SomMainLoopFunc)(char); SomMainLoopFunc ProxiedSomMainLoopFunc = (SomMainLoopFunc)0x00402410;
void __cdecl ProxySomMainLoopFunc(char param_1)
{
    UnsealScreenEffectTick();
    SomInputTick();
    SomSoundTick();

    // Track our updates here...
    // l_currTime = std::chrono::duration_cast<std::chrono::milliseconds>(std::chrono::system_clock::now().time_since_epoch());
    
    // std::ostringstream out;
    // out << "Tick Time: " << (l_lastTime.count() - l_currTime.count());
    // LogFWrite(out.str(), "DllMain>SomMainLoopFun");

    // l_lastTime = l_currTime;

    // Call the original main loop function
    ProxiedSomMainLoopFunc(param_1);

    // Don't show the FUCKING CURSOR PLEASE
    ShowCursor(FALSE);
}

// DLL ENTRY POINT WOOHOO
EXTFUNC BOOL APIENTRY DllMain(HMODULE module, DWORD  reason, LPVOID reserved)
{
    switch (reason)
    {
        case DLL_PROCESS_ATTACH:
            LogFInit();
            LogFWrite("Attached Unsealer... 'IM-IN' Protocol Successful.", "DllMain");

            LogFWrite("Loading Game Config...", "DllMain");
            LoadGameConfiguration();
            LogFWrite("Game Config Load OK!", "DllMain");

            LogFWrite("Loading User Config...", "DllMain");
            LoadUserConfiguration();
            LogFWrite("User Config Load OK!", "DllMain");

            // Bind our detours...
            DetourTransactionBegin();
            DetourUpdateThread(GetCurrentThread());

            // Main Loop
            UnsealDrawInit();
            UnsealScreenEffectInit();
            UnsealGoldInit();

            SomSoundInitDetours();
            SomInputInitDetours();
            SomGdiInit();

            // DetourAttach(&(PVOID&)ProxiedOutputDebugStringA, ProxyOutputDebugStringA);
            DetourAttach(&(PVOID&)ProxiedSomMainLoopFunc, ProxySomMainLoopFunc);
            DetourAttach(&(PVOID&)ProxiedCreateWindowExA, ProxyCreateWindowExA);
            DetourAttach(&(PVOID&)ProxiedRegisterClassA, ProxyRegisterClassA);
            DetourAttach(&(PVOID&)ProxiedSomSetDisplay, ProxySomSetDisplayVars);


            DetourTransactionCommit();
            break;

        case DLL_PROCESS_DETACH:
            DetourTransactionBegin();
            DetourUpdateThread(GetCurrentThread());

            UnsealDrawKill();
            UnsealScreenEffectKill();   
            UnsealGoldKill();

            SomSoundKillDetours();
            SomInputKillDetours();
            SomGdiKill();

            // DetourDetach(&(PVOID&)ProxiedOutputDebugStringA, ProxyOutputDebugStringA);
            DetourDetach(&(PVOID&)ProxiedSomMainLoopFunc, ProxySomMainLoopFunc);
            DetourDetach(&(PVOID&)ProxiedCreateWindowExA, ProxyCreateWindowExA);
            DetourDetach(&(PVOID&)ProxiedRegisterClassA, ProxyRegisterClassA);
            DetourDetach(&(PVOID&)ProxiedSomSetDisplay, ProxySomSetDisplayVars);

            DetourTransactionCommit();

            LogFWrite("Detached Unsealer.", "DllMain");
            break;
    }

    // Always return true or some processes crash
    return TRUE;
}

