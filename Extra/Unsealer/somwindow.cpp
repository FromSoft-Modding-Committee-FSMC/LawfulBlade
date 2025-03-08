#include "somwindow.h"
#include "logf.h"

SomRegisterClassA ProxiedRegisterClassA = RegisterClassA;
SomWndProc ProxiedSomWndProc = NULL;

ATOM __stdcall ProxyRegisterClassA(WNDCLASSA* lpWndClass)
{
	// Copy Soms WndProc
	ProxiedSomWndProc = lpWndClass->lpfnWndProc;

	// Replace the WndProc with our own implementation...
	lpWndClass->lpfnWndProc = ProxySomWndProc;

	return ProxiedRegisterClassA(lpWndClass);
}

LRESULT __stdcall ProxySomWndProc(HWND hWnd, UINT uMsg, WPARAM wParam, LPARAM lParam)
{
	// std::ostringstream out;
	// out << "{ WndMessage = " << uMsg << ", WndWParam = " << wParam << ", WndLParam = " << lParam << "} ";
	// LogFWrite(out.str(), "ProxySomWndProc");

	// There's a few messages we want to handle instead of SoM
	switch (uMsg)
	{
	case WM_INPUT:
		LogFWrite("Got RawInput Message!", "ProxySomWndProc");

		// https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-registerrawinputdevices
		break;
	}

	return ProxiedSomWndProc(hWnd, uMsg, wParam, lParam);
}