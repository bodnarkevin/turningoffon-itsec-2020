#pragma once

#include <stdint.h>
#include <string>
#include <vector>

namespace Converter {

class BytesToIntConverter {
    public:
        int convert8BytesToInteger(std::vector<unsigned char>& buffer);
    };

} // namespace Converter