#include "somconf.h"

#include "sdk\json.hpp"


// Locals
nlohmann::json l_userConfigObject;
nlohmann::json l_gameConfigObject;

/// <summary>
/// Loads the user configuration file, which contains options the user can modify
/// </summary>
void LoadUserConfiguration()
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
/// Loads the game configuration file, which contains options the developer specifies
/// </summary>
void LoadGameConfiguration()
{
	// Load User Configuration File
	std::ifstream f("GAME.CONF.SEAL");
	l_gameConfigObject = nlohmann::json::parse(f);
}

/// <summary>
/// Gets a string value from game configuration
/// </summary>
/// <param name="fieldName">The name of the field</param>
/// <returns>The value of the field</returns>
std::string GetGameConfigString(const char* fieldName)
{
	return l_gameConfigObject[fieldName].get<std::string>();
}

/// <summary>
/// Gets a boolean value from game configuration
/// </summary>
/// <param name="fieldName">The name of the field</param>
/// <returns>The value of the field</returns>
uint32_t GetGameConfigBool(const char* fieldName)
{
	return l_gameConfigObject[fieldName].get<bool>() == true ? 1 : 0;
}

/// <summary>
/// Gets a integer value from game configuration
/// </summary>
/// <param name="fieldName">The name of the field</param>
/// <returns>The value of the field</returns>
int32_t GetGameConfigInteger(const char* fieldName)
{
	return l_gameConfigObject[fieldName].get<int32_t>();
}
