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
	LPBYTE lpb;
	RAWINPUT* raw;
	UINT dwSize;

	// There's a few messages we want to handle instead of SoM
	switch (uMsg)
	{
	case WM_INPUT:
		GetRawInputData((HRAWINPUT)lParam, RID_INPUT, NULL, &dwSize, sizeof(RAWINPUTHEADER));
		lpb = (BYTE*)malloc(dwSize);

		if (GetRawInputData((HRAWINPUT)lParam, RID_INPUT, lpb, &dwSize, sizeof(RAWINPUTHEADER)) != dwSize)
			LogFWrite("GetRawInputData does not return correct size !", "ProxySomWndProc");
		
		// This isn't safe?..
		raw = (RAWINPUT*)lpb;

		if (raw->header.dwType == RIM_TYPEKEYBOARD)
		{
			g_somCurrKeyState[raw->data.keyboard.VKey & 0xFF] = ((raw->data.keyboard.Flags & 0x1) == 0);
		}
		else
		if (raw->header.dwType == RIM_TYPEMOUSE)
		{
			// THIS CAN 100% BE IMPROVED... THINK MORE ABOUT IT IDIOT.
			if (((raw->data.mouse.usButtonFlags >> 0) & 0x3) != 0)
				g_somCurrMouseState[0] = ((raw->data.mouse.usButtonFlags >> 0) & 0x3) == 0x1;	// LEFT     BUTTON

			if (((raw->data.mouse.usButtonFlags >> 2) & 0x3) != 0)
				g_somCurrMouseState[1] = ((raw->data.mouse.usButtonFlags >> 2) & 0x3) == 0x1;	// RIGHT    BUTTON

			if (((raw->data.mouse.usButtonFlags >> 4) & 0x3) != 0)
				g_somCurrMouseState[2] = ((raw->data.mouse.usButtonFlags >> 4) & 0x3) == 0x1;	// MIDDLE   BUTTON

			if (((raw->data.mouse.usButtonFlags >> 6) & 0x3) != 0)
				g_somCurrMouseState[3] = ((raw->data.mouse.usButtonFlags >> 6) & 0x3) == 0x1;	// XBUTTON1 BUTTON

			if (((raw->data.mouse.usButtonFlags >> 8) & 0x3) != 0)
				g_somCurrMouseState[4] = ((raw->data.mouse.usButtonFlags >> 8) & 0x3) == 0x1;	// XBUTTON2 BUTTON

			// Move the camera with the house
			if (GetUserConfigBool("UseMouseLook") == 1)
			{
				*g_somCameraX = (*g_somCameraX - raw->data.mouse.lLastX / 180.f);
				*g_somCameraY = (*g_somCameraY - raw->data.mouse.lLastY / 180.f);
			}

			// Force Hide Cursor
			ShowCursor(FALSE);
		}
		return 0;

		default:
			return ProxiedSomWndProc(hWnd, uMsg, wParam, lParam);
	}

	return 0;
}

// SomSetDisplay sets the width, height and depth of the display?..
SomSetDisplayVars ProxiedSomSetDisplay = (SomSetDisplayVars)0x00446370;
BYTE __cdecl ProxySomSetDisplayVars(WORD width, WORD height, WORD depth)
{
	std::ostringstream out;
	out << "{ Width = " << width
		<< ", Height = " << height
		<< ", Depth = " << depth
		<< " }";

	LogFWrite(out.str(), "SomDDRAWSetDisplayMode");

	return ProxiedSomSetDisplay(width, height, depth);
}

// Temp
SomDrawHudStuff ProxiedSomDrawHudStuff = (SomDrawHudStuff)0x004256f0;
void __cdecl ProxySomDrawHudStuff()
{
	return;
}

SomSoundCreateBuffer ProxiedSomSoundCreateBuffer = (SomSoundCreateBuffer)0x0044a3d0;
BOOL __cdecl ProxySomSoundCreateBuffer(DWORD param_1, DWORD sampleRate, DWORD param_2)
{
	// Always provide 44100 as the sample rate
	sampleRate = 44100;

	// Run original function
	return ProxiedSomSoundCreateBuffer(param_1, sampleRate, param_2);
}
