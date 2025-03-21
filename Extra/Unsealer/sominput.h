#ifndef _SOMINPUT_H
#define _SOMINPUT_H

// Stolen Memory

// Created Memory
extern bool g_somCurrKeyState[];
extern bool g_somLastKeyState[];
extern bool g_somCurrMouseState[];
extern bool g_somLastMouseState[];
extern float* g_somCameraX;
extern float* g_somCameraY;

extern float* g_somPlayerX;
extern float* g_somPlayerY;
extern float* g_somPlayerZ;

// Function Types
typedef bool(__cdecl* SomInputInit)();
typedef bool(__cdecl* SomInputSetKeyEnabled)(unsigned char, bool);
typedef bool(__cdecl* SomInputKeyboardPoll)();
typedef bool(__cdecl* SomInputKeyCheck)(unsigned char, bool);

// Original Functions
extern SomInputInit ProxiedSomInputInit;
extern SomInputSetKeyEnabled ProxiedSomInputSetKeyEnabled;
extern SomInputKeyboardPoll ProxiedSomInputKeyboardPoll;
extern SomInputKeyCheck ProxiedSomInputKeyCheck;

// Proxy Functions
extern bool __cdecl ProxySomInputInit();
extern bool __cdecl ProxySomInputSetKeyEnabled(unsigned char keyID, bool isEnabled);
extern bool __cdecl ProxySomInputKeyboardPoll();
extern bool __cdecl ProxySomInputKeyCheck(unsigned char keyID, bool param_2);

extern bool GetRemappedKeyHeld(const char* keyConfName);
extern bool GetRemappedKeyPressed(const char* keyConfName);

#endif