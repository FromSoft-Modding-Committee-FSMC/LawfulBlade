#include "unsealdata.h"

/**
 * CAMERA DATA
**/
float& g_somCameraYaw   = *(float*)0x019C0BB8;
float& g_somCameraPitch = *(float*)0x019C0BB4;

/**
 * SOUND DATA
**/
int16_t* g_somSysDatUserSounds = (int16_t*)0x01d19bba;
int8_t* g_somSysDatMenuSounds  = (int8_t*)0x01d19c7b;
uint8_t* g_somSoundVolumeSFX   = (uint8_t*)0x01d10c61;
uint8_t* g_somSoundVolumeBGM   = (uint8_t*)0x01d11464;

/**
 * MISC DATA
**/
char* g_somSoundCurrentBGMFile = (char*)0x01d11480;
uint8_t* g_somConfVolumeSFX = (uint8_t*)0x004bf660;
uint8_t* g_somConfVolumeBGM = (uint8_t*)0x004bf661;

/**
 * FIRST PARTY DATA
**/
bool g_unsealGameIsPaused = false;