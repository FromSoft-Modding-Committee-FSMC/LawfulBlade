#include "somconf.h"
#include "logf.h"
#include "hasher.h"

#include "sdk\json.hpp"

// Locals
nlohmann::json l_userConfigObject;
nlohmann::json l_gameConfigObject;

// Constants
const int32_t l_kingsmapDeltaTable[4]   = { +1, +3, -2, +4 };
const int32_t l_kingsmapSwizzleTable[8] = { 3,6,2,5,0,7,4,1 };

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
/// Gets a float value from user configuration
/// </summary>
/// <param name="fieldName">The name of the field</param>
/// <returns>The value of the field</returns>
float GetUserConfigFloat(const char* fieldName)
{
	return l_userConfigObject[fieldName].get<float>();
}


/// <summary>
/// Loads the game configuration file, which contains options the developer specifies
/// </summary>
void LoadGameConfiguration()
{
	// Load User Configuration File
	std::ifstream f("GAME.CONF.SEAL", std::ios::binary);

	int32_t formatTag;
	int16_t configMode;

	// Read the header of our game config file...
	f.read((char*)&formatTag, 4);
	f.read((char*)&configMode, 2);
	f.seekg(0, f.end);
	int32_t confSize = ((int32_t)f.tellg()) - 8;
	f.seekg(8, f.beg);

	// Check Header Tag...
	if (formatTag != 0x664E534A)
		LogFWrite("CONFIG PARSE ERROR", "SomConf>LoadGameConfiguration");

	// Read the json file to a buffer
	char* ptrToData = (char*)malloc(confSize + 1);
	f.read(ptrToData, confSize);
	ptrToData[confSize] = 0x00;
	f.close();

	// Check if encrypted...
	if (configMode == 0x5445)
	{
		LogFWrite("Decrypting Game Config...", "SomConf>LoadGameConfig");

		// Load the project.dat
		std::ifstream projDat("PROJECT.DAT", std::ios::binary);
		char projDatValue[4];
		projDat.read(projDatValue, 4);
		projDat.close();
		int32_t fnvProjDat = HashAsFNV32(projDatValue, 4);	// The project.dat value itself isn't very good, so we hash it
		byte* fnvProjBytes = (byte*)&fnvProjDat;

		// Reverse Pass #3: Delta 32
		for (int i = 0; i < confSize; ++i)
			ptrToData[i] += l_kingsmapDeltaTable[i % 4];	// Pattern: +1, +3, -2, +4
			
		// Reverse Pass #2: Swizzle 64
		char swizzle64[8] = { 0,0,0,0,0,0,0,0 };
		int i = 0;
		do 
		{
			if ((confSize - i) < 8)
				break;

			// Swizzle 8 bytes
			for (int j = 0; j < 8; ++j)
				swizzle64[j] = ptrToData[i + l_kingsmapSwizzleTable[j]];

			// Write 8 bytes
			for (int j = 0; j < 8; ++j)
				ptrToData[i + j] = swizzle64[j];

			i += 8;

		} while (i < confSize);

		// Reverse Pass #1: XOR
		for (int i = 0; i < confSize; ++i)
			ptrToData[i] ^= fnvProjBytes[i % 4];
	}

	std::ofstream exam("example.txt", std::ios::binary);
	exam.write(ptrToData, confSize);
	exam.close();

	// Deserialize our json object from our text
	l_gameConfigObject = nlohmann::json::parse(ptrToData, nullptr, true, true);

	free((void*)ptrToData);
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

/// <summary>
/// Gets a float value from game configuration
/// </summary>
/// <param name="fieldName">The name of the field</param>
/// <returns>The value of the field</returns>
float GetGameConfigFloat(const char* fieldName)
{
	return l_gameConfigObject[fieldName].get<float>();
}
