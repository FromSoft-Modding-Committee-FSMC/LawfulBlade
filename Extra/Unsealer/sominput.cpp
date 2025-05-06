#include <Windows.h>
#include <hidusage.h>
#include <hidsdi.h>
#include <map>

#include "logf.h"

#include "somgamedata.h"
#include "sominput.h"
#include "somwindow.h"
#include "somconf.h"

#include "detours.h"


#define UNSEAL_SYSDEV_KEYBOARD 0x1
#define UNSEAL_SYSDEV_MOUSE    0x2
#define UNSEAL_JOYDEV_BUTTON   0x1
#define UNSEAL_JOYDEV_AXIS     0x2
#define UNSEAL_JOYDEV_HAT      0x3

// Stolen Memory
uint32_t& m_somInputState = *(uint32_t*)0x005553d4;

// Created Memory
int32_t g_somMouseAccumulatorX = 0, g_somMouseAccumulatorY = 0;
bool  g_somCurrKeyState[256], g_somLastKeyState[256];    // 256 Keys
bool  g_somCurrMouseState[8], g_somLastMouseState[8];    // 8   Mouse
bool  g_somCurrPadState[128], g_somLastPadState[128];    // 128 Pad Buttons
int32_t g_somCurrPadValue[128], g_somLastPadValue[128];    // 128 Pad Values (AXIS etc...)

int32_t g_somSmoothedMouseX, g_somSmoothedMouseY, g_somSmoothedMousePrevX = 0, g_somSmoothedMousePrevY = 0;
float   g_somSmoothingVal = 0.5f;

std::map<std::string, uint32_t> m_controlMap;

void UnsealProcessRawInput(HRAWINPUT rawInput)
{
    // Get the size of our raw input buffer...
    UINT dwSize = 0;
    GetRawInputData(rawInput, RID_INPUT, NULL, &dwSize, sizeof(RAWINPUTHEADER));

    // Don't process empty buffers
    if (dwSize == 0)
        return;

    // Get the raw input buffer itself
    RAWINPUT* rawBuffer = new RAWINPUT;
    if (rawBuffer == NULL)
    {
        LogFWrite("Raw Input Buffer is NULL!", "SomInput>UnsealProcessRawInput");
        return;
    }

    if (GetRawInputData(rawInput, RID_INPUT, rawBuffer, &dwSize, sizeof(RAWINPUTHEADER)) != dwSize)
    {
        LogFWrite("GetRawInputData does not return correct size !", "SomInput>UnsealProcessRawInput");
        delete rawBuffer;
        return;
    }

    // Depending on type of Raw Input, we do something different...
    switch (rawBuffer->header.dwType)
    {
        // Keyboard Type Device
        case RIM_TYPEKEYBOARD:
            // Keys
            g_somCurrKeyState[rawBuffer->data.keyboard.VKey & 0xFF] = ((rawBuffer->data.keyboard.Flags & 0x1) == 0);
        break;

        // Mouse Type Device
        case RIM_TYPEMOUSE:
            // Buttons
            g_somCurrMouseState[0] = (((rawBuffer->data.mouse.usButtonFlags >>  0) & 0x3) != 0);
            g_somCurrMouseState[1] = (((rawBuffer->data.mouse.usButtonFlags >>  2) & 0x3) != 0);
            g_somCurrMouseState[2] = (((rawBuffer->data.mouse.usButtonFlags >>  4) & 0x3) != 0);
            g_somCurrMouseState[3] = (((rawBuffer->data.mouse.usButtonFlags >>  6) & 0x3) != 0);
            g_somCurrMouseState[4] = (((rawBuffer->data.mouse.usButtonFlags >>  8) & 0x3) != 0);
            g_somCurrMouseState[5] = (((rawBuffer->data.mouse.usButtonFlags >> 10) & 0x3) != 0);
            g_somCurrMouseState[6] = (((rawBuffer->data.mouse.usButtonFlags >> 12) & 0x3) != 0);
            g_somCurrMouseState[7] = (((rawBuffer->data.mouse.usButtonFlags >> 14) & 0x3) != 0);

            // Motion
            g_somMouseAccumulatorX += rawBuffer->data.mouse.lLastX;
            g_somMouseAccumulatorY += rawBuffer->data.mouse.lLastY;
        break;

        // Generic - in our case, joystick or gamepad
        case RIM_TYPEHID:
            /*
            // Check the size of the preparsed data chunk
            GetRawInputDeviceInfoW(rawBuffer->header.hDevice, RIDI_PREPARSEDDATA, NULL, &dwSize);
            if (dwSize == 0)
                break;

            // Now grab the preparsed data...
            PHIDP_PREPARSED_DATA parsedBuffer = (PHIDP_PREPARSED_DATA)malloc(dwSize);

            if (parsedBuffer == NULL)
            {
                LogFWrite("Device Info Buffer is NULL!", "SomInput>UnsealProcessRawInput");
                break;
            }

            if (GetRawInputDeviceInfoW(rawBuffer->header.hDevice, RIDI_PREPARSEDDATA, &parsedBuffer, &dwSize) != dwSize)
            {
                LogFWrite("GetRawInputDeviceInfoW does not return correct size !", "SomInput>UnsealProcessRawInput");
                free(parsedBuffer);
                break;
            }

            // Get the HID capabilities
            HIDP_CAPS hidCaps;
            if (HidP_GetCaps(parsedBuffer, &hidCaps) != HIDP_STATUS_SUCCESS)
            {
                LogFWrite("Failed to get HID caps!", "SomInput>UnsealProcessRawInput");
                free(parsedBuffer);
                break;
            }
            
            // Get Button Caps
            PHIDP_BUTTON_CAPS hidButtonCaps = (PHIDP_BUTTON_CAPS)malloc(sizeof(HIDP_BUTTON_CAPS) * hidCaps.NumberInputButtonCaps);
            if (HidP_GetButtonCaps(HidP_Input, hidButtonCaps, &hidCaps.NumberInputButtonCaps, parsedBuffer) != HIDP_STATUS_SUCCESS)
            {
                LogFWrite("Failed to get HID Button Caps!", "SomInput>UnsealProcessRawInput");
                free(hidButtonCaps);
                free(parsedBuffer);
                break;
            }
            int numButton = (hidButtonCaps->Range.UsageMax - hidButtonCaps->Range.UsageMin) + 1;

            // Get Value Caps

            free(parsedBuffer);
            */
        break;
    }

    // Free any data we allocated
    delete rawBuffer;
}

bool UnsealGetInputPressed(const char* inputName)
{
    // Make sure that if a mapping doesn't exist, we return false.
    if (m_controlMap.count(inputName) == 0)
        return false;

    // Grab the mapping
    uint32_t mapping  = m_controlMap[inputName];

    // If it's unmapped, return false.
    if (mapping == 0x00FFFFFF)
        return false;

    // Unpack the mapping
    uint8_t sysdevMain = (mapping >> 0)  & 0xFF;
    uint8_t sysdevAlt  = (mapping >> 8)  & 0xFF;
    uint8_t joypadBind = (mapping >> 16) & 0xFF;

    bool inputPressed = false;

    // Main System Device Check
    switch ((mapping >> 26) & 0x3)
    {
        case UNSEAL_SYSDEV_KEYBOARD:
            inputPressed |= (g_somCurrKeyState[sysdevMain] & !g_somLastKeyState[sysdevMain]);
            break;

        case UNSEAL_SYSDEV_MOUSE:
            inputPressed |= (g_somCurrMouseState[sysdevMain & 0x7] & !g_somLastMouseState[sysdevMain & 0x7]);
            break;
    }

    // Alt System Device Check
    switch ((mapping >> 24) & 0x3)
    {
        case UNSEAL_SYSDEV_KEYBOARD:
            inputPressed |= (g_somCurrKeyState[sysdevAlt] & !g_somLastKeyState[sysdevAlt]);
        break;

        case UNSEAL_SYSDEV_MOUSE:
            inputPressed |= (g_somCurrMouseState[sysdevAlt & 0x7] & !g_somLastMouseState[sysdevAlt & 0x7]);
        break;
    }

    // Gamepad Implementation
    switch ((mapping >> 28) & 0xF)
    {
        case UNSEAL_JOYDEV_BUTTON:
        break;

        case UNSEAL_JOYDEV_AXIS:
        break;

        case UNSEAL_JOYDEV_HAT:
        break;
    }

    return inputPressed;
}

bool UnsealGetInputReleased(const char* inputName)
{
    // Make sure that if a mapping doesn't exist, we return false.
    if (m_controlMap.count(inputName) == 0)
        return false;

    // Grab the mapping
    uint32_t mapping = m_controlMap[inputName];

    // If it's unmapped, return false.
    if (mapping == 0x00FFFFFF)
        return false;

    // Unpack the mapping
    uint8_t sysdevMain = (mapping >> 0)  & 0xFF;
    uint8_t sysdevAlt  = (mapping >> 8)  & 0xFF;
    uint8_t joypadBind = (mapping >> 16) & 0xFF;

    bool inputReleased = false;

    // Main System Device Check
    switch ((mapping >> 26) & 0x3)
    {
        case UNSEAL_SYSDEV_KEYBOARD:
            inputReleased |= (!g_somCurrKeyState[sysdevMain] & g_somLastKeyState[sysdevMain]);
        break;

        case UNSEAL_SYSDEV_MOUSE:
            inputReleased |= (!g_somCurrMouseState[sysdevMain & 0x7] & g_somLastMouseState[sysdevMain & 0x7]);
        break;
    }

    // Alt System Device Check
    switch ((mapping >> 24) & 0x3)
    {
        case UNSEAL_SYSDEV_KEYBOARD:
            inputReleased |= (!g_somCurrKeyState[sysdevAlt] & g_somLastKeyState[sysdevAlt]);
        break;

        case UNSEAL_SYSDEV_MOUSE:
            inputReleased |= (!g_somCurrMouseState[sysdevAlt & 0x7] & g_somLastMouseState[sysdevAlt & 0x7]);
        break;
    }

    // Gamepad Implementation
    switch ((mapping >> 28) & 0xF)
    {
        case UNSEAL_JOYDEV_BUTTON:
        break;

        case UNSEAL_JOYDEV_AXIS:
        break;

        case UNSEAL_JOYDEV_HAT:
        break;
    }

    return inputReleased;
}

bool UnsealGetInputHeld(const char* inputName)
{
    // Make sure that if a mapping doesn't exist, we return false.
    if (m_controlMap.count(inputName) == 0)
        return false;

    // Grab the mapping
    uint32_t mapping = m_controlMap[inputName];

    // If it's unmapped, return false.
    if (mapping == 0x00FFFFFF)
        return false;

    // Unpack the mapping
    uint8_t sysdevMain = (mapping >> 0)  & 0xFF;
    uint8_t sysdevAlt  = (mapping >> 8)  & 0xFF;
    uint8_t joypadBind = (mapping >> 16) & 0xFF;

    bool inputHeld = false;

    // Main System Device Check
    switch ((mapping >> 26) & 0x3)
    {
        case UNSEAL_SYSDEV_KEYBOARD:
            inputHeld |= g_somCurrKeyState[sysdevMain];
        break;

        case UNSEAL_SYSDEV_MOUSE:
            inputHeld |= g_somCurrMouseState[sysdevMain & 0x7];
        break;
    }

    // Alt System Device Check
    switch ((mapping >> 24) & 0x3)
    {
        case UNSEAL_SYSDEV_KEYBOARD:
            inputHeld |= g_somCurrKeyState[sysdevAlt];
        break;

        case UNSEAL_SYSDEV_MOUSE:
            inputHeld |= g_somCurrMouseState[sysdevAlt & 0x7];
        break;
    }

    // Gamepad Implementation
    switch ((mapping >> 28) & 0xF)
    {
        case UNSEAL_JOYDEV_BUTTON:
        break;

        case UNSEAL_JOYDEV_AXIS:
        break;

        case UNSEAL_JOYDEV_HAT:
        break;
    }

    return inputHeld;
}

/// <summary>
/// Initializes the input system.
/// </summary>
SomInputInit ProxiedSomInputInit = (SomInputInit)0x0040e590;
bool __cdecl ProxySomInputInit()
{
    LogFWrite("Initializing Input System...", "SomInput>ProxySomInputInit");

    // Register our raw input devices...
    RAWINPUTDEVICE rawDevices[4];

    // Keyboard
    rawDevices[0].usUsagePage = HID_USAGE_PAGE_GENERIC;
    rawDevices[0].usUsage = HID_USAGE_GENERIC_KEYBOARD;
    rawDevices[0].dwFlags = RIDEV_NOLEGACY;
    rawDevices[0].hwndTarget = g_somHwnd1;

    // Mouse
    rawDevices[1].usUsagePage = HID_USAGE_PAGE_GENERIC;
    rawDevices[1].usUsage = HID_USAGE_GENERIC_MOUSE;
    rawDevices[1].dwFlags = RIDEV_NOLEGACY;
    rawDevices[1].hwndTarget = g_somHwnd1;

    // DINPUT Controller
    rawDevices[2].usUsagePage = HID_USAGE_PAGE_GENERIC;
    rawDevices[2].usUsage = HID_USAGE_GENERIC_JOYSTICK;
    rawDevices[2].dwFlags = RIDEV_INPUTSINK;
    rawDevices[2].hwndTarget = g_somHwnd1;

    // XINPUT Controller
    rawDevices[3].usUsagePage = HID_USAGE_PAGE_GENERIC;
    rawDevices[3].usUsage = HID_USAGE_GENERIC_GAMEPAD;
    rawDevices[3].dwFlags = RIDEV_INPUTSINK;
    rawDevices[3].hwndTarget = g_somHwnd1;

    if (!RegisterRawInputDevices(rawDevices, 4, sizeof(rawDevices[0])))
    {
        std::ostringstream out;
        out << "Failed to register RawInput devices! { error = " << GetLastError() << " }";
        LogFWrite(out.str(), "SomInput>ProxySomInputInit");
    }

    // Now load the control configuration to our mappings
    m_controlMap["PlayerMoveForward"] = GetUserConfigU32("PlayerMoveForward");
    m_controlMap["PlayerMoveBackward"] = GetUserConfigU32("PlayerMoveBackward");
    m_controlMap["PlayerStrafeLeft"] = GetUserConfigU32("PlayerStrafeLeft");
    m_controlMap["PlayerStrafeRight"] = GetUserConfigU32("PlayerStrafeRight");
    m_controlMap["PlayerTurnLeft"] = GetUserConfigU32("PlayerTurnLeft");
    m_controlMap["PlayerTurnRight"] = GetUserConfigU32("PlayerTurnRight");
    m_controlMap["PlayerLookUp"] = GetUserConfigU32("PlayerLookUp");
    m_controlMap["PlayerLookDown"] = GetUserConfigU32("PlayerLookDown");
    m_controlMap["ActionAttack"] = GetUserConfigU32("ActionAttack");
    m_controlMap["ActionCast"] = GetUserConfigU32("ActionCast");
    m_controlMap["ActionInspect"] = GetUserConfigU32("ActionInspect");
    m_controlMap["ActionSprint"] = GetUserConfigU32("ActionSprint");
    m_controlMap["MenuOpen"] = GetUserConfigU32("MenuOpen");
    m_controlMap["MenuConfirm"] = GetUserConfigU32("MenuConfirm");
    m_controlMap["MenuCancel"] = GetUserConfigU32("MenuCancel");
    m_controlMap["MenuUp"] = GetUserConfigU32("MenuUp");
    m_controlMap["MenuDown"] = GetUserConfigU32("MenuDown");
    m_controlMap["MenuLeft"] = GetUserConfigU32("MenuLeft");
    m_controlMap["MenuRight"] = GetUserConfigU32("MenuRight");

    // SoM wants true or 1.
    return true;
}

/// <summary>
/// This would appear to disable a key, or set repeat for the key?.. Not sure.
/// </summary>
SomInputSetKeyEnabled ProxiedSomInputSetKeyEnabled = (SomInputSetKeyEnabled)0x004499e0;
bool __cdecl ProxySomInputSetKeyEnabled(unsigned char keyID, bool isEnabled)
{
    return true;
}

/// <summary>
/// Pushes the current keyboard state to last, and aquires a new current state.
/// </summary>
SomInputKeyboardPoll ProxiedSomInputKeyboardPoll = (SomInputKeyboardPoll)0x00449940;
bool __cdecl ProxySomInputKeyboardPoll()
{
    return true;
}

/// <summary>
/// Called by SoM to check if a key is pressed...
/// </summary>
SomInputKeyCheck ProxiedSomInputKeyCheck = (SomInputKeyCheck)0x00449990;
bool __cdecl ProxySomInputKeyCheck(unsigned char keyID, bool param_2)
{
    return true;
}

/// <summary>
/// Called by SoM to build the input buffer
/// </summary>
SomInputBufferBuild ProxiedSomInputBufferBuild = (SomInputBufferBuild)0x0040e680;
void __cdecl ProxySomInputBufferBuild()
{
    // Clear Input State...
    m_somInputState  = 0x00000000;
    
    // MOVEMENT
    m_somInputState |= (UnsealGetInputHeld("PlayerMoveForward") ?  0x00000010 : 0x00000000);    // Move Forwards
    m_somInputState |= (UnsealGetInputHeld("PlayerMoveBackward") ? 0x00000020 : 0x00000000);    // Move Backwards
    m_somInputState |= (UnsealGetInputHeld("PlayerStrafeLeft") ?   0x00000040 : 0x00000000);    // Strafe Left
    m_somInputState |= (UnsealGetInputHeld("PlayerStrafeRight") ?  0x00000080 : 0x00000000);    // Strafe Right

    // LOOK
    m_somInputState |= (UnsealGetInputHeld("PlayerTurnLeft")  ? 0x00000100 : 0x00000000);
    m_somInputState |= (UnsealGetInputHeld("PlayerTurnRight") ? 0x00000200 : 0x00000000);
    m_somInputState |= (UnsealGetInputHeld("PlayerLookUp")    ? 0x00000400 : 0x00000000);
    m_somInputState |= (UnsealGetInputHeld("PlayerLookDown")  ? 0x00000800 : 0x00000000);

    // ACTION
    m_somInputState |= (UnsealGetInputPressed("ActionAttack")  ? 0x00000001 : 0x00000000);
    m_somInputState |= (UnsealGetInputPressed("ActionCast")    ? 0x00000002 : 0x00000000);
    m_somInputState |= (UnsealGetInputPressed("ActionInspect") ? 0x00000008 : 0x00000000);
    m_somInputState |= (UnsealGetInputHeld("ActionSprint")     ? 0x00002000 : 0x00000000);  // Sprinting

    // MENU
    m_somInputState |= (UnsealGetInputPressed("MenuOpen") ?    0x00000004 : 0x00000000);    // Menu Open?
    m_somInputState |= (UnsealGetInputPressed("MenuConfirm") ? 0x01000000 : 0x00000000);    // Menu Accept
    m_somInputState |= (UnsealGetInputPressed("MenuCancel") ?  0x02000000 : 0x00000000);    // Menu Back - Close... ?
    m_somInputState |= (UnsealGetInputHeld("MenuUp") ?         0x10000000 : 0x00000000);    // Menu Up
    m_somInputState |= (UnsealGetInputHeld("MenuDown") ?       0x20000000 : 0x00000000);    // Menu Down
    m_somInputState |= (UnsealGetInputHeld("MenuLeft") ?       0x40000000 : 0x00000000);    // Menu Left
    m_somInputState |= (UnsealGetInputHeld("MenuRight") ?      0x80000000 : 0x00000000);    // Menu Right

    // Process mouse movement.
    if (GetUserConfigBool("UseMouseLook"))
    {
        float mouseSmooth = max(min(GetUserConfigFloat("MouseSmoothing"), 1.F), 0.F);

        // First apply smoothing.
        g_somSmoothedMouseX     = (int32_t) (mouseSmooth * g_somMouseAccumulatorX + (1.0f - mouseSmooth) * g_somSmoothedMousePrevX);
        g_somSmoothedMouseY     = (int32_t) (mouseSmooth * g_somMouseAccumulatorY + (1.0f - mouseSmooth) * g_somSmoothedMousePrevY);
        g_somSmoothedMousePrevX = g_somSmoothedMouseX;
        g_somSmoothedMousePrevY = g_somSmoothedMouseY;

        // Get mouse sensitivity
        float mouseSense = GetUserConfigFloat("MouseSensitivity");

        // Now Adjust camera by the smoothed vector
        *g_somCameraX -= (g_somSmoothedMouseX / 1024.f) * mouseSense;
        *g_somCameraY -= (g_somSmoothedMouseY / 1024.f) * mouseSense;
    }

    // Clear the mouse movement accumulator now we've processed it
    g_somMouseAccumulatorX = 0;
    g_somMouseAccumulatorY = 0;

    // Copy the current key,mouse and pad states to a history buffer
    memcpy_s(g_somLastKeyState,   sizeof(g_somLastKeyState),   g_somCurrKeyState,   sizeof(g_somLastKeyState));
    memcpy_s(g_somLastMouseState, sizeof(g_somLastMouseState), g_somCurrMouseState, sizeof(g_somLastMouseState));
    memcpy_s(g_somLastPadState,   sizeof(g_somLastPadState),   g_somCurrPadState,   sizeof(g_somLastMouseState));
    memcpy_s(g_somLastPadValue,   sizeof(g_somLastPadValue),   g_somCurrPadValue,   sizeof(g_somLastPadValue));

    // Disable the cursor here because we can?..
    ShowCursor(FALSE);
}

// Hook N Fuck - Init, Deinit, Tick
void __cdecl SomInputInitDetours()
{
    DetourAttach(&(PVOID&)ProxiedSomInputInit, ProxySomInputInit);
    DetourAttach(&(PVOID&)ProxiedSomInputSetKeyEnabled, ProxySomInputSetKeyEnabled);
    DetourAttach(&(PVOID&)ProxiedSomInputKeyboardPoll, ProxySomInputKeyboardPoll);
    DetourAttach(&(PVOID&)ProxiedSomInputKeyCheck, ProxySomInputKeyCheck);
    DetourAttach(&(PVOID&)ProxiedSomInputBufferBuild, ProxySomInputBufferBuild);
}

void __cdecl SomInputKillDetours()
{
    DetourDetach(&(PVOID&)ProxiedSomInputInit, ProxySomInputInit);
    DetourDetach(&(PVOID&)ProxiedSomInputSetKeyEnabled, ProxySomInputSetKeyEnabled);
    DetourDetach(&(PVOID&)ProxiedSomInputKeyboardPoll, ProxySomInputKeyboardPoll);
    DetourDetach(&(PVOID&)ProxiedSomInputKeyCheck, ProxySomInputKeyCheck);
    DetourDetach(&(PVOID&)ProxiedSomInputBufferBuild, ProxySomInputBufferBuild);
}

void __cdecl SomInputTick()
{

}   