#include "somscreeneffect.h"
#include "somconf.h"
#include "logf.h"

#include <Windows.h>
#include <ostream>
#include <detours.h>

uint32_t* m_someValueFromRedFlash = (uint32_t*)0x01d69090;

//
// Func Types
//
typedef void(__cdecl* SomScreenEffectRender)();
typedef void(__cdecl* SomScreenEffectUpdate)(uint32_t);
typedef uint32_t(__cdecl* SomScreenEffectCreate)(uint8_t, uint8_t, uint32_t);
typedef int32_t(__cdecl* SomScreenEffectUnknown)(RECT*, uint32_t, uint32_t);


//
// Proxied
//
SomScreenEffectRender ProxiedSomScreenEffectRender = (SomScreenEffectRender)0x0042cda0;
SomScreenEffectUpdate ProxiedSomScreenEffectUpdate = (SomScreenEffectUpdate)0x0042cc50;
SomScreenEffectCreate ProxiedSomScreenEffectCreate = (SomScreenEffectCreate)0x0042c870;
SomScreenEffectUnknown ProxiedSomScreenEffectUnknown = (SomScreenEffectUnknown)0x00449170;


//
// Proxies
//
void ProxySomScreenEffectRender()
{
	ProxiedSomScreenEffectRender();
}

void ProxySomScreenEffectUpdate(uint32_t updateTime)
{
	ProxiedSomScreenEffectUpdate(updateTime);
}

uint32_t ProxySomScreenEffectCreate(uint8_t type, uint8_t param_2, uint32_t param_3)
{
	return ProxiedSomScreenEffectCreate(type, param_2, param_3);
}

int32_t ProxySomScreenEffectUnknown(RECT* screenArea, uint32_t interp, uint32_t param_3)
{
	// Log it
	// std::ostringstream out1;
	// out1 << "Effect Area: { left = " << screenArea->left << ", top = " << screenArea->top << ", right = " << screenArea->right << ", bottom = " << screenArea->bottom << " }";
	// LogFWrite(out1.str(), "SomScreenEffect>SomScreenEffectUnknown");

	// std::ostringstream out2;
	// out2 << "Effect Misc: { interp = " << interp << ", param_3 = " << param_3 << " }";
	// LogFWrite(out2.str(), "SomScreenEffect>SomScreenEffectUnknown");

	return ProxiedSomScreenEffectUnknown(screenArea, interp, param_3);
}

//
// Unsealer
//
void __cdecl UnsealScreenEffectInit()
{
	DetourAttach(&(PVOID&)ProxiedSomScreenEffectRender, ProxySomScreenEffectRender);
	DetourAttach(&(PVOID&)ProxiedSomScreenEffectUpdate, ProxySomScreenEffectUpdate);
	DetourAttach(&(PVOID&)ProxiedSomScreenEffectCreate, ProxySomScreenEffectCreate);
}

void __cdecl UnsealScreenEffectKill()
{
	DetourDetach(&(PVOID&)ProxiedSomScreenEffectRender, ProxySomScreenEffectRender);
	DetourDetach(&(PVOID&)ProxiedSomScreenEffectUpdate, ProxySomScreenEffectUpdate);
	DetourDetach(&(PVOID&)ProxiedSomScreenEffectCreate, ProxySomScreenEffectCreate);
}

void __cdecl UnsealScreenEffectTick()
{

}