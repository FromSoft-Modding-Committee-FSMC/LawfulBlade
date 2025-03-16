#include <string>

#include "somsound.h"
#include "logf.h"
#include "somconf.h"

#include "detours.h"
#include "fmod.h"
#include "fmod_errors.h"

// Local Globals
FMOD_SYSTEM*  l_fmodSystem;		// The main FMOD System

// Mixer
FMOD_CHANNELGROUP* l_fmodBgmChannelGroup;
FMOD_CHANNELGROUP* l_fmodSfxChannelGroup;

// Background Music
FMOD_SOUND*   l_fmodBgmSound;	// The Audio File Buffer for BGM
FMOD_CHANNEL* l_fmodBgmVoice;	// The currently playing BGM channel

char l_bgmRoot[] = "DATA/SOUND/BGM/";
char l_sndRoot[] = "DATA/SOUND/SE/";

// Capture N Wrap
SomSoundInit ProxiedSomSoundInit = (SomSoundInit)0x0043e470;
void __cdecl ProxySomSoundInit()
{
	if (GetGameConfigBool("SoundUseNewEngine") == TRUE)
	{
		// Our own sound initialization logic
		LogFWrite("Initializing FMOD...", "SomSound>SomSoundInit");

		// Use this to capture FMOD errors.
		FMOD_RESULT fmResult = FMOD_OK;

		// Create the FMOD System
		if (fmResult = FMOD_System_Create(&l_fmodSystem, FMOD_VERSION), fmResult != FMOD_OK || l_fmodSystem == NULL)
			goto FmodFail;

		// Initialize the FMOD System we just created
		if (fmResult = FMOD_System_Init(l_fmodSystem, 256, FMOD_INIT_THREAD_UNSAFE | FMOD_INIT_VOL0_BECOMES_VIRTUAL, NULL), fmResult != FMOD_OK)
			goto FmodFail;

		// Create our channels
		if (fmResult = FMOD_System_CreateChannelGroup(l_fmodSystem, "Music", &l_fmodBgmChannelGroup), fmResult != FMOD_OK || l_fmodBgmChannelGroup == NULL)
			goto FmodFail;

		if (fmResult = FMOD_System_CreateChannelGroup(l_fmodSystem, "Sound", &l_fmodSfxChannelGroup), fmResult != FMOD_OK || l_fmodSfxChannelGroup == NULL)
			goto FmodFail;

		FmodSuccess:
		LogFWrite("FMOD Init Success!", "SomSound>SomSoundInit");
		ProxiedSomSoundInit();	// REMOVE ME WHEN MATURE
		return;

		// When FMOD is bad dog - fallback to original sound engine.
		FmodFail:
		LogFWrite("FMOD Init Failed!", "SomSound>SomSoundInit");
		LogFWrite(FMOD_ErrorString(fmResult), "SomSound>SomSoundInit");

		// Clean up
		if (l_fmodBgmChannelGroup != NULL)
			FMOD_ChannelGroup_Release(l_fmodBgmChannelGroup);

		if (l_fmodSfxChannelGroup != NULL)
			FMOD_ChannelGroup_Release(l_fmodSfxChannelGroup);

		if (l_fmodSystem != NULL)
			FMOD_System_Release(l_fmodSystem);

		l_fmodSystem = NULL;
		l_fmodBgmChannelGroup = NULL;
		l_fmodSfxChannelGroup = NULL;			
	}

	// Original Sound Engine
	ProxiedSomSoundInit();
	return;
}

SomSoundBGMPlay ProxiedSomSoundBGMPlay = (SomSoundBGMPlay)0x0043e850;
uint32_t __cdecl ProxySomSoundBGMPlay(const char* filename, int32_t loopMode)
{
	LogFWrite("Playing BGM...", "SomSound>SomSoundBGMPlay");

	if (l_fmodSystem != NULL)
	{
		// Get the full path of the BGM file...
		std::string filepath(l_bgmRoot);
		filepath.append(filename);

		// Has the developer requested a different extension for BGM?
		std::string fileExt = GetGameConfigString("SoundOverrideBGMExtension");
		if (!fileExt.empty())
			filepath = filepath.substr(0, filepath.find_last_of('.')) + fileExt;

		// Figure out what flags we need...
		FMOD_MODE bgmPlayMode = FMOD_2D | FMOD_CREATESTREAM;				// Background music should always be 2D, and a stream.
		bgmPlayMode |= FMOD_IGNORETAGS | FMOD_LOWMEM;						// We don't need tags or most of this functionality for a game lol.
		bgmPlayMode |= (loopMode == 1 ? FMOD_LOOP_NORMAL : FMOD_LOOP_OFF);	// Do we want looping?

		// Actually try to create the sound.
		FMOD_RESULT result = FMOD_System_CreateSound(l_fmodSystem, filepath.c_str(), bgmPlayMode, NULL, &l_fmodBgmSound);
		if (result != FMOD_OK)
		{
			LogFWrite("Failed to create sound!!! ", "SomSoundBGMPlay");
			LogFWrite(filepath, "SomSoundBGMPlay");
			LogFWrite(FMOD_ErrorString(result), "SomSoundBGMPlay");

			l_fmodBgmSound = NULL;
		}

		// Attempt to play the sound...
		if (l_fmodBgmSound != NULL)
		{
			result = FMOD_System_PlaySound(l_fmodSystem, l_fmodBgmSound, l_fmodBgmChannelGroup, FALSE, &l_fmodBgmVoice);
			if (result != FMOD_OK)
			{
				LogFWrite("Faield to play sound!", "SomSoundBGMPlay");
				LogFWrite(FMOD_ErrorString(result), "SomSoundBGMPlay");

				l_fmodBgmVoice = NULL;
			}
		}
	}

	// Run the original function...
	return ProxiedSomSoundBGMPlay(filename, loopMode);
}

SomSoundBGMStop ProxiedSomSoundBGMStop = (SomSoundBGMStop)0x0043ea50;
uint32_t __cdecl ProxySomSoundBGMStop()
{
	LogFWrite("Stopping BGM...", "SomSound>SomSoundBGMStop");

	// New Engine
	if (l_fmodSystem != NULL)
	{
		FMOD_RESULT fmResult = FMOD_OK;

		// Stop the channel
		if (l_fmodBgmVoice != NULL)	// If these are already null, we don't want to try and stop or release them.
		{
			if (fmResult = FMOD_Channel_Stop(l_fmodBgmVoice), fmResult != FMOD_OK)
				goto FmodFail;

			l_fmodBgmVoice = NULL;
		}

		// Free the sound
		if (l_fmodBgmSound != NULL)
		{
			if (fmResult = FMOD_Sound_Release(l_fmodBgmSound), fmResult != FMOD_OK)
				goto FmodFail;

			l_fmodBgmSound = NULL;
		}

		FmodSuccess:
		return ProxiedSomSoundBGMStop();	// Remove me when mature!

		FmodFail:
		LogFWrite("Failed to stop BGM!", "SomSound>SomSoundBGMStop");
		LogFWrite(FMOD_ErrorString(fmResult), "SomSound>SomSoundInit");
		// return 0;	// Add me when mature!
	}
	
	return ProxiedSomSoundBGMStop();
}

// Hook N Fuck
void __cdecl SomSoundInitDetours()
{
	DetourAttach(&(PVOID&)ProxiedSomSoundInit, ProxySomSoundInit);
	DetourAttach(&(PVOID&)ProxiedSomSoundBGMPlay, ProxySomSoundBGMPlay);
	DetourAttach(&(PVOID&)ProxiedSomSoundBGMStop, ProxySomSoundBGMStop);
}

void __cdecl SomSoundKillDetours()
{
	DetourDetach(&(PVOID&)ProxiedSomSoundInit, ProxySomSoundInit);
	DetourDetach(&(PVOID&)ProxiedSomSoundBGMPlay, ProxySomSoundBGMPlay);
	DetourDetach(&(PVOID&)ProxiedSomSoundBGMStop, ProxySomSoundBGMStop);
}