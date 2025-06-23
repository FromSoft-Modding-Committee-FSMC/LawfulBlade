#include <Windows.h>
#include <detours.h>

#include "unsealdata.h"
#include "unsealconf.h"
#include "unseallog.h"

#include "sominput.h"
#include "somsound.h";


#include "unsealwindow.h"

/**
 * Local Data
**/
HWND g_somWindowHandle = NULL;

/**
 * Function Type Definition
**/
typedef HWND(__stdcall* SomCreateWindowExA)(DWORD, LPCSTR, LPCSTR, DWORD, int, int, int, int, HWND, HMENU, HINSTANCE, LPVOID);
typedef LRESULT(__stdcall* SomDefWindowProcA)(HWND, UINT, WPARAM, LPARAM);

/**
 * Function Hooking
**/
SomCreateWindowExA ProxiedCreateWindowExA = CreateWindowExA;
SomDefWindowProcA ProxiedDefWindowProcA = DefWindowProcA;

/**
 * Function Proxies
**/
HWND __stdcall ProxyCreateWindowExA(DWORD dwExStyle, LPCSTR lpClassName, LPCSTR lpWindowName, DWORD dwStyle, int x, int y, int nWidth, int nHeight, HWND hWndParent, HMENU hMenu, HINSTANCE hInstance, LPVOID lpParam)
{
	// Create our window and capture the hWnd.
	g_somWindowHandle = ProxiedCreateWindowExA(dwExStyle, lpClassName, lpWindowName, dwStyle, x, y, nWidth, nHeight, hWndParent, hMenu, hInstance, lpParam);

	// Return the handle
	return g_somWindowHandle;
}

LRESULT __stdcall ProxyDefWindowProcA(HWND hWnd, UINT uMsg, WPARAM wParam, LPARAM lParam)
{
	switch (uMsg)
	{
		// FOCUS
		case WM_SETFOCUS:
			UnsealLog("Window Got Focus", "Window>DefWindowProcA", UNSEAL_LOG_LEVEL_OOPS);

			UnsealSoundOnResume();
			g_unsealGameIsPaused = false;
		return 0;

		case WM_KILLFOCUS:
			UnsealLog("Window Lost Focus", "Window>DefWindowProcA", UNSEAL_LOG_LEVEL_OOPS);

			UnsealSoundOnSuspend();
			g_unsealGameIsPaused = true;
		return 0;

		// INPUT
		case WM_INPUT:
			UnsealProcessRawInput((HRAWINPUT)lParam);
		return 0;
	}

	return ProxiedDefWindowProcA(hWnd, uMsg, wParam, lParam);
}

/**
 * Unsealer
**/
void __cdecl UnsealWindowInit()
{
	DetourAttach(&(PVOID&)ProxiedCreateWindowExA, ProxyCreateWindowExA);
	DetourAttach(&(PVOID&)ProxiedDefWindowProcA, ProxyDefWindowProcA);
}

void __cdecl UnsealWindowKill()
{
	DetourDetach(&(PVOID&)ProxiedCreateWindowExA, ProxyCreateWindowExA);
	DetourDetach(&(PVOID&)ProxiedDefWindowProcA, ProxyDefWindowProcA);
}