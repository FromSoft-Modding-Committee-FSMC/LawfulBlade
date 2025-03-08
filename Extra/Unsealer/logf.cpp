#include "logf.h"

std::ofstream logFile;

void LogFInit()
{
	logFile.open("unsealer.log");
}

void LogFWrite(const std::string& message, const std::string& src)
{
    if (!logFile.is_open())
        return;

    // Get Log Time
    std::time_t t = std::time(nullptr);
    std::tm timeStruct;

    if (localtime_s(&timeStruct, &t) != 0)
        return;
    
    char timeStr[100];
    std::strftime(timeStr, sizeof(timeStr), "%H:%M:%S", &timeStruct);

    logFile << "<[" << timeStr << "]:[" << src << "]> " << message << std::endl;
}