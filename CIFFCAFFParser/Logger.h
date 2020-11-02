#pragma once

#include <stdint.h>
#include <string>
namespace Log {
    class Logger {
    public:
        static void logMessage(std::string message);
        static void logBytesProcessed(int numberOfBytesProcessed);
        static void logSuccess();
    };
} // namespace Log