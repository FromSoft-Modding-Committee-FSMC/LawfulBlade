#ifndef _SOMGDI_H_
#define _SOMGDI_H_

#include <Windows.h>
#include <string>
#include <stdint.h>

// Struct is used for handling fonts in our game configuration
typedef struct
{
	std::string typeface;
	int32_t     weight;
} SomFontConfiguration;

// CreateFontA
typedef HFONT(__stdcall* SomCreateFontA)(int, int, int, int, int, DWORD, DWORD, DWORD, DWORD, DWORD, DWORD, DWORD, DWORD, LPCSTR);
extern SomCreateFontA ProxiedCreateFontA;
extern HFONT __stdcall ProxyCreateFontA(int cHeight, int cWidth, int cEscapement, int cOrientation, int cWeight, DWORD bItalic, DWORD bUnderline, DWORD bStrikeOut, DWORD iCharSet, DWORD iOutPrecision, DWORD iClipPrecision, DWORD iQuality, DWORD iPitchAndFamily, LPCSTR pszFaceName);

// TextOutA
typedef BOOL(__stdcall* SomTextOutA)(HDC, int, int, LPCSTR, int);
extern SomTextOutA ProxiedTextOutA;
extern BOOL __stdcall ProxySomTextOutA(HDC hdc, int x, int y, LPCSTR lpString, int c);

// Our logic
extern void __cdecl SomGdiInit();
extern void __cdecl SomGdiKill();


#endif