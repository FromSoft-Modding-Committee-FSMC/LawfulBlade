#include "somdraw.h"

#include "sommemory.h"
#include "somconf.h"
#include "logf.h"

#include <Windows.h>
#include <ostream>
#include <detours.h>

//
// Variables
//
int32_t* m_somMaterialUseCount  = (int32_t*)0x01d3c060;		// The current number of used materials
int32_t* m_somMaterialMaxCount  = (int32_t*)0x01d3c064;		// The maximum number of materials
int32_t* m_somMaterialInstances = (int32_t*)0x01d3c058;		// Allocation of materials...	(not actually an int, but we want to set the value to a pointer)

//
// Helpers
//
void __cdecl DumpTLVertex(uint32_t* buffer, int32_t vertices)
{
	std::ostringstream out;

	float* fbuff = (float*)buffer;

	out << "VERTICES: " << std::endl;

	for (int i = 0; i < vertices; ++i)
	{
		out << "Vertex (" << i << "):" << std::endl;
		out << "X = " << fbuff[0] << ", Y = " << fbuff[1] << ", Z = " << fbuff[2] << ", W = " << fbuff[3] << std::endl;
		out << "Diffuse = " << buffer[4] << ", Specular = " << buffer[5] << std::endl;
		out << "U = " << fbuff[6] << ", V = " << fbuff[7] << std::endl;
	}

	LogFWrite(out.str(), "SomDraw>DumpTLVertex");
}

//
// Func Types
//
typedef uint32_t(__cdecl* SomDrawMaterialSystemInit)();
typedef void(__cdecl* SomDrawMaterialStoreD3DDev)();
typedef void(__cdecl* SomDrawMaterialCreate)(float r, float g, float b, float a);
typedef int32_t(__cdecl* SomDrawMaterialCreateEx)(float r, float g, float b, float a, float emR, float emG, float emB);
typedef int32_t(__cdecl* SomDrawMaterialSetRGBA)(int32_t materialID, float r, float g, float b, float a);
typedef int32_t(__cdecl* SomDrawRenderFunc1)(uint32_t*);

//
// Proxied
//
SomDrawMaterialSystemInit ProxiedSomDrawMaterialSystemInit   = (SomDrawMaterialSystemInit)0x00446d70;
SomDrawMaterialStoreD3DDev ProxiedSomDrawMaterialStoreD3DDev = (SomDrawMaterialStoreD3DDev)0x00446eb0;
SomDrawMaterialCreate ProxiedSomDrawMaterialCreate           = (SomDrawMaterialCreate)0x00446ee0;
SomDrawMaterialCreateEx ProxiedSomDrawMaterialCreateEx		 = (SomDrawMaterialCreateEx)0x00447170;
SomDrawMaterialSetRGBA ProxiedSomDrawMaterialSetRGBA		 = (SomDrawMaterialSetRGBA)0x00447370;
SomDrawRenderFunc1 ProxiedSomDrawRenderFunc1				 = (SomDrawRenderFunc1)0x0044c820;

//
// Proxies
//
uint32_t __cdecl ProxySomDrawMaterialSystemInit()
{
	// Logging...
	LogFWrite("Initializing Material System...", "SomDraw>MaterialSystemInit");

	// Unsure why this actually exists, but it loads values into memory so we need to call it...
	ProxiedSomDrawMaterialStoreD3DDev();

	*m_somMaterialUseCount  = 0;
	*m_somMaterialMaxCount  = 4096;

	void* allocatedMaterialBlock = ProxiedSomMemoryAllocate(0x4C * *m_somMaterialMaxCount);
	*m_somMaterialInstances = (int32_t)allocatedMaterialBlock;

	// Now clear the block
	memset(allocatedMaterialBlock, 0, 0x4C * *m_somMaterialMaxCount);

	// Now SoM will create some default materials...
	ProxiedSomDrawMaterialCreate(1.0f, 1.0f, 1.0f, 1.0f);
	ProxiedSomDrawMaterialCreate(0.75f, 0.75f, 0.75f, 1.0f);
	ProxiedSomDrawMaterialCreate(0.5f, 0.5f, 0.5f, 1.0f);
	ProxiedSomDrawMaterialCreate(0.25f, 0.25f, 0.25f, 1.0f);
	ProxiedSomDrawMaterialCreate(0.0f, 0.0f, 0.0f, 1.0f);
	ProxiedSomDrawMaterialCreate(1.0f, 0.0f, 0.0f, 1.0f);
	ProxiedSomDrawMaterialCreate(0.0f, 1.0f, 0.0f, 1.0f);
	ProxiedSomDrawMaterialCreate(0.0f, 0.0f, 1.0f, 1.0f);

	// Original Call.
	return TRUE;
}

int32_t __cdecl ProxySomDrawMaterialCreateEx(float r, float g, float b, float a, float emR, float emG, float emB)
{
	return ProxiedSomDrawMaterialCreateEx(r, g, b, a, emR, emG, emB);
}

int32_t __cdecl ProxySomDrawMaterialSetRGBA(int32_t materialID, float r, float g, float b, float a)
{
	return ProxiedSomDrawMaterialSetRGBA(materialID, r, g, b, a);
}

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

//
// Unsealer
//
void __cdecl UnsealDrawInit()
{
	DetourAttach(&(PVOID&)ProxiedSomDrawMaterialSystemInit, ProxySomDrawMaterialSystemInit);
	DetourAttach(&(PVOID&)ProxiedSomDrawMaterialCreateEx, ProxySomDrawMaterialCreateEx);
	DetourAttach(&(PVOID&)ProxiedSomDrawMaterialSetRGBA, ProxySomDrawMaterialSetRGBA);
	DetourAttach(&(PVOID&)ProxiedSomDrawRenderFunc1, ProxyProxiedSomDrawRenderFunc1);
}

void __cdecl UnsealDrawKill()
{
	DetourDetach(&(PVOID&)ProxiedSomDrawMaterialSystemInit, ProxySomDrawMaterialSystemInit);
	DetourDetach(&(PVOID&)ProxiedSomDrawMaterialCreateEx, ProxySomDrawMaterialCreateEx);
	DetourDetach(&(PVOID&)ProxiedSomDrawMaterialSetRGBA, ProxySomDrawMaterialSetRGBA);
	DetourDetach(&(PVOID&)ProxiedSomDrawRenderFunc1, ProxyProxiedSomDrawRenderFunc1);
}

void __cdecl UnsealDrawTick()
{

}
