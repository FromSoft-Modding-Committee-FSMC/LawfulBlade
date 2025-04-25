#include "somgamedata.h"
#include "somwindow.h"
#include "sominput.h"
#include "somconf.h"

#include "logf.h"

// Globals
HWND g_somHwnd1   = NULL;
HDC g_somWindowDC = NULL;

// CreateWindowExA creates a window for display
SomCreateWindowExA ProxiedCreateWindowExA = CreateWindowExA;
HWND __stdcall ProxyCreateWindowExA(DWORD dwExStyle, LPCSTR lpClassName, LPCSTR lpWindowName, DWORD dwStyle, int x, int y, int nWidth, int nHeight, HWND hWndParent, HMENU hMenu, HINSTANCE hInstance, LPVOID lpParam)
{
	// Parameter Overrides
	// dwExStyle = 0;								// Disable WS_EX_TOPMOST
	// dwStyle = WS_CLIPSIBLINGS | WS_OVERLAPPED;	// Borderless Style?..

	// Create our window and capture the hWnd.
	g_somHwnd1 = ProxiedCreateWindowExA(dwExStyle, lpClassName, lpWindowName, dwStyle, x, y, nWidth, nHeight, hWndParent, hMenu, hInstance, lpParam);

	// Get Device Context
	g_somWindowDC = GetDC(g_somHwnd1);

	// Return the handle
	return g_somHwnd1;
}

// RegisterClassA registers a new class for creating windows
SomRegisterClassA ProxiedRegisterClassA = RegisterClassA;
ATOM __stdcall ProxyRegisterClassA(WNDCLASSA* lpWndClass)
{
	// Copy Soms WndProc
	ProxiedSomWndProc = lpWndClass->lpfnWndProc;

	// Replace the WndProc with our own implementation...
	lpWndClass->lpfnWndProc = ProxySomWndProc;

	return ProxiedRegisterClassA(lpWndClass);
}

// SomWndProc handles window messages
SomWndProc ProxiedSomWndProc = NULL;
LRESULT __stdcall ProxySomWndProc(HWND hWnd, UINT uMsg, WPARAM wParam, LPARAM lParam)
{
	switch (uMsg)
	{
		case WM_INPUT: UnsealProcessRawInput((HRAWINPUT)lParam); return 0;
	}

	return ProxiedSomWndProc(hWnd, uMsg, wParam, lParam);
}

// SomSetDisplay sets the width, height and depth of the display?..
SomSetDisplayVars ProxiedSomSetDisplay = (SomSetDisplayVars)0x00446370;
BYTE __cdecl ProxySomSetDisplayVars(WORD width, WORD height, WORD depth)
{
	return ProxiedSomSetDisplay(width, height, depth);
}
