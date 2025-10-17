#ifndef _UNSEALGDI_H_
#define _UNSEALGDI_H_

#include <string>
#include <stdint.h>

// Struct is used for handling fonts in our game configuration
typedef struct
{
	std::string typeface;
	int32_t     weight;
} SomFontConfiguration;

// Our logic
extern void __cdecl UnsealGdiInit();
extern void __cdecl UnsealGdiKill();


#endif