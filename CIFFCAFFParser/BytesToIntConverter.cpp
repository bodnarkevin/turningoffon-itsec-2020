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

} // namespace Converter