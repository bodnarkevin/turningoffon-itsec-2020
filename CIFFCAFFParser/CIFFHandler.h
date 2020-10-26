#pragma once

#include <stdint.h>
#include <string>
#include <vector>

namespace CIFF {

struct Header {
    char magic[4];
    uint64_t header_size;
    uint64_t content_size;
    uint64_t width;
    uint64_t height;
    std::string caption;
    std::vector<std::string> tags;
};

struct CiffFile {
    Header* header;
    std::vector<uint8_t> pixels;
};

} // namespace CIFF