#pragma once

#include "CIFFHandler.h"

#include <stdint.h>
#include <string>

namespace CAFF {

enum BlockType { HEADER = 1, CREDITS = 2, ANIMATION = 3};

struct Header {
    char magic[4];
    uint64_t header_size = 0;
    uint64_t num_anim = 0;
};

struct CaffDate {
    uint16_t year = 0;
    uint8_t month = 0;
    uint8_t day = 0;
    uint8_t hour = 0;
    uint8_t minute = 0;
};

struct Credits {
    CaffDate date;
    uint64_t creator_len = 0;
    std::string creator;
};

struct Animation {
    uint64_t duration = 0;
    CIFF::CIFFFile ciff_file;
};

struct Block {
    uint8_t id = 0;
    uint64_t length = 0;
    Header header_data;
    Credits credits_data;
    Animation animation_data;
};

struct CAFFFile {
    int count = 0;
    Block* blocks;
};

class CAFFHandler {
public:
    CAFFFile processCAFF(std::vector<unsigned char>& buffer);
    Header handleHeader(std::vector<unsigned char>& buffer, CAFF::Block& block);
    void getCAFFMagic(std::vector<unsigned char>& buffer, char* result);
    Credits handleCredits(std::vector<unsigned char>& buffer, CAFF::Block& block);
    Animation handleAnimation(std::vector<unsigned char>& buffer);
    bool verifyNumAnim(const CAFFFile& caffFile);
};

} // namespace CAFF