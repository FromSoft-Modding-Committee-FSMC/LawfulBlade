#include <Windows.h>
#include <detours.h>
#include <ostream>
#include <ShlObj.h>

#include "unsealconf.h"
#include "unseallog.h"

#include "unsealfilesystem.h"

/**
 * Local Data
**/
char m_saveFileRoot[MAX_PATH];

/**
 * Function Type Definition
**/
typedef BOOL(__stdcall* SomCreateDirectoryA)(LPCSTR, LPSECURITY_ATTRIBUTES);
typedef FILE* (__cdecl* SomFileOpen)(char*, char*);

/**
 * Function Hooking
**/
SomCreateDirectoryA ProxiedCreateDirectoryA = CreateDirectoryA;
SomFileOpen ProxiedSomFileOpen = (SomFileOpen)0x0044e5ae;

/**
 * Function Proxies
**/
BOOL __stdcall ProxyCreateDirectoryA(LPCSTR lpPathName, LPSECURITY_ATTRIBUTES lpSecurityAttributes)
{
	// Make sure SoM doesn't create it's default save directory...
	if (strncmp(lpPathName, "save", 4) == 0)
		return TRUE;

	return ProxiedCreateDirectoryA(lpPathName, lpSecurityAttributes);
}

FILE* __cdecl ProxySomFileOpen(char* filename, char* mode)
{
	// non root save feature
	if (g_gameConfigFeatures.nonRootSave)
	{
		// Implement our injected replacements...
		if (*((uint32_t*)filename) == 0x65766173)	// SAVE OVERRIDE
		{
			// We must create a new path for the file name...
			char targetFilePath[MAX_PATH];

			strncpy_s(targetFilePath, m_saveFileRoot, MAX_PATH);
			targetFilePath[MAX_PATH - 1] = '\0';

			strncat_s(targetFilePath, &filename[4], MAX_PATH);	// fuck you, it is zero terminated...
			targetFilePath[MAX_PATH - 1] = '\0';

			filename = (char*)targetFilePath;
		}
	}

	return ProxiedSomFileOpen(filename, mode);
}

/**
 * Unsealer
**/
void __cdecl UnsealFileSystemInit()
{
	/**
	 * Save Hacks
	**/
	if (g_gameConfigFeatures.nonRootSave)
	{
		PWSTR appDataDir;

		// Clearing path to prevent invalid data being included...
		memset(&m_saveFileRoot[0], 0, MAX_PATH);

		if (SHGetKnownFolderPath(FOLDERID_RoamingAppData, 0, NULL, &appDataDir) == S_OK)
		{
			// Convert shitty Windows UTF-16 path to UTF-8...
			int32_t pathSize = WideCharToMultiByte(CP_UTF8, 0, appDataDir, -1, NULL, 0, NULL, NULL);
			if (pathSize > 0)
				WideCharToMultiByte(CP_UTF8, 0, appDataDir, -1, m_saveFileRoot, pathSize, NULL, NULL);

			// Now build the save path...
			strncat_s(m_saveFileRoot, "\\", MAX_PATH);
			strncat_s(m_saveFileRoot, g_gameConfigInfo.author.c_str(), MAX_PATH);
			CreateDirectoryA(m_saveFileRoot, 0);

			strncat_s(m_saveFileRoot, "\\", MAX_PATH);
			strncat_s(m_saveFileRoot, g_gameConfigInfo.name.c_str(), MAX_PATH);
			CreateDirectoryA(m_saveFileRoot, 0);

			strncat_s(m_saveFileRoot, "\\saves", MAX_PATH);
			CreateDirectoryA(m_saveFileRoot, 0);
		}
		else
			UnsealLog("Failed to get new save data location...", "FileSystem>Init", UNSEAL_LOG_LEVEL_SHIT);

		CoTaskMemFree(appDataDir);

		DetourAttach(&(PVOID&)ProxiedCreateDirectoryA, ProxyCreateDirectoryA);
	}

	DetourAttach(&(PVOID&)ProxiedSomFileOpen, ProxySomFileOpen);
}

void __cdecl UnsealFileSystemKill()
{
	if (g_gameConfigFeatures.nonRootSave)
	{
		DetourDetach(&(PVOID&)ProxiedCreateDirectoryA, ProxyCreateDirectoryA);
	}

	DetourDetach(&(PVOID&)ProxiedSomFileOpen, ProxySomFileOpen);
}
