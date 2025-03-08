#ifndef _SOMWINDOW_H
#define _SOMWINDOW_H

#include <Windows.h>


// Stolen Memory

// Function Types
typedef ATOM(__stdcall* SomRegisterClassA)(const WNDCLASSA* lpWndClass);
typedef LRESULT(__stdcall* SomWndProc)(HWND hWnd, UINT uMsg, WPARAM wParam, LPARAM lParam);

// Original Functions
extern SomRegisterClassA ProxiedRegisterClassA;
extern SomWndProc ProxiedSomWndProc;

// Proxy Functions
extern ATOM __stdcall ProxyRegisterClassA(WNDCLASSA* lpWndClass);
extern LRESULT __stdcall ProxySomWndProc(HWND hWnd, UINT uMsg, WPARAM wParam, LPARAM lParam);

#endif