#ifndef _SOMMATH_H
#define _SOMMATH_H

#include <math.h>

typedef struct
{
	float x;
	float y;
} VECTOR2F;

typedef struct
{
	float x;
	float y;
	float z;
} VECTOR3F;

extern float VectorDistance2f(VECTOR2F* a, VECTOR2F* b);
extern float VectorDistance3f(VECTOR3F* a, VECTOR3F* b);

#endif