#ifndef _UNSEALCONF_H
#define _UNSEALCONF_H

#include <stdint.h>
#include <string>

/**
 * Game Configuration Types
**/
typedef struct
{
	std::string name;
	std::string author;
} GameConfigurationInfo;

typedef struct
{
	bool delayedStartup;
	bool goldDropCrash;
} GameConfigurationFixes;

typedef struct
{
	bool nonRootSave;
} GameConfigurationFeatures;

typedef struct
{
	bool useNewEngine;
	std::string bgmExtensionOverride;
	int footstepMode;
	int footstepSoundID;
	int footstepCounterID;
} GameConfigurationSound;

typedef struct
{
	bool useNewEngine;
} GameConfigurationInput;

typedef struct
{
	bool pickupMultiple;
	float pickupRange;
} GameConfigurationCurrency;

/**
 * Game Configuration Datas
**/
extern int g_gameConfigLogLevel;
extern GameConfigurationInfo g_gameConfigInfo;
extern GameConfigurationFixes g_gameConfigFixes;
extern GameConfigurationFeatures g_gameConfigFeatures;
extern GameConfigurationSound g_gameConfigSound;
extern GameConfigurationInput g_gameConfigInput;
extern GameConfigurationCurrency g_gameConfigCurrency;

/*************/
/* START OLD */
/*************/
extern uint32_t GetUserConfigU32(const char* fieldName);
extern uint32_t GetUserConfigBool(const char* fieldName);
extern float GetUserConfigFloat(const char* fieldName);

/***********/
/* END OLD */
/***********/

/**
 * Unsealer
**/
extern void __cdecl UnsealConfInit();
extern void __cdecl UnsealConfKill();


#endif