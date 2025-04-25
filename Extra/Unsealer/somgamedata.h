#ifndef _SOMGAMEDATA_H_
#define _SOMGAMEDATA_H_

#include <stdint.h>

extern int16_t* g_somSysDatUserSounds;
extern int8_t*  g_somSysDatMenuSounds;// Some Stolen Variables
extern float* g_somCameraX;
extern float* g_somCameraY;
extern float* g_somPlayerX;
extern float* g_somPlayerY;
extern float* g_somPlayerZ;


extern bool* g_somGameIsPaused;

// Sound
extern uint8_t* g_somSoundVolumeSFX;
extern uint8_t* g_somSoundVolumeBGM;
extern char* g_somSoundCurrentBGMFile;

// Configuration
extern uint8_t* g_somConfVolumeSFX;
extern uint8_t* g_somConfVolumeBGM;

#endif