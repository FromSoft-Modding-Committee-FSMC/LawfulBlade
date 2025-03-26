#include <string>
#include <map>
#include <math.h>

#include "somsound.h"
#include "logf.h"
#include "somconf.h"
#include "sominput.h"
#include "somgamedata.h"

#include "detours.h"
#include "fmod_errors.h"

// Local Globals
char l_bgmRoot[] = "DATA/SOUND/BGM/";
char l_sndRoot[] = "DATA/SOUND/SE/";

FMOD_SYSTEM*  l_fmodSystem;		// The main FMOD System

// Mixer
FMOD_CHANNELGROUP* l_fmodBgmChannelGroup;
FMOD_CHANNELGROUP* l_fmodSfxChannelGroup;

// DSP
FMOD_DSP* l_fmodDspReverb;

// Background Music
FMOD_SOUND*   l_fmodBgmSound;	// The Audio File Buffer for BGM
FMOD_CHANNEL* l_fmodBgmVoice;	// The currently playing BGM channel

// Listener
FMOD_VECTOR l_fmodListenerPosition;
FMOD_VECTOR l_fmodListenerVelocity;
FMOD_VECTOR l_fmodListenerUp;
FMOD_VECTOR l_fmodListenerForward;

// Use these for foot steps
float l_lastPlayerX = 0;
float l_lastPlayerZ = 0;

// Sound Effects
std::map<int16_t, SomSoundSource> l_somSounds;

// Capture N Wrap - Init, Deinit
SomSoundInit ProxiedSomSoundInit = (SomSoundInit)0x0043e470;
void __cdecl ProxySomSoundInit()
{
	if (GetGameConfigBool("SoundUseNewEngine") == TRUE)
	{
		// Our own sound initialization logic
		LogFWrite("Initializing FMOD...", "SomSound>SomSoundInit");

		// Fucking goto man...
		FMOD_REVERB_PROPERTIES fmReverbProperties = FMOD_PRESET_GENERIC;

		// Use this to capture FMOD errors.
		FMOD_RESULT fmResult = FMOD_OK;

		// Create the FMOD System
		if (fmResult = FMOD_System_Create(&l_fmodSystem, FMOD_VERSION), fmResult != FMOD_OK || l_fmodSystem == NULL)
			goto FmodFail;

		// Initialize the FMOD System we just created
		if (fmResult = FMOD_System_Init(l_fmodSystem, 64, FMOD_INIT_THREAD_UNSAFE | FMOD_INIT_VOL0_BECOMES_VIRTUAL, NULL), fmResult != FMOD_OK)
			goto FmodFail;
		
		// We only want 1 listener
		if (fmResult = FMOD_System_Set3DNumListeners(l_fmodSystem, 1), fmResult != FMOD_OK)
			goto FmodFail;

		// Our reverb properties...
		if (fmResult = FMOD_System_SetReverbProperties(l_fmodSystem, 0, &fmReverbProperties), fmResult != FMOD_OK)
			goto FmodFail;

		// Create a DSP for reverb SFX and configure it...
		if (fmResult = FMOD_System_CreateDSPByType(l_fmodSystem, FMOD_DSP_TYPE_SFXREVERB, &l_fmodDspReverb), fmResult != FMOD_OK || l_fmodDspReverb == NULL)
			goto FmodFail;

		// Music Channel
		if (fmResult = FMOD_System_CreateChannelGroup(l_fmodSystem, "Music", &l_fmodBgmChannelGroup), fmResult != FMOD_OK || l_fmodBgmChannelGroup == NULL)
			goto FmodFail;

		// Set volume of music channel to 0.5 by default (TO-DO: use the SoM volumes...)
		if (fmResult = FMOD_ChannelGroup_SetVolume(l_fmodBgmChannelGroup, 0.5f), fmResult != FMOD_OK)
			goto FmodFail;

		// create Sound Channel
		if (fmResult = FMOD_System_CreateChannelGroup(l_fmodSystem, "Sound", &l_fmodSfxChannelGroup), fmResult != FMOD_OK || l_fmodSfxChannelGroup == NULL)
			goto FmodFail;

		// Add the reverb dsp to the sound channel
		if (fmResult = FMOD_ChannelGroup_AddDSP(l_fmodSfxChannelGroup, 0, l_fmodDspReverb), fmResult != FMOD_OK)
			goto FmodFail;

		// Set reverb properties for the sound channel
		if (fmResult = FMOD_ChannelGroup_SetReverbProperties(l_fmodSfxChannelGroup, 0, 0.8f), fmResult != FMOD_OK)
			goto FmodFail;

		// FMOD Success
		LogFWrite("FMOD Init Success!", "SomSound>SomSoundInit");

		// Load User Choice Sounds
		for (int i = 0; i < 16; ++i)
			ProxySomSoundLoad(g_somSysDatUserSounds[i], TRUE);

		// Load Menu Sounds
		ProxySomSoundLoad(1004 + (4 * *g_somSysDatMenuSounds) + 0, TRUE);
		ProxySomSoundLoad(1004 + (4 * *g_somSysDatMenuSounds) + 1, TRUE);
		ProxySomSoundLoad(1004 + (4 * *g_somSysDatMenuSounds) + 2, TRUE);
		ProxySomSoundLoad(1004 + (4 * *g_somSysDatMenuSounds) + 3, TRUE);

		// Load Footstep Sounds
		if (GetGameConfigInteger("SoundFootStepID") >= 0)
			ProxySomSoundLoad(GetGameConfigInteger("SoundFootStepID"), TRUE);

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

	return;
}


// Capture N Wrap - BGM
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

		// Store results here...
		FMOD_RESULT fmResult = FMOD_OK;

		// Figure out what flags we need...
		FMOD_MODE bgmPlayMode = FMOD_2D | FMOD_CREATESTREAM;				// Background music should always be 2D, and a stream.
		bgmPlayMode |= FMOD_IGNORETAGS | FMOD_LOWMEM;						// We don't need tags or most of this functionality for a game lol.
		bgmPlayMode |= (loopMode == 1 ? FMOD_LOOP_NORMAL : FMOD_LOOP_OFF);	// Do we want looping?

		if (fmResult = FMOD_System_CreateSound(l_fmodSystem, filepath.c_str(), bgmPlayMode, NULL, &l_fmodBgmSound), fmResult != FMOD_OK || l_fmodBgmSound == NULL)
			goto FmodFail;

		if (fmResult = FMOD_System_PlaySound(l_fmodSystem, l_fmodBgmSound, l_fmodBgmChannelGroup, FALSE, &l_fmodBgmVoice), fmResult != FMOD_OK || l_fmodBgmVoice == NULL)
			goto FmodFail;

		// Success
		return NULL;

		FmodFail:
		LogFWrite("Failed to play BGM!", "SomSound>SomSoundBGMPlay");
		LogFWrite(FMOD_ErrorString(fmResult), "SomSound>SomSoundBGMPlay");
	}

	return NULL; 
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

		// Success
		return 0;

		FmodFail:
		LogFWrite("Failed to stop BGM!", "SomSound>SomSoundBGMStop");
		LogFWrite(FMOD_ErrorString(fmResult), "SomSound>SomSoundBGMStop");
		return 0;
	}

	// Run the original function when not using FMOD
	return NULL; //ProxiedSomSoundBGMStop();
}


// Capture N Wrap - SFX
SomSoundLoad ProxiedSomSoundLoad = (SomSoundLoad)0x0043e5a0;
bool __cdecl ProxySomSoundLoad(int16_t soundId, int8_t dontUnload)
{
	// Don't bother processing invalid indices.
	if (soundId < 0 || soundId >= 2048)
		return false;

	// If this sound doesn't exist in our map, we need to load it...
	if (!l_somSounds.count(soundId))
	{
		memset(&l_somSounds[soundId], 0, sizeof(SomSoundSource));
		l_somSounds[soundId].dontUnload   = (dontUnload == 1);
		l_somSounds[soundId].refCount     = 1;
		l_somSounds[soundId].soundId      = soundId;

		// Attempt to load the sound...
		char filepath[MAX_PATH];
		sprintf_s(filepath, "%s%.4d.snd\0", l_sndRoot, soundId);

		// Open the SND file and read the header...
		std::ifstream sndFile(filepath, std::ios::binary);

		uint16_t sndUnkx00;				// TO-DO: Turn this into a struct and a single read
		uint16_t sndUnkx02;
		uint32_t sndSampleRate1;
		uint32_t sndSampleRate2;
		uint16_t sndBytesPerSample;
		uint16_t sndBitsPerSample;
		uint16_t sndUnkx10;
		uint32_t sndByteSize;
		uint16_t sndUnkx14;

		sndFile.read((char*)&sndUnkx00, sizeof(sndUnkx00));
		sndFile.read((char*)&sndUnkx02, sizeof(sndUnkx02));
		sndFile.read((char*)&sndSampleRate1, sizeof(sndSampleRate1));
		sndFile.read((char*)&sndSampleRate2, sizeof(sndSampleRate2));
		sndFile.read((char*)&sndBytesPerSample, sizeof(sndBytesPerSample));
		sndFile.read((char*)&sndBitsPerSample, sizeof(sndBitsPerSample));
		sndFile.read((char*)&sndUnkx10, sizeof(sndUnkx10));
		sndFile.read((char*)&sndByteSize, sizeof(sndByteSize));
		sndFile.read((char*)&sndUnkx14, sizeof(sndUnkx14));

		// Read the buffer from the SND...
		l_somSounds[soundId].sampleBuffer = new uint8_t[sndByteSize];
		sndFile.read((char*)l_somSounds[soundId].sampleBuffer, sndByteSize);

		// Create the sound using FMOD
		FMOD_RESULT fmResult = FMOD_OK;

		// Figure out what flags we need...
		FMOD_CREATESOUNDEXINFO exInfo;
		memset(&exInfo, 0, sizeof(FMOD_CREATESOUNDEXINFO));
		exInfo.cbsize			= sizeof(FMOD_CREATESOUNDEXINFO);	
		exInfo.length           = sndByteSize;
		exInfo.numchannels	    = 1;
		exInfo.defaultfrequency = sndSampleRate1;
		exInfo.format			= FMOD_SOUND_FORMAT_PCM16;

		if (fmResult = FMOD_System_CreateSound(l_fmodSystem, (char*)l_somSounds[soundId].sampleBuffer, FMOD_DEFAULT | FMOD_3D | FMOD_CREATESAMPLE | FMOD_OPENRAW | FMOD_OPENMEMORY_POINT, &exInfo, &l_somSounds[soundId].fmodSound), fmResult != FMOD_OK || l_somSounds[soundId].fmodSound == NULL)
			goto FmodFail;

		return NULL;

		// Create the sound using FMOD...
		FmodFail:
		LogFWrite("Failed to load sound!", "SomSound>SomSoundLoad");
		LogFWrite(FMOD_ErrorString(fmResult), "SomSound>SomSoundLoad");
	}
	else
	{
		// otherwise, just increase the reference count
		l_somSounds[soundId].refCount++;
	}

	return NULL;
}

SomSoundUnload ProxiedSomSoundUnload = (SomSoundUnload)0x0044a4f0;
uint32_t __cdecl ProxySomSoundUnload(int16_t soundId) 
{
	// Don't bother processing invalid indices.
	if (soundId < 0 || soundId >= 2048)
		return NULL;

	// If the sound isn't loaded, exit early.
	if (!l_somSounds.count(soundId))
		return NULL;

	// Clear out the sound
	l_somSounds[soundId].dontUnload = false;
	l_somSounds[soundId].refCount   = 0;
	l_somSounds[soundId].soundId    = -1;

	return NULL;
}

SomSoundUnloadRef ProxiedSomSoundUnloadRef = (SomSoundUnloadRef)0x0043e670;
uint32_t __cdecl ProxySomSoundUnloadRef(int16_t soundId)
{
	// Don't bother processing invalid indices.
	if (soundId < 0 || soundId >= 2048)
		return NULL;

	// If the sound isn't loaded, exit early.
	if (!l_somSounds.count(soundId))
		return NULL;

	// If the sound is flagged don't unload, exit early.
	if (l_somSounds[soundId].dontUnload)
		return NULL;

	// Finally, do unload logic...
	l_somSounds[soundId].refCount--;

	// If there are no more references, unload the sound...
	if (l_somSounds[soundId].refCount == 0)
		ProxySomSoundUnload(soundId);

	return NULL;
}

SomSoundPlay3D ProxiedSomSoundPlay3D = (SomSoundPlay3D)0x0043e730;
bool __cdecl ProxySomSoundPlay3D(int32_t soundId, int8_t pitch, float x, float y, float z)
{
	// Exit early if the sound ID is outside this range...
	if (soundId < 0 || soundId >= 2048)
		return false;

	// Does this sound even exist?
	if (!l_somSounds.count(soundId))
		return false;

	// If the sound is null, something has gone wrong.
	if (l_somSounds[soundId].fmodSound == NULL)
		return false;

	// Try to play the sound
	FMOD_RESULT fmResult = FMOD_OK;
	FMOD_CHANNEL* voice;

	// Configure Sound For 3D
	FMOD_MODE fmMode;
	FMOD_Sound_GetMode(l_somSounds[soundId].fmodSound, &fmMode);

	// Disable 2D Flag (if set), and set 3D flag.
	fmMode &= ~FMOD_2D;
	fmMode |=  FMOD_3D;

	FMOD_Sound_SetMode(l_somSounds[soundId].fmodSound, fmMode);

	if (fmResult = FMOD_System_PlaySound(l_fmodSystem, l_somSounds[soundId].fmodSound, l_fmodSfxChannelGroup, FALSE, &voice), fmResult != FMOD_OK)
		return false;
	
	// Convert SoM pitch to FMOD Pitch.
	// SoM gives a range of -2 to +2 octaves to playback sound, or -24 to 24. 0 is regular pitch... -24 needs to be mapped to 0.25, +24 needs to be mapped to 3.00 ?
	if (pitch != 0 && pitch >= -24 && pitch <= 24)
		FMOD_Channel_SetPitch(voice, (pitch - -24.0f) * (3.00f - 0.25f) / (24.0f - -24.0f) + 0.25f);

	FMOD_VECTOR position;
	position.x = x;
	position.y = y;
	position.z = z;

	FMOD_VECTOR velocity;
	velocity.x = 0.f;
	velocity.y = 0.f;
	velocity.z = 0.f;

	FMOD_Channel_Set3DAttributes(voice, &position, &velocity);

	return true;
}

SomSoundPlay2D ProxiedSomSoundPlay2D = (SomSoundPlay2D)0x0043e6c0;
bool __cdecl ProxySomSoundPlay2D(int32_t soundId, int8_t pitch)
{
	// Exit early if the sound ID is outside this range...
	if (soundId < 0 || soundId >= 2048)
		return false;	

	// Does this sound even exist?
	if (!l_somSounds.count(soundId))
		return false;

	// If the sound is null, something has gone wrong.
	if (l_somSounds[soundId].fmodSound == NULL)
		return false;

	// Try to play the sound
	FMOD_RESULT fmResult = FMOD_OK;
	FMOD_CHANNEL* voice;

	// Configure Sound For 2D
	FMOD_MODE fmMode;
	FMOD_Sound_GetMode(l_somSounds[soundId].fmodSound, &fmMode);

	// Disable 3D Flag (if set), and set 2D flag.
	fmMode &= ~FMOD_3D;
	fmMode |=  FMOD_2D;

	FMOD_Sound_SetMode(l_somSounds[soundId].fmodSound, fmMode);

	// Try to play the sound
	if (fmResult = FMOD_System_PlaySound(l_fmodSystem, l_somSounds[soundId].fmodSound, l_fmodSfxChannelGroup, FALSE, &voice), fmResult != FMOD_OK)
		return false;

	// Convert SoM pitch to FMOD Pitch.
	// SoM gives a range of -2 to +2 octaves to playback sound, or -24 to 24. 0 is regular pitch... -24 needs to be mapped to 0.25, +24 needs to be mapped to 3.00 ?
	if (pitch != 0 && pitch >= -24 && pitch <= 24)
		FMOD_Channel_SetPitch(voice, (pitch - -24.0f) * (3.00f - 0.25f) / (24.0f - -24.0f) + 0.25f);

	return false;
}


// Hook N Fuck - Init, Deinit, Tick
void __cdecl SomSoundInitDetours()
{
	DetourAttach(&(PVOID&)ProxiedSomSoundInit, ProxySomSoundInit);

	DetourAttach(&(PVOID&)ProxiedSomSoundBGMPlay, ProxySomSoundBGMPlay);
	DetourAttach(&(PVOID&)ProxiedSomSoundBGMStop, ProxySomSoundBGMStop);

	DetourAttach(&(PVOID&)ProxiedSomSoundLoad, ProxySomSoundLoad);
	DetourAttach(&(PVOID&)ProxiedSomSoundUnload, ProxySomSoundUnload);
	DetourAttach(&(PVOID&)ProxiedSomSoundUnloadRef, ProxySomSoundUnloadRef);
	DetourAttach(&(PVOID&)ProxiedSomSoundPlay3D, ProxySomSoundPlay3D);
	DetourAttach(&(PVOID&)ProxiedSomSoundPlay2D, ProxySomSoundPlay2D);
}

void __cdecl SomSoundKillDetours()
{
	DetourDetach(&(PVOID&)ProxiedSomSoundInit, ProxySomSoundInit);

	DetourDetach(&(PVOID&)ProxiedSomSoundBGMPlay, ProxySomSoundBGMPlay);
	DetourDetach(&(PVOID&)ProxiedSomSoundBGMStop, ProxySomSoundBGMStop);

	DetourDetach(&(PVOID&)ProxiedSomSoundLoad, ProxySomSoundLoad);
	DetourDetach(&(PVOID&)ProxiedSomSoundUnload, ProxySomSoundUnload);
	DetourDetach(&(PVOID&)ProxiedSomSoundUnloadRef, ProxySomSoundUnloadRef);
	DetourDetach(&(PVOID&)ProxiedSomSoundPlay3D, ProxySomSoundPlay3D);
	DetourDetach(&(PVOID&)ProxiedSomSoundPlay2D, ProxySomSoundPlay2D);
}

void __cdecl SomSoundTick()
{
	// Don't ticket when fmod is null
	if (l_fmodSystem == NULL)
		return;

	// Caclculate Listener Vectors...
	l_fmodListenerPosition.x = *g_somPlayerX;
	l_fmodListenerPosition.y = *g_somPlayerY;
	l_fmodListenerPosition.z = *g_somPlayerZ;
	l_fmodListenerVelocity.x = 0.f;
	l_fmodListenerVelocity.y = 0.f;
	l_fmodListenerVelocity.z = 0.f;

	l_fmodListenerForward.x = -sin(*g_somCameraX);	// This is wrong but I've spent a few hours fiddling with it and don't want to do it anymore.
	l_fmodListenerForward.y =  0.f;
	l_fmodListenerForward.z =  cos(*g_somCameraX);

	l_fmodListenerUp.x = 0.f;
	l_fmodListenerUp.y = 1.f;
	l_fmodListenerUp.z = 0.f;

	// Set listener configuration before updating...
	FMOD_System_Set3DListenerAttributes(l_fmodSystem, 0, &l_fmodListenerPosition, &l_fmodListenerVelocity, &l_fmodListenerForward, &l_fmodListenerUp);

	//
	// FOOT STEP SOUND FEATURE
	//
	int32_t footStepSoundId = GetGameConfigInteger("SoundFootStepID");
	if (footStepSoundId >= 0)
	{
		// While the player isn't holding any of the movement keys, update their last position
		if (!GetRemappedKeyHeld("MovePlayerForward") && !GetRemappedKeyHeld("MovePlayerBack") && !GetRemappedKeyHeld("MovePlayerRight") && !GetRemappedKeyHeld("MovePlayerLeft"))
		{
			l_lastPlayerX = *g_somPlayerX;
			l_lastPlayerZ = *g_somPlayerZ;
		} 
		else 
		{
			// The player is holding _some_ movement key, lets do the magic now...

			float playerDLX = *g_somPlayerX - l_lastPlayerX;
			float playerDLZ = *g_somPlayerZ - l_lastPlayerZ;

			// If the distance between the last and current position is > 2m, we play the footstep sound...
			if (sqrt((playerDLX * playerDLX) + (playerDLZ * playerDLZ)) > 2.f)
			{
				ProxySomSoundPlay3D(footStepSoundId, 0, *g_somPlayerX, *g_somPlayerY, *g_somPlayerZ);

				l_lastPlayerX = *g_somPlayerX;
				l_lastPlayerZ = *g_somPlayerZ;
			}
		}	
	}

	// Now update.
	FMOD_System_Update(l_fmodSystem);
}