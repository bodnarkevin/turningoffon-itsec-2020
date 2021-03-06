#include "BytesToIntConverter.h"
#include <iostream>
#include <math.h>
#include "ParserExceptions.h"
#include "Logger.h"

using namespace ParserExceptions;

namespace Converter {

    int BytesToIntConverter::convert8BytesToInteger(std::vector<unsigned char>& buffer) {
        if (buffer.size() < 8) {
            std::string message = "ERROR while parsing integer: Buffer is too small: " + std::to_string(buffer.size()) + "bytes. ";
            throw ParserException(message.c_str(), "BytesToIntConverter", 11, "convert8BytesToInteger");
        }

        std::vector<int> lengthVector(8);
        for (int i = 0; i < 8; i++) {
            lengthVector[i] = static_cast<int>(buffer[i]);
        }

        const int multiplier = 256;
        int result = lengthVector[0] * pow(multiplier, 0) + lengthVector[1] * pow(multiplier, 1) + lengthVector[2] * pow(multiplier, 2) + lengthVector[3] * pow(multiplier, 3)
                + lengthVector[4] * pow(multiplier, 4) + lengthVector[5] * pow(multiplier, 5) + lengthVector[6] * pow(multiplier, 6) + lengthVector[7] * pow(multiplier, 7);

        // Remove the parsed 8 bytes from the buffer
        Log::Logger::logBytesProcessed(8);
        std::vector<unsigned char>(buffer.begin() + 8, buffer.end()).swap(buffer);

        return result;
    }

    uint16_t BytesToIntConverter::convert2BytesToInteger(std::vector<unsigned char>& buffer){
        if (buffer.size() < 2) {
            std::string message = "ERROR while parsing integer: Buffer is too small: " + std::to_string(buffer.size()) + "bytes. ";
            throw ParserException(message.c_str(), "BytesToIntConverter", 11, "convert8BytesToInteger");
        }

        std::vector<int> lengthVector(2);
        for (int i = 0; i < 2; i++) {
            lengthVector[i] = static_cast<int>(buffer[i]);
        }

        const int multiplier = 2;
        int result = lengthVector[0] | lengthVector[1] << 8;
        // Remove the parsed 2 bytes from the buffer
        Log::Logger::logBytesProcessed(2);
        std::vector<unsigned char>(buffer.begin() + 2, buffer.end()).swap(buffer);

        return result;
    }

    uint8_t BytesToIntConverter::convert1ByteToInteger(std::vector<unsigned char>& buffer){
        if (buffer.size() < 1) {
            std::string message = "ERROR while parsing integer: Buffer is too small: " + std::to_string(buffer.size()) + "bytes. ";
            throw ParserException(message.c_str(), "BytesToIntConverter", 11, "convert8BytesToInteger");
        }

        uint8_t result = static_cast<uint8_t>(buffer[0]);
        
        // Remove the parsed 1 bytes from the buffer
        Log::Logger::logBytesProcessed(1);
        std::vector<unsigned char>(buffer.begin() + 1, buffer.end()).swap(buffer);

        return result;
    }

} // namespace Converter