#ifndef _SOMWINDOW_H
#define _SOMWINDOW_H

#include <Windows.h>

// Created Memory
extern HWND g_somHwnd1;
extern HDC g_somWindowDC;

// Proxies and Proxied
typedef HWND(__stdcall* SomCreateWindowExA)(DWORD, LPCSTR, LPCSTR, DWORD, int, int, int, int, HWND, HMENU, HINSTANCE, LPVOID);
extern SomCreateWindowExA ProxiedCreateWindowExA;
extern HWND __stdcall ProxyCreateWindowExA(DWORD dwExStyle, LPCSTR lpClassName, LPCSTR lpWindowName, DWORD dwStyle, int x, int y, int nWidth, int nHeight, HWND hWndParent, HMENU hMenu, HINSTANCE hInstance, LPVOID lpParam);

typedef ATOM(__stdcall* SomRegisterClassA)(const WNDCLASSA*);
extern SomRegisterClassA ProxiedRegisterClassA;
extern ATOM __stdcall ProxyRegisterClassA(WNDCLASSA* lpWndClass);

typedef LRESULT(__stdcall* SomWndProc)(HWND, UINT, WPARAM, LPARAM);
extern SomWndProc ProxiedSomWndProc;
extern LRESULT __stdcall ProxySomWndProc(HWND hWnd, UINT uMsg, WPARAM wParam, LPARAM lParam);

typedef BYTE(__cdecl* SomSetDisplayVars)(WORD, WORD, WORD);
extern SomSetDisplayVars ProxiedSomSetDisplay;
extern BYTE __cdecl ProxySomSetDisplayVars(WORD width, WORD height, WORD depth);

#endif