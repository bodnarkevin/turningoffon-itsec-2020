#include "pch.h"
#include "Logger.h"
#include <iostream>

namespace Log {

    void Logger::logMessage(std::string message) {
        std::cout << message << std::endl;
    }

    void Logger::logBytesProcessed(int numberOfBytesProcessed) {
        std::string bytesString = " byte";
        if (numberOfBytesProcessed > 1) {
            bytesString += "s";
        }
        bytesString += ".";
        std::cout << "      Processed " << numberOfBytesProcessed << bytesString << std::endl;
    }

    void Logger::logSuccess() {
        std::cout << std::endl << "+--------------------------------------------------+" << std::endl;
        std::cout << "|                                                  |" << std::endl;
        std::cout << "|              Process was succesful               |" << std::endl;
        std::cout << "|                                                  |" << std::endl;
        std::cout << "+--------------------------------------------------+" << std::endl;
    }

} // namespace Log