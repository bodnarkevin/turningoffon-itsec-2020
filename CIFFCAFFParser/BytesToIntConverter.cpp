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

        const int valami = 256;
        int result = lengthVector[0] * pow(valami, 0) + lengthVector[1] * pow(valami, 1) + lengthVector[2] * pow(valami, 2) + lengthVector[3] * pow(valami, 3)
                + lengthVector[4] * pow(valami, 4) + lengthVector[5] * pow(valami, 5) + lengthVector[6] * pow(valami, 6) + lengthVector[7] * pow(valami, 7);

        return result;
    }

} // namespace Converter