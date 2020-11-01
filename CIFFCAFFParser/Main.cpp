#include "CAFFHandler.h"
#include "CIFFHandler.h"

#include <iostream>
#include <fstream>
#include <math.h>

int convert8ByteToInteger(std::vector<unsigned char> buffer, int index) {
    if (buffer.size() < index + 8) {
        std::cout << "Size is wrong " << buffer.size() << std::endl;
        return 0;
    }

    std::vector<int> lengthVector(8);
    int count = 0;
    for (int i = index; i < index + 8; i++) {
       lengthVector[count] = static_cast<int>(buffer[i]);
       count++;
    }

    const int valami = 256;
    int result = lengthVector[0] * pow(valami, 0) + lengthVector[1] * pow(valami, 1) + lengthVector[2] * pow(valami, 2) + lengthVector[3] * pow(valami, 3)
               + lengthVector[4] * pow(valami, 4) + lengthVector[5] * pow(valami, 5) + lengthVector[6] * pow(valami, 6) + lengthVector[7] * pow(valami, 7);

    return result;
}


void parseCIFF(std::vector<unsigned char> buffer, int index, CIFF::CIFFFile& ciff) {
   int idx = index + 4; // CIFF characters skipped for now
   int headerLength = convert8ByteToInteger(buffer, idx);
   idx += 8;
   int contentLength = convert8ByteToInteger(buffer, idx);
   idx += 8;
   int width = convert8ByteToInteger(buffer, idx);
   idx += 8;
   int height = convert8ByteToInteger(buffer, idx);
   idx += 8;
   std::cout << "position: " << idx << std::endl;
   // todo: caption (variable length text ending with \n)
   // todo: tags
}

int handleHeader(std::vector<unsigned char> buffer, int index, CAFF::Block& block) {
    std::cout << "Handling CAFF header..." << std::endl;
    block.length = convert8ByteToInteger(buffer, index);
    return convert8ByteToInteger(buffer, index) + 8;
}

int handleCredits(std::vector<unsigned char> buffer, int index, CAFF::Block& block) {
    std::cout << "Handling credits..." << std::endl;
    block.length = convert8ByteToInteger(buffer, index);
    return convert8ByteToInteger(buffer, index) + 8;
}

int handleAnimation(std::vector<unsigned char> buffer, int index, CAFF::Block& block) {
    std::cout << "Handling animation..." << std::endl;
    int animationLength = convert8ByteToInteger(buffer, index);
    index += 8;
    int duration = convert8ByteToInteger(buffer, index);
    index += 8;
    std::cout << "   Duration of ciff: " << duration << " ms" << std::endl;
    CIFF::CIFFFile ciff;
    parseCIFF(buffer, index, ciff);
    // todo block.animation_data += ciff ?
    return animationLength += 8;
}

void processCAFF(std::vector<unsigned char> buffer, CAFF::CAFFFile& caffFile) {
    int index = 0;
    while (index < buffer.size()) {
        int identifier = static_cast<int>(buffer[index]);
        int jump = 1;

        CAFF::Block block;
        CAFF::Header header;
        CAFF::Credits credits;
        CAFF::Animation animation;
        block.id = identifier;

        switch (identifier) {
            case CAFF::BlockType::HEADER:
                block.header_data = &header;
                jump = handleHeader(buffer, index + 1, block);
                break;
            case CAFF::BlockType::CREDITS:
                block.credits_data = &credits;
                jump = handleCredits(buffer, index + 1, block);
                break;
            case CAFF::BlockType::ANIMATION:
                block.animation_data = &animation;
                jump = handleAnimation(buffer, index + 1, block);
                break;
            default:
                std::cout << "Unknown identifier. Stopping..." << std::endl;
                return;
        }
        index += jump + 1;
    }
    std::cout << "Process was succesful" << std::endl;
}

int main() {
    std::ifstream source("1.caff", std::ios_base::binary);
    std::vector<unsigned char> buffer(std::istreambuf_iterator<char>(source), {});
    int count = 0;
    for (auto element : buffer) {
        std::cout << "count: " << count << " " << static_cast<int>(element) << std::endl;
        if (count > 180) {
            break;
        }
        count++;
    }
    CAFF::CAFFFile caffFile;
    processCAFF(buffer, caffFile);

    return 0;
}