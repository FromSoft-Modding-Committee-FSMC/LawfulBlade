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

// Include libraries, fuck C++ man. I love C#
#pragma comment(lib, "detours.lib")

// DLL ENTRY POINT WOOHOO
BOOL APIENTRY DllMain(HMODULE module, DWORD  reason, LPVOID reserved)
{
    switch (reason)
    {
        case DLL_PROCESS_ATTACH:
            MessageBoxA(NULL, "I'm in", "Caption", 0);
            break;

        case DLL_PROCESS_DETACH:
            MessageBoxA(NULL, "I'm out", "Caption", 0);
            break;
    }

    // Always return true or some processes crash
    return TRUE;
}

