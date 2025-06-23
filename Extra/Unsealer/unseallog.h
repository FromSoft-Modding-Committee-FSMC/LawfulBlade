#ifndef _UNSEALLOG_H_
#define _UNSEALLOG_H_

#include <string>

// Log levels
enum UNSEAL_LOG_LEVEL : int32_t
{
	UNSEAL_LOG_LEVEL_NONE = 0,
	UNSEAL_LOG_LEVEL_INFO = 1,
	UNSEAL_LOG_LEVEL_WARN = 2,
	UNSEAL_LOG_LEVEL_SHIT = 3,
	UNSEAL_LOG_LEVEL_OOPS = 4
};

extern void UnsealLog(const std::string& message, const std::string& heading, int level);
extern void UnsealLoggerInit(int logLevel);
extern void UnsealLoggerKill();

#endif
