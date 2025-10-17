#include <Windows.h>
#include <detours.h>

#include "unsealdata.h"
#include "unsealconf.h"
#include "unseallog.h"

#include "unsealdraw.h"

/**
 * Function Type Definition
**/
typedef uint32_t(__cdecl* SomTitleLogoDisplay)(void*);
typedef int32_t(__cdecl* SomDrawRenderFunc1)(uint32_t*);

/**
 * Function Hooking
**/
SomTitleLogoDisplay ProxiedSomTitleLogoDisplay = (SomTitleLogoDisplay)0x0042b2e0;
SomDrawRenderFunc1 ProxiedSomDrawRenderFunc1 = (SomDrawRenderFunc1)0x0044c820;

/**
 * Function Proxies
**/
uint32_t ProxySomTitleLogoDisplay(void* param_1)
{
	return 1;
}

/*
int32_t __cdecl ProxyProxiedSomDrawRenderFunc1(uint32_t* renderObject)
{
	// 0 = unknown
	// 1 = shadow
	// 2 = object, item, enemy, fades
	// 3 = unknown
	// 4 = gdi content?..
	// 5 = unknown
	// 6 = unknown
	// 7 = unknown
	// 8 = Screen Effect
	uint32_t drawType = (renderObject[1]) >> 0xc & 0xf;

	switch (drawType)
	{
		// Screen Effect
		case 8:

			// Get the type of screen effect
			uint32_t screenEffectType = renderObject[0] & 0xf;
			
			switch (screenEffectType)
			{
				// Red Flash
				case 2:

					// Declare some data
					float cToScale, tForLerp;
					uint8_t flashR, flashG, flashB;
					uint32_t flashColourStart, flashColourEnd, flashColourFinal;

					// Grab the fvfbuffer
					uint32_t* fvfBuffer = (renderObject + 4);

					switch (GetGameConfigInteger("DamageFlashMode"))
					{
						// Unsealed Mode #1 - Pulse
						case 1:
							// First we caculate a lerping value from the original colour (which is 0xFF0000 -> 0x000000) (there might be a lerp value passed to the renderObject...)
							cToScale = (float)fvfBuffer[04] / (float)16711680;

							// Use a bell curve on the scale to smooth it out, and alter the shape of the effect, then scale it by intensity
							cToScale  = sinf(3.14159265359f * cToScale);
							cToScale *= GetGameConfigFloat("DamageFlashIntensity");

							// Calculate our new colour
							flashColourStart = GetGameConfigInteger("DamageFlashColour");
							flashR = (uint8_t)(((flashColourStart >> 16) & 0xFF) * cToScale);
							flashG = (uint8_t)(((flashColourStart >>  8) & 0xFF) * cToScale);
							flashB = (uint8_t)(((flashColourStart >>  0) & 0xFF) * cToScale);

							// Merge the colour back together, assign to vbuffer
							flashColourFinal = ((flashR & 0xFF) << 16) | ((flashG & 0xFF) << 8) | ((flashB & 0xFF) << 0);

							fvfBuffer[04] = flashColourFinal;
							fvfBuffer[12] = flashColourFinal;
							fvfBuffer[20] = flashColourFinal;
							fvfBuffer[28] = flashColourFinal;
							break;

						// Unsealed Mode #2 - Pulse + Lerp
						case 2:
							tForLerp = (float)fvfBuffer[04] / (float)16711680;

							cToScale = sinf(3.14159265359f * tForLerp);
							cToScale *= GetGameConfigFloat("DamageFlashIntensity");

							// Calculate our new colour
							flashColourEnd   = GetGameConfigInteger("DamageFlashColour");
							flashColourStart = GetGameConfigInteger("DamageFlashColourStart");

							// Difference is here between Unsealed #mode 1. First lerp the colours.
							flashColourFinal = (uint32_t)(flashColourStart * (1 - tForLerp) + flashColourEnd * tForLerp);

							// Difference from 1 is here. Need to lerp between flash colour start and end based on the tForLerp value...
							flashR = (uint8_t)(((flashColourFinal >> 16) & 0xFF) * cToScale);
							flashG = (uint8_t)(((flashColourFinal >>  8) & 0xFF) * cToScale);
							flashB = (uint8_t)(((flashColourFinal >>  0) & 0xFF) * cToScale);

							// Merge the colour back together
							flashColourFinal = ((flashR & 0xFF) << 16) | ((flashG & 0xFF) << 8) | ((flashB & 0xFF) << 0);

							fvfBuffer[04] = flashColourFinal;
							fvfBuffer[12] = flashColourFinal;
							fvfBuffer[20] = flashColourFinal;
							fvfBuffer[28] = flashColourFinal;
							break;

							// Unsealed Mode #2 - No Flash
						case 3:
							fvfBuffer[04] = 0;
							fvfBuffer[12] = 0;
							fvfBuffer[20] = 0;
							fvfBuffer[28] = 0;
							break;

						// Default (SoM Behaviour)
						default: break;
					}
				break;
			}
		break;
	}

	return 	ProxiedSomDrawRenderFunc1(renderObject);
}
*/

/**
 * Unsealer
**/
void __cdecl UnsealDrawInit()
{
	if (g_gameConfigFixes.delayedStartup)
		DetourAttach(&(PVOID&)ProxiedSomTitleLogoDisplay, ProxySomTitleLogoDisplay);

	// DetourAttach(&(PVOID&)ProxiedSomDrawRenderFunc1, ProxyProxiedSomDrawRenderFunc1);
}

void __cdecl UnsealDrawKill()
{
	if (g_gameConfigFixes.delayedStartup)
		DetourDetach(&(PVOID&)ProxiedSomTitleLogoDisplay, ProxySomTitleLogoDisplay);

	// DetourDetach(&(PVOID&)ProxiedSomDrawRenderFunc1, ProxyProxiedSomDrawRenderFunc1);
}
