#ifndef _UNSEALMEMORY_H_
#define _UNSEALMEMORY_H_

#include <stdint.h>

/**
 * Function Type Definition
**/
typedef void*(__cdecl* SomMemoryAllocateBlock)(int32_t);

/**
 * Function Hooking
**/
extern SomMemoryAllocateBlock ProxiedSomMemoryAllocateBlock;

/**
 * Unsealer
**/
extern void __cdecl UnsealMemoryInit();
extern void __cdecl UnsealMemoryKill();

#endif