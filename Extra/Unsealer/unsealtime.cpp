#include <Windows.h>
#include <detours.h>
#include <ostream>
#include <chrono>
#include <stdint.h>

#include "unsealconf.h"
#include "unseallog.h"

#include "unsealtime.h"


/**
 * Data
**/
uint16_t(&m_timerValues)[4] = *(uint16_t(*)[4])0x01d19c80;
bool& m_timersArePaused     = *(bool*)0x01d19c88;
uint32_t& m_timeSincePlay   = *(uint32_t*)0x01d19c8c;
uint32_t& m_timeSinceEpoch  = *(uint32_t*)0x01d19c90;
bool& m_playtimeIsPaused    = *(bool*)0x01d19c94;

/**
 * Function Type Definition
**/
typedef void (__cdecl* SomTimeReset)();

/**
 * Function Hooking
**/
SomTimeReset ProxiedSomTimeReset = (SomTimeReset)0x0043ed50;

/**
 * Function Proxies
**/
void ProxySomTimeReset()
{
	// Get seconds since epoch
	std::chrono::system_clock::time_point now = std::chrono::system_clock::now();
	std::chrono::system_clock::duration epoch = now.time_since_epoch();
	std::chrono::seconds seconds = std::chrono::duration_cast<std::chrono::seconds>(epoch);
	m_timeSinceEpoch = (uint32_t)seconds.count();

	// Reset Timers
	for (int i = 0; i < 4; ++i)
		m_timerValues[i] = 0;
	m_timersArePaused = false;

	// Reset game time
	m_playtimeIsPaused = false;
}

/**
 * Unsealer
**/
void __cdecl UnsealTimeInit()
{
	/**
	 * Initial timer init which SoM fails to do...
	**/
	m_timeSinceEpoch = 0;
	m_timeSincePlay  = 0;
	for (int i = 0; i < 4; ++i)
		m_timerValues[i] = 0;

	m_timersArePaused = false;
	m_playtimeIsPaused = false;

	DetourAttach(&(PVOID&)ProxiedSomTimeReset, ProxySomTimeReset);
}

void __cdecl UnsealTimeKill()
{ 
	DetourDetach(&(PVOID&)ProxiedSomTimeReset, ProxySomTimeReset);
}
