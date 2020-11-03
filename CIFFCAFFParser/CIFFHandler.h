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
    Header header;
    std::vector<uint8_t> pixels;
};

class CIFFHandler {
    public:
        CIFFFile parseCIFF(std::vector<unsigned char>& buffer);
    private:
        std::string getCaption(std::vector<unsigned char>& buffer, int& captionLength);
        std::vector<std::string> getTags(std::vector<unsigned char>& buffer, int headerLength);
        std::vector<uint8_t> getPixels(std::vector<unsigned char>& buffer, int contentLength);
        void getCIFFMagic(std::vector<unsigned char>& buffer, char* result);
};

} // namespace CIFF