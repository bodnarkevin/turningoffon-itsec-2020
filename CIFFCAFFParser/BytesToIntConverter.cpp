#include "BytesToIntConverter.h"
#include <iostream>
#include <math.h>

namespace Converter {

    int BytesToIntConverter::convert8BytesToInteger(const std::vector<unsigned char>& buffer, int index) {
        if (buffer.size() < index + 8) {
            std::cout << "Size is wrong " << buffer.size() << std::endl;
            return 0;
        }

        std::vector<int> lengthVector(8);
        int count = 0;
        for (int i = index; i < index + 8; i++) {
        lengthVector[count] = static_cast<int>(buffer[i]);
        count++;
        }

        const int valami = 256;
        int result = lengthVector[0] * pow(valami, 0) + lengthVector[1] * pow(valami, 1) + lengthVector[2] * pow(valami, 2) + lengthVector[3] * pow(valami, 3)
                + lengthVector[4] * pow(valami, 4) + lengthVector[5] * pow(valami, 5) + lengthVector[6] * pow(valami, 6) + lengthVector[7] * pow(valami, 7);

        return result;
    }

} // namespace Converter