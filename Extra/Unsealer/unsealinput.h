#ifndef _UNSEALINPUT_H
#define _UNSEALINPUT_H

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

// Proxy Functions
extern bool __cdecl ProxySomInputInit();
extern bool __cdecl ProxySomInputSetKeyEnabled(unsigned char keyID, bool isEnabled);
extern bool __cdecl ProxySomInputKeyboardPoll();
extern bool __cdecl ProxySomInputKeyCheck(unsigned char keyID, bool param_2);
extern void __cdecl ProxySomInputBufferBuild();

// Detours
extern void __cdecl UnsealInputInit();
extern void __cdecl UnsealInputKill();

#endif