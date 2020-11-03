#pragma once

#include "CIFFHandler.h"

#include <stdint.h>
#include <string>

namespace CAFF {

enum BlockType { HEADER = 1, CREDITS = 2, ANIMATION = 3};

struct Header {
    char magic[4];
    uint64_t header_size;
    uint64_t num_anim;
};

struct Date {
    uint8_t year[2];
    uint8_t month;
    uint8_t day;
    uint8_t hour;
    uint8_t minute;
};

struct Credits {
    Date date;
    uint64_t creator_len;
    std::string creator;
};

struct Animation {
    uint64_t duration;
    CIFF::CIFFFile ciff_file;
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
};

class CAFFHandler {
public:
    CAFFFile processCAFF(std::vector<unsigned char>& buffer, CAFF::CAFFFile& caffFile);
private:
    Header handleHeader(std::vector<unsigned char>& buffer, CAFF::Block& block);
    Credits handleCredits(std::vector<unsigned char>& buffer, CAFF::Block& block);
    Animation handleAnimation(std::vector<unsigned char>& buffer, CAFF::Block& block);
};

} // namespace CAFF