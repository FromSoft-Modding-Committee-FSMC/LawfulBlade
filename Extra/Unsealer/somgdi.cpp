#include <map>

#include "somgdi.h"
#include "somconf.h"
#include "logf.h"
#include "hasher.h"

#include "sdk/json.hpp"
#include "sdk/detours/inc/detours.h"

// Store loaded fonts
std::map<uint32_t, SomFontConfiguration> l_textFontData;

// Capture N Wrap - CreateFontA
SomCreateFontA ProxiedCreateFontA = CreateFontA;
HFONT __stdcall ProxyCreateFontA(int cHeight, int cWidth, int cEscapement, int cOrientation, int cWeight, DWORD bItalic, DWORD bUnderline, DWORD bStrikeOut, DWORD iCharSet, DWORD iOutPrecision, DWORD iClipPrecision, DWORD iQuality, DWORD iPitchAndFamily, LPCSTR pszFaceName)
{
	// Get the hash of the face name...
	uint32_t faceNameHash = HashAsFNV32(pszFaceName, strnlen_s(pszFaceName, 32));

	// If the face name exists in our map, we want to override properties of it...
	if (l_textFontData.count(faceNameHash))
	{
		// If it does - we want to override certain parameters...
		pszFaceName = l_textFontData[faceNameHash].typeface.c_str();
		cWeight     = l_textFontData[faceNameHash].weight;
	}
	else 
	{
		// We will log the font so we can add a configuration for it...
		std::ostringstream out;
		out << "Font Info: { Width = " << cWidth << ", Height = " << cHeight << ", Face Hash = " << faceNameHash << " }";
		LogFWrite(out.str(), "DllMain>SomMainLoopFun");
	}

	// Return the created font.
	return ProxiedCreateFontA(cHeight, cWidth, 0, 0, cWeight, 0, 0, 0, DEFAULT_CHARSET, OUT_TT_PRECIS, CLIP_DEFAULT_PRECIS, PROOF_QUALITY, DEFAULT_PITCH | FF_ROMAN, pszFaceName);
}

// Capture N Wrap - TextOutA
SomTextOutA ProxiedTextOutA = TextOutA;
BOOL __stdcall ProxySomTextOutA(HDC hdc, int x, int y, LPCSTR lpString, int c)
{
	// We will log the font so we can add a configuration for it...
	std::ostringstream out;
	out << "TextOutA: { Value = " << lpString << " }";
	LogFWrite(out.str(), "somgdi>ProxySomTextOutA");

	return ProxiedTextOutA(hdc, x, y, lpString, c);
}


// Hook N Fuck - Init, Deinit, Tick
void __cdecl SomGdiInit()
{
	// Load our font configurations...
	l_textFontData[4085079925] = { GetGameConfigString("TextMenuFontFace"), GetGameConfigInteger("TextMenuFontWeight") };	// SoM1 Font

	// Detouring
	DetourAttach(&(PVOID&)ProxiedCreateFontA, ProxyCreateFontA);
	// DetourAttach(&(PVOID&)ProxiedTextOutA, ProxySomTextOutA);	- Disabled for now, but we could potentially use this for localization?
}

void __cdecl SomGdiKill()
{
	// Undetouring
	DetourDetach(&(PVOID&)ProxiedCreateFontA, ProxyCreateFontA);
	// DetourDetach(&(PVOID&)ProxiedTextOutA, ProxySomTextOutA);
}