// Move these to a header battenberg
#define EXTFUNC extern "C" __declspec(dllexport)
#define WIN32_LEAN_AND_MEAN

// Third party includes
#include <Windows.h>
#include <fstream>
#include <iostream>
#include <sstream>
#include <string>

// First party includes
#include "detours.h"
#include "logf.h"
#include "somwindow.h"
#include "sominput.h"
#include "somsound.h"
#include "somconf.h"

// Include libraries, fuck C++ man. I love C#
#pragma comment(lib, "sdk\\detours\\lib\\detours.lib")
#pragma comment(lib, "sdk\\fmod\\lib\\fmod_vc.lib")

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
            DetourAttach(&(PVOID&)ProxiedOutputDebugStringA, ProxyOutputDebugStringA);

            // SoM Specific
            SomSoundInitDetours();

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

            // TEMP TO MOVE
            DetourAttach(&(PVOID&)ProxiedSomSoundCreateBuffer, ProxySomSoundCreateBuffer);

            DetourTransactionCommit();

            break;

        case DLL_PROCESS_DETACH:

            // Unbind our detours...
            DetourTransactionBegin();
            DetourUpdateThread(GetCurrentThread());

            // Logger
            DetourDetach(&(PVOID&)ProxiedOutputDebugStringA, ProxyOutputDebugStringA);

            // SoM Specific
            SomSoundKillDetours();

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


            // TEMP TO MOVE
            DetourDetach(&(PVOID&)ProxiedSomSoundCreateBuffer, ProxySomSoundCreateBuffer);

            DetourTransactionCommit();

            LogFWrite("Detached Unsealer.", "DllMain");

            break;
    }

    // Always return true or some processes crash
    return TRUE;
}

