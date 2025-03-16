#ifndef _LOGF_H_
#define _LOGF_H_

#include <Windows.h>
#include <fstream>
#include <iostream>
#include <sstream>
#include <string>


// Using a wrap of OutputDebugString for our convinience... Fuck that function.
typedef void(__stdcall* SomOutputDebugStringA)(LPCSTR);

// Original Functions
extern SomOutputDebugStringA ProxiedOutputDebugStringA;

// Proxy Functions
extern void __stdcall ProxyOutputDebugStringA(LPCSTR lpOutputString);

extern void LogFInit();
extern void LogFWrite(const std::string& message, const std::string& src);

#endif
