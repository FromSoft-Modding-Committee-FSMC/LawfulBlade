#ifndef _SOMCONF_H
#define _SOMCONF_H

#include <stdint.h>
#include <string>

extern void LoadUserConfiguration();
extern uint32_t GetUserConfigU32(const char* fieldName);
extern uint32_t GetUserConfigBool(const char* fieldName);

extern void LoadGameConfiguration();
extern std::string GetGameConfigString(const char* fieldName);
extern uint32_t GetGameConfigBool(const char* fieldName);
extern int32_t GetGameConfigInteger(const char* fieldName);

#endif