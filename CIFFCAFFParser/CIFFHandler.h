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
        CIFFFile parseCIFF(std::vector<unsigned char>& buffer, CIFF::CIFFFile& ciff);
    private:
        std::string getCaption(const std::vector<unsigned char>& buffer, int& captionLength);
        std::vector<std::string> getTags(const std::vector<unsigned char>& buffer, int headerLength);
        std::vector<uint8_t> getPixels(const std::vector<unsigned char>& buffer, int contentLength);
        void getCIFFMagic(const std::vector<unsigned char>& buffer, char* result);
};

} // namespace CIFF