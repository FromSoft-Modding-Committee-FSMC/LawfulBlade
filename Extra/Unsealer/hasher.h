#ifndef _HASHER_H_
#define _HASHER_H_

#include <stdint.h>

extern uint32_t __stdcall HashAsFNV32(const char* str, int32_t length);

#endif
