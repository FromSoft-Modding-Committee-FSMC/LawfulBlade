#ifndef _LOGF_H_
#define _LOGF_H_

#include <fstream>
#include <iostream>
#include <sstream>
#include <string>

extern void LogFInit();
extern void LogFWrite(const std::string& message, const std::string& src);

#endif
