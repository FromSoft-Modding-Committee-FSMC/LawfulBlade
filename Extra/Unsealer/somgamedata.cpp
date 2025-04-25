#include "somgamedata.h"

// 16 user selectable sounds...
int16_t* g_somSysDatUserSounds = (int16_t*)0x01d19bba;

// 1-4 bank switch for menu sounds
int8_t* g_somSysDatMenuSounds  = (int8_t*)0x01d19c7b;

// Camera Rotation
float* g_somCameraX = (float*)0x019C0BB8;
float* g_somCameraY = (float*)0x019C0BB4;

// Player Position
float* g_somPlayerX = (float*)0x019C0BA8;
float* g_somPlayerY = (float*)0x019C0BAC;
float* g_somPlayerZ = (float*)0x019C0BB0;

bool* g_somGameIsPaused = (bool*)0x004c1163;

// Sound
uint8_t* g_somSoundVolumeSFX   = (uint8_t*)0x01d10c61;
uint8_t* g_somSoundVolumeBGM   = (uint8_t*)0x01d11464;
char* g_somSoundCurrentBGMFile = (char*)0x01d11480;

// Configuration
uint8_t* g_somConfVolumeSFX = (uint8_t*)0x004bf660;
uint8_t* g_somConfVolumeBGM = (uint8_t*)0x004bf661;