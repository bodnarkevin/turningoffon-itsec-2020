#pragma once

#include <stdint.h>
#include <string>
#include <vector>

namespace Converter {

class BytesToIntConverter {
    public:
        int convert8BytesToInteger(const std::vector<unsigned char>& buffer, int index);
    };

} // namespace Converter