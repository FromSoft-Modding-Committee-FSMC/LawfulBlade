#include "sominput.h"
#include "logf.h"

// Lookup table for DInput Key Names
const char* DINPUT_KEY_NAMES[256] =
{
    "INVALID DIK CODE", // 00
    "ESCAPE", // 01
    "1", // 02
    "2", // 03
    "3", // 04
    "4", // 05
    "5", // 06
    "6", // 07
    "7", // 08
    "8", // 09
    "9", // 0A
    "0", // 0B
    "MINUS", // 0C
    "EQUALS", // 0D
    "BACK", // 0E
    "TAB", // 0F

    "Q", // 10
    "W", // 11
    "E", // 12
    "R", // 13
    "T", // 14
    "Y", // 15
    "U", // 16
    "I", // 17
    "O", // 18
    "P", // 19
    "LBRACKET", // 1A
    "RBRACKET", // 1B
    "RETURN", // 1C
    "LCONTROL", // 1D
    "A", // 1E
    "S", // 1F

    "D", // 20
    "F", // 21
    "G", // 22
    "H", // 23
    "J", // 24
    "K", // 25
    "L", // 26
    "SEMICOLON", // 27
    "APOSTROPHE", // 28
    "GRAVE", // 29
    "LSHIFT", // 2A
    "BACKSLASH", // 2B
    "Z", // 2C
    "X", // 2D
    "C", // 2E
    "V", // 2F

    "B", // 30
    "N", // 31
    "M", // 32
    "COMMA", // 33
    "PERIOD", // 34
    "SLASH", // 35
    "RSHIFT", // 36
    "MULTIPLY", // 37
    "LMENU", // 38
    "SPACE", // 39
    "CAPITAL", // 3A
    "F1", // 3B
    "F2", // 3C
    "F3", // 3D
    "F4", // 3E
    "F5", // 3F

    "F6", // 40
    "F7", // 41
    "F8", // 42
    "F9", // 43
    "F10", // 44
    "NUMLOCK", // 45
    "SCROLL", // 46
    "NUMPAD7", // 47
    "NUMPAD8", // 48
    "NUMPAD9", // 49
    "SUBTRACT", // 4A
    "NUMPAD4", // 4B
    "NUMPAD5", // 4C
    "NUMPAD6", // 4D
    "ADD", // 4E
    "NUMPAD1", // 4F

    "NUMPAD2", // 50
    "NUMPAD3", // 51
    "NUMPAD0", // 52
    "DECIMAL", // 53
    "INVALID DIK CODE", // 54
    "INVALID DIK CODE", // 55
    "OEM_102", // 56
    "F11", // 57
    "F12", // 58
    "INVALID DIK CODE", // 59
    "INVALID DIK CODE", // 5A
    "INVALID DIK CODE", // 5B
    "INVALID DIK CODE", // 5C
    "INVALID DIK CODE", // 5D
    "INVALID DIK CODE", // 5E
    "INVALID DIK CODE", // 5F

    "INVALID DIK CODE", // 60
    "INVALID DIK CODE", // 61
    "INVALID DIK CODE", // 62
    "INVALID DIK CODE", // 63
    "F13", // 64
    "F14", // 65
    "F15", // 66
    "INVALID DIK CODE", // 67
    "INVALID DIK CODE", // 68
    "INVALID DIK CODE", // 69
    "INVALID DIK CODE", // 6A
    "INVALID DIK CODE", // 6B
    "INVALID DIK CODE", // 6C
    "INVALID DIK CODE", // 6D
    "INVALID DIK CODE", // 6E
    "INVALID DIK CODE", // 6F

    "KANA", // 70
    "INVALID DIK CODE", // 71
    "INVALID DIK CODE", // 72
    "ABNT_C1", // 73
    "INVALID DIK CODE", // 74
    "INVALID DIK CODE", // 75
    "INVALID DIK CODE", // 76
    "INVALID DIK CODE", // 77
    "INVALID DIK CODE", // 78
    "CONVERT", // 79
    "INVALID DIK CODE", // 7A
    "NOCONVERT", // 7B
    "INVALID DIK CODE", // 7C
    "YEN", // 7D
    "ABNT_C2", // 7E
    "INVALID DIK CODE", // 7F

    "INVALID DIK CODE", // 80
    "INVALID DIK CODE", // 81
    "INVALID DIK CODE", // 82
    "INVALID DIK CODE", // 83
    "INVALID DIK CODE", // 84
    "INVALID DIK CODE", // 85
    "INVALID DIK CODE", // 86
    "INVALID DIK CODE", // 87
    "INVALID DIK CODE", // 88
    "INVALID DIK CODE", // 89
    "INVALID DIK CODE", // 8A
    "INVALID DIK CODE", // 8B
    "INVALID DIK CODE", // 8C
    "NUMPADEQUALS", // 8D
    "INVALID DIK CODE", // 8E
    "INVALID DIK CODE", // 8F

    "CIRCUMFLEX", // 90
    "AT", // 91
    "COLON", // 92
    "UNDERLINE", // 93
    "KANJI", // 94
    "STOP", // 95
    "AX", // 96
    "UNLABELED", // 97
    "INVALID DIK CODE", // 98
    "NEXTTRACK", // 99
    "INVALID DIK CODE", // 9A
    "INVALID DIK CODE", // 9B
    "NUMPADENTER", // 9C
    "RCONTROL", // 9D
    "INVALID DIK CODE", // 9E
    "INVALID DIK CODE", // 9F

    "MUTE", // A0
    "CALCULATOR", // A1
    "PLAYPAUSE", // A2
    "INVALID DIK CODE", // A3
    "MEDIASTOP", // A4
    "INVALID DIK CODE", // A5
    "INVALID DIK CODE", // A6
    "INVALID DIK CODE", // A7
    "INVALID DIK CODE", // A8
    "INVALID DIK CODE", // A9
    "INVALID DIK CODE", // AA
    "INVALID DIK CODE", // AB
    "INVALID DIK CODE", // AC
    "INVALID DIK CODE", // AD
    "VOLUMEDOWN", // AE
    "INVALID DIK CODE", // AF

    "VOLUMEUP", // B0
    "INVALID DIK CODE", // B1
    "WEBHOME", // B2
    "NUMPADCOMMA", // B3
    "INVALID DIK CODE", // B4
    "DIVIDE", // B5
    "INVALID DIK CODE", // B6
    "SYSRQ", // B7
    "RMENU", // B8
    "INVALID DIK CODE", // B9
    "INVALID DIK CODE", // BA
    "INVALID DIK CODE", // BB
    "INVALID DIK CODE", // BC
    "INVALID DIK CODE", // BD
    "INVALID DIK CODE", // BE
    "INVALID DIK CODE", // BF

    "INVALID DIK CODE", // C0
    "INVALID DIK CODE", // C1
    "INVALID DIK CODE", // C2
    "INVALID DIK CODE", // C3
    "INVALID DIK CODE", // C4
    "PAUSE", // C5
    "INVALID DIK CODE", // C6
    "HOME", // C7
    "UP", // C8
    "PRIOR", // C9
    "INVALID DIK CODE", // CA
    "LEFT", // CB
    "INVALID DIK CODE", // CC
    "RIGHT", // CD
    "INVALID DIK CODE", // CE
    "END", // CF

    "DOWN", // D0
    "NEXT", // D1
    "INSERT", // D2
    "DELETE", // D3
    "INVALID DIK CODE", // D4
    "INVALID DIK CODE", // D5
    "INVALID DIK CODE", // D6
    "INVALID DIK CODE", // D7
    "INVALID DIK CODE", // D8
    "INVALID DIK CODE", // D9
    "INVALID DIK CODE", // DA
    "LWIN", // DB
    "RWIN", // DC
    "APPS", // DD
    "POWER", // DE
    "SLEEP", // DF

    "INVALID DIK CODE", // E0
    "INVALID DIK CODE", // E1
    "INVALID DIK CODE", // E2
    "WAKE", // E3
    "INVALID DIK CODE", // E4
    "WEBSEARCH", // E5
    "WEBFAVORITES", // E6
    "WEBREFRESH", // E7
    "WEBSTOP", // E8
    "WEBFORWARD", // E9
    "WEBBACK", // EA
    "MYCOMPUTER", // EB
    "MAIL", // EC
    "MEDIASELECT", // ED
    "INVALID DIK CODE", // EE
    "INVALID DIK CODE", // EF

    "INVALID DIK CODE", // F0
    "INVALID DIK CODE", // F1
    "INVALID DIK CODE", // F2
    "INVALID DIK CODE", // F3
    "INVALID DIK CODE", // F4
    "INVALID DIK CODE", // F5
    "INVALID DIK CODE", // F6
    "INVALID DIK CODE", // F7
    "INVALID DIK CODE", // F8
    "INVALID DIK CODE", // F9
    "INVALID DIK CODE", // FA
    "INVALID DIK CODE", // FB
    "INVALID DIK CODE", // FC
    "INVALID DIK CODE", // FD
    "INVALID DIK CODE", // FE
    "INVALID DIK CODE"  // FF
};

// Function Definition
SomInputInit ProxiedSomInputInit = (SomInputInit)0x0040e590;
SomInputSetKeyEnabled ProxiedSomInputSetKeyEnabled = (SomInputSetKeyEnabled)0x004499e0;
SomInputKeyboardPoll ProxiedSomInputKeyboardPoll = (SomInputKeyboardPoll)0x00449940;

// Proxies
bool __cdecl ProxySomInputInit()
{
    LogFWrite("Initialized Input System...", "ProxySomInputInit");

    return ProxiedSomInputInit();
}

bool __cdecl ProxySomInputSetKeyEnabled(unsigned char keyID, bool isEnabled)
{
    std::ostringstream out;
    out << "{ Key = " << DINPUT_KEY_NAMES[keyID] << ", isEnabled = " << isEnabled << "} ";
    LogFWrite(out.str(), "ProxySomSetKeyEnabled");

    return ProxiedSomInputSetKeyEnabled(keyID, isEnabled);
}

/// <summary>
/// Pushes the current keyboard state to last, and aquires a new current state.
/// </summary>
/// <returns></returns>
bool __cdecl ProxySomInputKeyboardPoll()
{
    // LogFWrite("Polling Keyboard Device...", "ProxySomInputKeyboardPoll");

    return ProxiedSomInputKeyboardPoll();
}