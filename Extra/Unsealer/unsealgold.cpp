#include <Windows.h>
#include <ostream>
#include <detours.h>

#include "unsealconf.h"
#include "unseallog.h"
#include "unsealmemory.h"
#include "unsealplayer.h"

#include "unsealgold.h"

//
// Defines
//
#define GOLD_DROP_UNUSED 0x00
#define GOLD_DROP_ACTIVE 0x02
#define GOLD_DROP_PICKED 0x03

/**
 * Data
**/
int32_t m_somMaxGoldInstanceCount = 32;		// The maximum number of gold drops which can be present.
SomGoldInstance* m_goldDropInstances = (SomGoldInstance*)0x004c1228;
uint32_t** m_goldMeshInstances = (uint32_t**)0x004c162c;

/**
 * Function Type Definition
**/
typedef uint32_t(__cdecl* SomGoldSpawnCoin)(float*, short);
typedef void(__cdecl* SomGoldTryPickup)();

/**
 * Function Hooking
**/
SomGoldSpawnCoin ProxiedSomGoldSpawnCoin = (SomGoldSpawnCoin)0x00403cf0;
SomGoldTryPickup ProxiedSomGoldTryPickup = (SomGoldTryPickup)0x00403ee0;

/**
 * Function Proxies
**/
uint32_t __cdecl ProxySomGoldSpawnCoin(float* position, short value)
{
	SomGoldInstance* foundGoldInst = &m_goldDropInstances[0];
	uint32_t* foundGoldMesh = m_goldMeshInstances[0];

	// Find an empty (or oldest) gold instance and mesh
	for (int i = 0; i < m_somMaxGoldInstanceCount; ++i)
	{ 
		// First check for state == 0.
		if (m_goldDropInstances[i].state == GOLD_DROP_UNUSED)
		{
			foundGoldInst = &m_goldDropInstances[i];
			foundGoldMesh = m_goldMeshInstances[i];
			break;
		}

		// If it's not zero, then we should check if the current one is older or newer...
		if (foundGoldInst->s32x04 > m_goldDropInstances[i].s32x04)
		{
			foundGoldInst = &m_goldDropInstances[i];
			foundGoldMesh = m_goldMeshInstances[i];
		}	
	}

	// Now we can set up our coin inst
	foundGoldInst->value = value;
	foundGoldInst->state = 2;
	foundGoldInst->position.x = position[0];
	foundGoldInst->position.y = position[1];
	foundGoldInst->position.z = position[2];	
	foundGoldInst->s32x14 = 0;
	foundGoldInst->s32x18 = 0;
	foundGoldInst->s32x1c = 0;

	// Edit - we are going to store the spawn time in this field
	foundGoldInst->s32x04 = timeGetTime();

	// Update the position of the mesh to reflect the coin
	memcpy((foundGoldMesh + 0x0D), position + 0, 4);
	memcpy((foundGoldMesh + 0x0E), position + 1, 4);
	memcpy((foundGoldMesh + 0x0F), position + 2, 4);
	memcpy((foundGoldMesh + 0x13), &foundGoldInst->s32x14, 4);	// Pointless, but we best make sure we do it anyhow
	memcpy((foundGoldMesh + 0x14), &foundGoldInst->s32x18, 4);
	memcpy((foundGoldMesh + 0x15), &foundGoldInst->s32x1c, 4);
	foundGoldMesh[0x1F] = 0x00;

	return 0;
}

void __cdecl ProxySomGoldTryPickup()
{
	// Loop Over every single gold instance...
	for (int i = 0; i < m_somMaxGoldInstanceCount; ++i)
	{
		// Grab the current gold instance...
		SomGoldInstance* current = &m_goldDropInstances[i];

		// Make sure this is an active instance
		if (current->state == GOLD_DROP_UNUSED || current->state > GOLD_DROP_ACTIVE)
			continue;

		// Skip the gold if we're too far from it.
		if (VectorDistance3f(&current->position, &g_somPlayerPosition) > g_gameConfigCurrency.pickupRange)
			continue;

		// Add Gold to the player
		g_somPlayerGold += current->value;
		if (g_somPlayerGold > 999999)
			g_somPlayerGold = 999999;

		// Mark gold to fade out
		current->state = GOLD_DROP_PICKED;

		// Queue up a message
		// std::ostringstream out;
		// out << current->value << "COIN OBTAINED";

		//ProxiedSomMessageQueuePush(out.str().c_str(), 0);

		if (g_gameConfigCurrency.pickupMultiple == false)
			break;
	}
}

/**
 * Unsealer
**/
void __cdecl UnsealGoldInit()
{
	DetourAttach(&(PVOID&)ProxiedSomGoldSpawnCoin, ProxySomGoldSpawnCoin);
	DetourAttach(&(PVOID&)ProxiedSomGoldTryPickup, ProxySomGoldTryPickup);
}

void __cdecl UnsealGoldKill()
{
	DetourDetach(&(PVOID&)ProxiedSomGoldSpawnCoin, ProxySomGoldSpawnCoin);
	DetourDetach(&(PVOID&)ProxiedSomGoldTryPickup, ProxySomGoldTryPickup);
}