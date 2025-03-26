#include "hasher.h"

#define FNV_32_BASIS 0x811c9dc5
#define FNV_32_PRIME 0x01000193

uint32_t __stdcall HashAsFNV32(const char* str, int32_t length)
{
	// Make sure length is not zero, and string is not null.
	if (length <= 0 || str == NULL)
		return 0;

	// Store result
	uint32_t result = FNV_32_BASIS;
	
	// Hash the buffer, and return the result.
	do 
	{
		result ^= str[length - 1];
		result *= FNV_32_PRIME;
	} while (--length > 0);

	// Return our calculated hash
	return result;
}