#ifndef _SOMSOUND_H
#define _SOMSOUND_H

#include <stdint.h>

// Globals

// Proxies
typedef void(__cdecl* SomSoundInit)();
extern SomSoundInit ProxiedSomSoundInit;
extern void __cdecl ProxySomSoundInit();

typedef uint32_t(__cdecl* SomSoundBGMPlay)(const char*, int32_t);
extern SomSoundBGMPlay ProxiedSomSoundBGMPlay;
extern uint32_t __cdecl ProxySomSoundBGMPlay(const char* filename, int32_t loopMode);

typedef uint32_t(__cdecl* SomSoundBGMStop)();
extern SomSoundBGMStop ProxiedSomSoundBGMStop;
extern uint32_t __cdecl ProxySomSoundBGMStop();

// Detours
extern void __cdecl SomSoundInitDetours();
extern void __cdecl SomSoundKillDetours();

#endif