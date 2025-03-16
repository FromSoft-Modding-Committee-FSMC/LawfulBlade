#include <Windows.h>
#include <hidusage.h>

#include "logf.h"

#include "sominput.h"
#include "somwindow.h"
#include "somconf.h"

// Define SOM keys here...
#define SOM_KEYS_MVBWD 0xD0 // DIK_DOWN         Move player backwards, scroll menu down
#define SOM_KEYS_MVFWD 0xC8 // DIK_UP           Move player forwards, scroll menu up
#define SOM_KEYS_MVRGT 0x4D // DIK_NUMPAD6      Move player right
#define SOM_KEYS_MVLFT 0x4B // DIK_NUMPAD4      Move player left

#define SOM_KEYS_RTRGT 0xCD // DIK_RIGHT        Rotate player right
#define SOM_KEYS_RTLFT 0xCB // DIK_LEFT         Rotate player left
#define SOM_KEYS_LKUP 0x49  // DIK_NUMPAD9      Look Up
#define SOM_KEYS_LKDWN 0x47 // DIK_NUMPAD7      Look Down

#define SOM_KEYS_ATTAK 0x2A // DIK_LSHIFT       Attack
#define SOM_KEYS_MAGIC 0x1D // DIK_LCONTROL     Magic
#define SOM_KEYS_EVENT 0x39 // DIK_SPACE        Event/Interaction

#define SOM_KEYS_MENU 0xF   // DIK_TAB          Menu Enter
#define SOM_KEYS_BACK 0x1   // DIK_ESCAPE       Skip Sequence, Exit Menu
#define SOM_KEYS_START 0x1C // DIK_RETURN       Play Game, Menu Accept

// Created Memory
bool g_somCurrKeyState[256];
bool g_somLastKeyState[256];
bool g_somCurrMouseState[8];
bool g_somLastMouseState[8];

// Function Definition
SomInputInit ProxiedSomInputInit = (SomInputInit)0x0040e590;
SomInputSetKeyEnabled ProxiedSomInputSetKeyEnabled = (SomInputSetKeyEnabled)0x004499e0;
SomInputKeyboardPoll ProxiedSomInputKeyboardPoll = (SomInputKeyboardPoll)0x00449940;
SomInputKeyCheck ProxiedSomInputKeyCheck = (SomInputKeyCheck)0x00449990;

// Some Stolen Variables
float* g_somCameraX = (float*)0x019C0BB8;
float* g_somCameraY = (float*)0x019C0BB4;

// Proxies
bool __cdecl ProxySomInputInit()
{
    LogFWrite("Initializing Input System...", "ProxySomInputInit");

    // Get Raw Input Devices. We're going to register too them then gut SoM's default input crap
    UINT numRawInputDevice = 0;

    if (GetRawInputDeviceList(NULL, &numRawInputDevice, sizeof(RAWINPUTDEVICELIST)) != 0)
    {
        LogFWrite("Error! Failed to get Raw Input Device Count!", "ProxySomInputInit");
    }
    else 
    {
        std::ostringstream out;
        out << "{ Num Device = " << numRawInputDevice << "}";
        LogFWrite(out.str(), "ProxySomInputInit");

        // Register our raw input devices...
        RAWINPUTDEVICE Rid[2];

        // Keyboard
        Rid[0].usUsagePage = HID_USAGE_PAGE_GENERIC;
        Rid[0].usUsage     = HID_USAGE_GENERIC_KEYBOARD;
        Rid[0].dwFlags     = RIDEV_NOLEGACY;
        Rid[0].hwndTarget  = g_somHwnd1;

        // Mouse
        Rid[1].usUsagePage = HID_USAGE_PAGE_GENERIC;
        Rid[1].usUsage     = HID_USAGE_GENERIC_MOUSE;
        Rid[1].dwFlags     = RIDEV_NOLEGACY;
        Rid[1].hwndTarget = g_somHwnd1;

        if (!RegisterRawInputDevices(Rid, 2, sizeof(Rid[0])))
        {
            std::ostringstream out;
            out << "Failed to register RawInput devices! { error = " << GetLastError() << " }";

            LogFWrite(out.str(), "ProxySomInputInit");
        }
    }

    return ProxiedSomInputInit();
}

/// <summary>
/// Unknown Function.<br/>
/// This would appear to disable a key, or set repeat for the key?.. Not sure.
/// </summary>
bool __cdecl ProxySomInputSetKeyEnabled(unsigned char keyID, bool isEnabled)
{
    return true; // ProxiedSomInputSetKeyEnabled(keyID, isEnabled);
}

/// <summary>
/// Pushes the current keyboard state to last, and aquires a new current state.
/// </summary>
/// <returns></returns>
bool __cdecl ProxySomInputKeyboardPoll()
{
    // Copy our current keystates into the last keystates...
    memcpy_s(g_somLastKeyState, sizeof(g_somLastKeyState), g_somCurrKeyState, sizeof(g_somLastKeyState));

    // Copy our current mouse states into the last mouse states
    memcpy_s(g_somLastMouseState, sizeof(g_somLastMouseState), g_somCurrMouseState, sizeof(g_somLastMouseState));

    return true; //ProxiedSomInputKeyboardPoll();
}

bool GetRemappedKey(const char* keyConfName)
{
    // Get the configuration value from the mappings list
    unsigned int userMapping = GetUserConfigU32(keyConfName);

    // What device is mapped?
    BYTE mappedDevice = (userMapping & 0x0F000000) >> 24;
    BYTE mappedInput = (userMapping & 0x000000FF);

    // Keyboard Device
    if (mappedDevice == 1)
    {
        return g_somCurrKeyState[mappedInput];
    }
    else
    if (mappedDevice == 2)
    {
        return g_somCurrMouseState[mappedInput];
    }

    // Unknown Device or Unbound
    return false;
}

bool __cdecl ProxySomInputKeyCheck(unsigned char keyID, bool param_2)
{
    // Translate our DIK into VK - Rewrite this mess...
    switch (keyID)
    {
        // Move
        case SOM_KEYS_MVFWD: return GetRemappedKey("MovePlayerForward");
        case SOM_KEYS_MVBWD: return GetRemappedKey("MovePlayerBack");
        case SOM_KEYS_MVRGT: return GetRemappedKey("MovePlayerRight");
        case SOM_KEYS_MVLFT: return GetRemappedKey("MovePlayerLeft");

        // Look
        case SOM_KEYS_RTRGT: return GetRemappedKey("TurnPlayerRight");
        case SOM_KEYS_RTLFT: return GetRemappedKey("TurnPlayerLeft");
        case SOM_KEYS_LKUP:  return GetRemappedKey("LookPlayerUp");
        case SOM_KEYS_LKDWN: return GetRemappedKey("LookPlayerDown");

        // Action
        case SOM_KEYS_ATTAK: return GetRemappedKey("ActionAttack");
        case SOM_KEYS_MAGIC: return GetRemappedKey("ActionMagicCast");
        case SOM_KEYS_EVENT: return GetRemappedKey("ActionInspect");

        // Menu
        case SOM_KEYS_MENU:  return GetRemappedKey("ActionOpenMenu");
        case SOM_KEYS_START: return GetRemappedKey("ActionAcceptMenu");
        case SOM_KEYS_BACK:  return GetRemappedKey("ActionCloseMenu");
    }
    
    return false;
}