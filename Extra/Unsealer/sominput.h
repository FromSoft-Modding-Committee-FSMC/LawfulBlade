#ifndef _SOMINPUT_H
#define _SOMINPUT_H

// Stolen Memory


// Function Types
typedef bool(__cdecl* SomInputInit)();
typedef bool(__cdecl* SomInputSetKeyEnabled)(unsigned char keyID, bool isEnabled);
typedef bool(__cdecl* SomInputKeyboardPoll)();

// Original Functions
extern SomInputInit ProxiedSomInputInit;
extern SomInputSetKeyEnabled ProxiedSomInputSetKeyEnabled;
extern SomInputKeyboardPoll ProxiedSomInputKeyboardPoll;

// Proxy Functions
extern bool __cdecl ProxySomInputInit();
extern bool __cdecl ProxySomInputSetKeyEnabled(unsigned char keyID, bool isEnabled);
extern bool __cdecl ProxySomInputKeyboardPoll();

#endif