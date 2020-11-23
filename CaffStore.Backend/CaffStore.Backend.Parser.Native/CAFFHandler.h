#pragma once

#include "CIFFHandler.h"

#include <stdint.h>
#include <string>

enum BlockType { HEADER = 1, CREDITS = 2, ANIMATION = 3 };

struct InteropHeader {
    uint8_t id;
    uint64_t length;
    const char* magic;
    uint64_t header_size;
    uint64_t num_anim;
};

struct InteropCredits {
    uint8_t id;
    uint64_t length;
    const char* creator;
    uint16_t year;
    uint8_t month;
    uint8_t day;
    uint8_t hour;
    uint8_t minute;
};

struct Header {
    char magic[4];
    uint64_t header_size;
    uint64_t num_anim;
};

struct CaffDate {
    uint16_t year;
    uint8_t month;
    uint8_t day;
    uint8_t hour;
    uint8_t minute;
};

struct Credits {
    CaffDate date;
    uint64_t creator_len;
    std::string creator;
};

struct Animation {
    uint64_t duration;
    CIFFFile ciff_file;
};

struct Block {
    uint8_t id;
    uint64_t length;
    Header header_data;
    Credits credits_data;
    Animation animation_data;
};

struct CAFFFile {
    int count = 0;
    Block* blocks;

    ~CAFFFile() {
        delete[] blocks;
    }
};

namespace CAFF {
    class CAFFHandler {
    public:
        CAFFFile processCAFF(std::vector<unsigned char>& buffer);
        Header handleHeader(std::vector<unsigned char>& buffer, Block& block);
        void getCAFFMagic(std::vector<unsigned char>& buffer, char* result);
        Credits handleCredits(std::vector<unsigned char>& buffer, Block& block);
        Animation handleAnimation(std::vector<unsigned char>& buffer);
        bool verifyNumAnim(const CAFFFile& caffFile);
    };

} // namespace CAFF
