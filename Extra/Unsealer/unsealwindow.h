#ifndef _UNSEALWINDOW_H
#define _UNSEALWINDOW_H

#include <Windows.h>

extern HWND g_somWindowHandle;

extern void __cdecl UnsealWindowInit();
extern void __cdecl UnsealWindowKill();

#endif