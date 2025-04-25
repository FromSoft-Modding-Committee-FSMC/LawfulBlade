#include "sommath.h"

float VectorDistance2f(VECTOR2F* a, VECTOR2F* b)
{
	float dx = (b->x - a->x);
	float dy = (b->y - a->y);
	return sqrtf(dx * dx + dy * dy);
}

float VectorDistance3f(VECTOR3F* a, VECTOR3F* b)
{
	float dx = (b->x - a->x);
	float dy = (b->y - a->y);
	float dz = (b->z - a->z);
	return sqrtf(dx * dx + dy * dy + dz * dz);
}