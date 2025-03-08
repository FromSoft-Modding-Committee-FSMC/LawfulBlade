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

// Include libraries, fuck C++ man. I love C#
#pragma comment(lib, "detours.lib")

// DLL ENTRY POINT WOOHOO
EXTFUNC BOOL APIENTRY DllMain(HMODULE module, DWORD  reason, LPVOID reserved)
{
    switch (reason)
    {
        case DLL_PROCESS_ATTACH:
            LogFInit();
            LogFWrite("Attached Unsealer... IMIN Protocol Successful.", "DllMain");

            // Bind our detours...
            DetourTransactionBegin();
            DetourUpdateThread(GetCurrentThread());

            // SoM Specific - Window
            DetourAttach(&(PVOID&)ProxiedRegisterClassA, ProxyRegisterClassA);

            // SoM Specific - Input
            DetourAttach(&(PVOID&)ProxiedSomInputInit, ProxySomInputInit);
            DetourAttach(&(PVOID&)ProxiedSomInputSetKeyEnabled, ProxySomInputSetKeyEnabled);
            DetourAttach(&(PVOID&)ProxiedSomInputKeyboardPoll, ProxySomInputKeyboardPoll);

            DetourTransactionCommit();

            break;

        case DLL_PROCESS_DETACH:

            // Bind our detours...
            DetourTransactionBegin();
            DetourUpdateThread(GetCurrentThread());

            // SoM Specific - Window
            DetourDetach(&(PVOID&)ProxiedRegisterClassA, ProxyRegisterClassA);

            // SoM Specific - Input
            DetourDetach(&(PVOID&)ProxiedSomInputInit, ProxySomInputInit);
            DetourDetach(&(PVOID&)ProxiedSomInputSetKeyEnabled, ProxySomInputSetKeyEnabled);
            DetourDetach(&(PVOID&)ProxiedSomInputKeyboardPoll, ProxySomInputKeyboardPoll);

            DetourTransactionCommit();

            LogFWrite("Detached Unsealer.", "DllMain");

            break;
    }

    // Always return true or some processes crash
    return TRUE;
}

