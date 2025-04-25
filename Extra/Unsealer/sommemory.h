#ifndef _SOMMEMORY_H_
#define _SOMMEMORY_H_

#include <stdint.h>

//
// Func Types
//
typedef void*(__cdecl* SomMemoryAllocateT)(int32_t);

//
// Proxied
//
extern SomMemoryAllocateT ProxiedSomMemoryAllocate;

//
// Unsealer
//
extern void __cdecl UnsealMemoryInit();
extern void __cdecl UnsealMemoryKill();

#endif