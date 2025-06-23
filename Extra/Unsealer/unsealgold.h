#ifndef _UNSEALGOLD_H
#define _UNSEALGOLD_H

#include <stdint.h>
#include "unsealmath.h"

typedef struct
{
	int8_t state;
	int8_t s8x01;
	int16_t value;
	int32_t s32x04;
	VECTOR3F position;
	int32_t s32x14;
	int32_t s32x18;
	int32_t s32x1c;
} SomGoldInstance;

extern void __cdecl UnsealGoldInit();
extern void __cdecl UnsealGoldKill();

#endif