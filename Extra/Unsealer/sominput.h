#ifndef _SOMINPUT_H
#define _SOMINPUT_H

// Created Memory
extern uint32_t& m_somInputState;
extern bool g_somCurrKeyState[];
extern bool g_somLastKeyState[];
extern bool g_somCurrMouseState[];
extern bool g_somLastMouseState[];
extern int32_t g_somMouseAccumulatorX;
extern int32_t g_somMouseAccumulatorY;

// Unseal Stuff
extern void UnsealProcessRawInput(HRAWINPUT rawInput);

// Function Types
typedef bool(__cdecl* SomInputInit)();
typedef bool(__cdecl* SomInputSetKeyEnabled)(unsigned char, bool);
typedef bool(__cdecl* SomInputKeyboardPoll)();
typedef bool(__cdecl* SomInputKeyCheck)(unsigned char, bool);
typedef void(__cdecl* SomInputBufferBuild)();

// Original Functions
extern SomInputInit ProxiedSomInputInit;
extern SomInputSetKeyEnabled ProxiedSomInputSetKeyEnabled;
extern SomInputKeyboardPoll ProxiedSomInputKeyboardPoll;
extern SomInputKeyCheck ProxiedSomInputKeyCheck;
extern SomInputBufferBuild ProxiedSomInputBufferBuild;

// Proxy Functions
extern bool __cdecl ProxySomInputInit();
extern bool __cdecl ProxySomInputSetKeyEnabled(unsigned char keyID, bool isEnabled);
extern bool __cdecl ProxySomInputKeyboardPoll();
extern bool __cdecl ProxySomInputKeyCheck(unsigned char keyID, bool param_2);
extern void __cdecl ProxySomInputBufferBuild();

// Detours
extern void __cdecl SomInputInitDetours();
extern void __cdecl SomInputKillDetours();
extern void __cdecl SomInputTick();

#endif