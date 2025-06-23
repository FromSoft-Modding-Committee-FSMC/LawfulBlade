#include <Windows.h>
#include <detours.h>
#include <ostream>

#include "unsealconf.h"
#include "unseallog.h"

#include "unsealmemory.h"

/**
 * Function Hooking
**/
SomMemoryAllocateBlock ProxiedSomMemoryAllocateBlock = (SomMemoryAllocateBlock)0x00401500;

/**
 * Unsealer
**/
void __cdecl UnsealMemoryInit()
{

}

void __cdecl UnsealMemoryKill()
{

}