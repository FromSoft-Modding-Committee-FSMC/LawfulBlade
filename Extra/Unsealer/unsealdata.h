#ifndef _UNSEALDATA_H_
#define _UNSEALDATA_H_

#include <stdint.h>
#include "unsealmath.h"

/**
 * CAMERA DATA
**/
extern float& g_somCameraYaw;
extern float& g_somCameraPitch;

/**
 * PLAYER DATA
**/
extern VECTOR3F& g_somPlayerPosition;

/**
 * SOUND DATA
**/
extern int16_t* g_somSysDatUserSounds;
extern int8_t*  g_somSysDatMenuSounds;
extern uint8_t* g_somSoundVolumeSFX;
extern uint8_t* g_somSoundVolumeBGM;

/**
 * MISC DATA
**/
extern char* g_somSoundCurrentBGMFile;
extern uint8_t* g_somConfVolumeSFX;
extern uint8_t* g_somConfVolumeBGM;

/**
 * FIRST PARTY DATA
**/
extern bool g_unsealGameIsPaused;

#endif