#include <fkyaml/node.hpp>
#include <fstream>

#include "unsealconf.h"

/**
 * Game Configuration Datas
**/
int g_gameConfigLogLevel;
GameConfigurationInfo g_gameConfigInfo;
GameConfigurationFixes g_gameConfigFixes;
GameConfigurationFeatures g_gameConfigFeatures;
GameConfigurationSound g_gameConfigSound;
GameConfigurationInput g_gameConfigInput;
GameConfigurationCurrency g_gameConfigCurrency;

/**
 * Game Configuration YAML Deserialization Overloads
**/
void from_node(const fkyaml::node& node, GameConfigurationInfo& info)
{
	info.name = node["name"].get_value<std::string>();
	info.author = node["author"].get_value<std::string>();
}

void from_node(const fkyaml::node& node, GameConfigurationFixes& fixes)
{
	fixes.delayedStartup = node["delayedStartup"].get_value<bool>();
	fixes.goldDropCrash = node["goldDropCrash"].get_value<bool>();
}

void from_node(const fkyaml::node& node, GameConfigurationFeatures& features)
{
	features.nonRootSave = node["nonRootSave"].get_value<bool>();
}

void from_node(const fkyaml::node& node, GameConfigurationSound& sound)
{
	sound.useNewEngine = node["useNewEngine"].get_value<bool>();
	sound.bgmExtensionOverride = node["bgmExtensionOverride"].get_value<std::string>();
	sound.footstepMode = node["footstepMode"].get_value<int>();
	sound.footstepSoundID = node["footstepSoundID"].get_value<int>();
	sound.footstepCounterID = node["footstepCounterID"].get_value<int>();
}

void from_node(const fkyaml::node& node, GameConfigurationInput& input)
{
	input.useNewEngine = node["useNewEngine"].get_value<bool>();
}

void from_node(const fkyaml::node& node, GameConfigurationCurrency& currency)
{
	currency.pickupMultiple = node["pickupMultiple"].get_value<bool>();
	currency.pickupRange = node["pickupRange"].get_value<float>();
}

/**
 * Game Configuration Interaction
**/
void GameConfigurationLoad()
{
	// Open YAML file for reading...
	std::ifstream gameYaml("game.yaml");
	fkyaml::node root = fkyaml::node::deserialize(gameYaml);

	g_gameConfigLogLevel = root["logLevel"].get_value<int>();

	g_gameConfigInfo = root["info"].get_value<GameConfigurationInfo>();
	g_gameConfigFixes = root["fixes"].get_value<GameConfigurationFixes>();
	g_gameConfigFeatures = root["features"].get_value<GameConfigurationFeatures>();
	g_gameConfigSound = root["sound"].get_value<GameConfigurationSound>();
	g_gameConfigInput = root["input"].get_value<GameConfigurationInput>();
	g_gameConfigCurrency = root["currency"].get_value<GameConfigurationCurrency>();
}

/*************/
/* START OLD */
/*************/
#include "json.hpp"

// Locals
nlohmann::json l_userConfigObject;

/// <summary>
/// Loads the user configuration file, which contains options the user can modify
/// </summary>
void UserConfigurationLoad()
{
	// Load User Configuration File
	std::ifstream f("USER.CONF.SEAL");
	l_userConfigObject = nlohmann::json::parse(f);
}

/// <summary>
/// Gets a unsigned integer value from user configuration
/// </summary>
/// <param name="fieldName">The name of the field</param>
/// <returns>The value of the field</returns>
uint32_t GetUserConfigU32(const char* fieldName)
{
	return l_userConfigObject[fieldName].get<uint32_t>();
}

/// <summary>
/// Gets a boolean value from user configuration
/// </summary>
/// <param name="fieldName">The name of the field</param>
/// <returns>The value of the field</returns>
uint32_t GetUserConfigBool(const char* fieldName)
{
	return l_userConfigObject[fieldName].get<bool>() == true ? 1 : 0;
}

/// <summary>
/// Gets a float value from user configuration
/// </summary>
/// <param name="fieldName">The name of the field</param>
/// <returns>The value of the field</returns>
float GetUserConfigFloat(const char* fieldName)
{
	return l_userConfigObject[fieldName].get<float>();
}
/***********/
/* END OLD */
/***********/

/**
 * Unsealer
**/
void __cdecl UnsealConfInit()
{
	GameConfigurationLoad();
	UserConfigurationLoad();
}

void __cdecl UnsealConfKill()
{

}