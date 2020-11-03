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

struct CIFFFile {
    Header* header;
    std::vector<uint8_t> pixels;
};

class CIFFHandler {
    public:
        void parseCIFF(std::vector<unsigned char>& buffer, CIFF::CIFFFile& ciff);
    private:
        void getCaption(const std::vector<unsigned char>& buffer, int index, std::string& result);
        void getTags(const std::vector<unsigned char>& buffer, int index, int headerLength, std::vector<std::string>& result);
        void getPixels(const std::vector<unsigned char>& buffer, int index, int contentLength, std::vector<uint8_t>& result);
        void getCIFFMagic(const std::vector<unsigned char>& buffer, int index, char* result);
};

} // namespace CIFF