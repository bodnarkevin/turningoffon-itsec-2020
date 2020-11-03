#include "BytesToIntConverter.h"
#include <iostream>
#include <math.h>

namespace Converter {

    int BytesToIntConverter::convert8BytesToInteger(const std::vector<unsigned char>& buffer) {
        if (buffer.size() < 8) {
            std::cout << "ERROR while parsing integer: Buffer too small " << buffer.size() << std::endl;
            throw "Buffer too small";
        }

        std::vector<int> lengthVector(8);
        for (int i = 0; i < 8; i++) {
            lengthVector[i] = static_cast<int>(buffer[i]);
        }

        const int multiplier = 256;
        int result = lengthVector[0] * pow(multiplier, 0) + lengthVector[1] * pow(multiplier, 1) + lengthVector[2] * pow(multiplier, 2) + lengthVector[3] * pow(multiplier, 3)
                + lengthVector[4] * pow(multiplier, 4) + lengthVector[5] * pow(multiplier, 5) + lengthVector[6] * pow(multiplier, 6) + lengthVector[7] * pow(multiplier, 7);

        return result;
    }

} // namespace Converter