#ifndef _SOMMESSAGEQUEUE_H
#define _SOMMESSAGEQUEUE_H

#include <stdint.h>

//
// Func Types
//
typedef int32_t(__cdecl* SomMessageQueuePush)(const char* message, int8_t reinit);

//
// Proxied
//
extern SomMessageQueuePush ProxiedSomMessageQueuePush;

#endif