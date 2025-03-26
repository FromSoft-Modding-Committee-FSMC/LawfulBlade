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
#include "detours.h"
#include "logf.h"
#include "somwindow.h"
#include "sominput.h"
#include "somsound.h"
#include "somgdi.h"
#include "somconf.h"

// Include libraries, fuck C++ man. I love C#
#pragma comment(lib, "sdk\\detours\\lib\\detours.lib")
#pragma comment(lib, "sdk\\fmod\\lib\\fmod_vc.lib")

// Some globals
std::chrono::milliseconds l_lastTime;
std::chrono::milliseconds l_currTime;

// We're going to hook the main loop function in here...
typedef void(__cdecl* SomMainLoopFunc)(char); SomMainLoopFunc ProxiedSomMainLoopFunc = (SomMainLoopFunc)0x00402410;
void __cdecl ProxySomMainLoopFunc(char param_1)
{
    // Any Updating should go here...
    SomSoundTick();

    GetRemappedKeyPressed("ActionMagicCast");

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
    switch (reason)
    {
        case DLL_PROCESS_ATTACH:
            LogFInit();
            LogFWrite("Attached Unsealer... IMIN Protocol Successful.", "DllMain");

            LoadGameConfiguration();
            LogFWrite("Game Config Load OK!", "DllMain");

            LoadUserConfiguration();
            LogFWrite("User Config Load OK!", "DllMain");

            // Bind our detours...
            DetourTransactionBegin();
            DetourUpdateThread(GetCurrentThread());

            // Logger
            // DetourAttach(&(PVOID&)ProxiedOutputDebugStringA, ProxyOutputDebugStringA);

            // Main Loop
            DetourAttach(&(PVOID&)ProxiedSomMainLoopFunc, ProxySomMainLoopFunc);

            // SoM Specific
            SomSoundInitDetours();
            SomGdiInit();

            // SoM Specific - Window
            DetourAttach(&(PVOID&)ProxiedCreateWindowExA, ProxyCreateWindowExA);
            DetourAttach(&(PVOID&)ProxiedRegisterClassA, ProxyRegisterClassA);

            // SoM Specific - Drawing
            DetourAttach(&(PVOID&)ProxiedSomSetDisplay, ProxySomSetDisplayVars);

            // SoM Specific - Input
            DetourAttach(&(PVOID&)ProxiedSomInputInit, ProxySomInputInit);
            DetourAttach(&(PVOID&)ProxiedSomInputSetKeyEnabled, ProxySomInputSetKeyEnabled);
            DetourAttach(&(PVOID&)ProxiedSomInputKeyboardPoll, ProxySomInputKeyboardPoll);
            DetourAttach(&(PVOID&)ProxiedSomInputKeyCheck, ProxySomInputKeyCheck);


            DetourTransactionCommit();

            break;

        case DLL_PROCESS_DETACH:

            // Unbind our detours...
            DetourTransactionBegin();
            DetourUpdateThread(GetCurrentThread());

            // Logger
            // DetourDetach(&(PVOID&)ProxiedOutputDebugStringA, ProxyOutputDebugStringA);

            // Main Loop
            DetourDetach(&(PVOID&)ProxiedSomMainLoopFunc, ProxySomMainLoopFunc);

            // SoM Specific
            SomSoundKillDetours();
            SomGdiKill();

            // SoM Specific - Window
            DetourDetach(&(PVOID&)ProxiedCreateWindowExA, ProxyCreateWindowExA);
            DetourDetach(&(PVOID&)ProxiedRegisterClassA, ProxyRegisterClassA);

            // SoM Specific - Drawing
            DetourDetach(&(PVOID&)ProxiedSomSetDisplay, ProxySomSetDisplayVars);

            // SoM Specific - Input
            DetourDetach(&(PVOID&)ProxiedSomInputInit, ProxySomInputInit);
            DetourDetach(&(PVOID&)ProxiedSomInputSetKeyEnabled, ProxySomInputSetKeyEnabled);
            DetourDetach(&(PVOID&)ProxiedSomInputKeyboardPoll, ProxySomInputKeyboardPoll);
            DetourDetach(&(PVOID&)ProxiedSomInputKeyCheck, ProxySomInputKeyCheck);

            DetourTransactionCommit();

            LogFWrite("Detached Unsealer.", "DllMain");

            break;
    }

    // Always return true or some processes crash
    return TRUE;
}

