#pragma once

#include <stdint.h>
#include <string>
#include <vector>

namespace Converter {

class BytesToIntConverter {
    public:
        int convert8BytesToInteger(std::vector<unsigned char>& buffer);
        uint16_t convert2BytesToInteger(std::vector<unsigned char>& buffer);
        uint8_t convert1ByteToInteger(std::vector<unsigned char>& buffer);
    };

} // namespace Converter