#ifndef _UNSEALSOUND_H
#define _UNSEALSOUND_H

#include <stdint.h>

#include "fmod.h"
#include "fmod_errors.h"

// Structures
typedef struct 
{
	bool dontUnload;		// if the sound should be unloaded automatically
	int16_t soundId;		// the ID of the sound
	int16_t refCount;		// the number of references to the sound
	uint8_t* sampleBuffer;	// the buffer of samples
	FMOD_SOUND* fmodSound;	// the sound for our FMOD buffer...
} SomSoundSource;

// Detours
extern void __cdecl UnsealSoundInit();
extern void __cdecl UnsealSoundKill();
extern void __cdecl UnsealSoundTick();
extern void __cdecl UnsealSoundOnSuspend();
extern void __cdecl UnsealSoundOnResume();

#endif