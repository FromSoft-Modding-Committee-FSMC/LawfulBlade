#ifndef _SOMSOUND_H
#define _SOMSOUND_H

#include <stdint.h>

#include "sdk/fmod/inc/fmod.h"
#include "sdk/fmod/inc/fmod_errors.h"

// Structures
typedef struct 
{
	bool dontUnload;		// if the sound should be unloaded automatically
	int16_t soundId;		// the ID of the sound
	int16_t refCount;		// the number of references to the sound
	uint8_t* sampleBuffer;	// the buffer of samples
	FMOD_SOUND* fmodSound;	// the sound for our FMOD buffer...
} SomSoundSource;

// Init, DeInit
typedef void(__cdecl* SomSoundInit)();
extern SomSoundInit ProxiedSomSoundInit;
extern void __cdecl ProxySomSoundInit();

// Background Music
typedef uint32_t(__cdecl* SomSoundBGMPlay)(const char*, int32_t);
extern SomSoundBGMPlay ProxiedSomSoundBGMPlay;
extern uint32_t __cdecl ProxySomSoundBGMPlay(const char* filename, int32_t loopMode);

typedef uint32_t(__cdecl* SomSoundBGMStop)();
extern SomSoundBGMStop ProxiedSomSoundBGMStop;
extern uint32_t __cdecl ProxySomSoundBGMStop();

// Sound Effects
typedef bool(__cdecl* SomSoundLoad)(int16_t, int8_t);
extern SomSoundLoad ProxiedSomSoundLoad;
extern bool __cdecl ProxySomSoundLoad(int16_t soundId, int8_t dontUnload);

typedef uint32_t(__cdecl* SomSoundUnload)(int16_t);
extern SomSoundUnload ProxiedSomSoundUnload;
extern uint32_t __cdecl ProxySomSoundUnload(int16_t soundId);

typedef uint32_t(__cdecl* SomSoundUnloadRef)(int16_t);
extern SomSoundUnloadRef ProxiedSomSoundUnloadRef;
extern uint32_t __cdecl ProxySomSoundUnloadRef(int16_t soundId);

typedef bool(__cdecl* SomSoundPlay3D)(int32_t, int8_t, float, float, float);
extern SomSoundPlay3D ProxiedSomSoundPlay3D;
extern bool __cdecl ProxySomSoundPlay3D(int32_t soundId, int8_t pitch, float param_3, float param_4, float param_5);

typedef bool(__cdecl* SomSoundPlay2D)(int32_t, int8_t);
extern SomSoundPlay2D ProxiedSomSoundPlay2D;
extern bool __cdecl ProxySomSoundPlay2D(int32_t soundId, int8_t pitch);

// Detours
extern void __cdecl SomSoundInitDetours();
extern void __cdecl SomSoundKillDetours();
extern void __cdecl SomSoundTick();
extern void __cdecl UnsealSoundOnSuspend();
extern void __cdecl UnsealSoundOnResume();

#endif