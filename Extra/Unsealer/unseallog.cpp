#include <fstream>
#include <sstream>

#include "unseallog.h"

std::ofstream m_logFs;
int m_logLevel = UNSEAL_LOG_LEVEL_NONE;

void UnsealLog(const std::string& message, const std::string& heading, int level)
{
    // Don't log anything when we're not allow
    if (level <= m_logLevel || !m_logFs.is_open())
        return;

    m_logFs << "<[" << heading << "]> " << message << std::endl;
}

void UnsealLoggerInit(int logLevel)
{
    if (logLevel == UNSEAL_LOG_LEVEL_NONE)
        return;

	m_logFs.open("unsealer.log");
    m_logLevel = logLevel;
}

void UnsealLoggerKill()
{
    if (!m_logFs.is_open())
        return;

    m_logFs.flush();
    m_logFs.close();
}