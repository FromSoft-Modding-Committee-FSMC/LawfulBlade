#include "sommemory.h"
#include "logf.h"

#include <Windows.h>
#include <ostream>
#include <detours.h>

//
// Proxied
//
SomMemoryAllocateT ProxiedSomMemoryAllocate = (SomMemoryAllocateT)0x00401500;

//
// Unsealer
//
void __cdecl UnsealMemoryInit()
{

}

void __cdecl UnsealMemoryKill()
{

}