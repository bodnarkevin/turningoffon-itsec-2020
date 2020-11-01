#include "CAFFHandler.h"
#include "CIFFHandler.h"

#include <iostream>
#include <fstream>
#include <math.h>

int convert8ByteToInteger(const std::vector<unsigned char>& buffer, int index) {
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

void getCIFFMagic(const std::vector<unsigned char>& buffer, int index, char* result) {
    std::cout << "getMagic..." << std::endl;
    int count = 0;
    char temp[4];
    for (int i = index; i < index + 4; i++) {
        temp[count] = static_cast<char>(buffer[i]);
        count++;
    }

    if (temp[0] != 'C' && temp[1] != 'I' && temp[2] != 'F' && temp[3] != 'F') {
        std::cout << "CIFF magic word not found." << std::endl;
        return;
    }

    for (int i = 0; i < 4; i++) {
        result[i] = temp[i];
    }
}

int getCaption(const std::vector<unsigned char>& buffer, int index, std::string& result) {
    std::cout << "getCaption..." << std::endl;
    int size = 0;
    int idx = index;

    while (buffer[idx] != '\n') {
        size++;
        result += buffer[idx];
        idx++;
    }

    size++;
    return size;
}

int getTags(const std::vector<unsigned char>& buffer, int index, int headerLength, std::vector<std::string>& result) {
    std::cout << "getTags" << std::endl;
    int size = 0;
    int idx = index;

    while (idx < index + headerLength) {
        std::string tag = "";
        while (buffer[idx] != '\0') {
            tag += buffer[idx];
            idx++;
            size++;
        }

        idx++;
        size++;

        result.push_back(tag);
    }

    size++;
    return size;
}

void parseCIFF(const std::vector<unsigned char>& buffer, int index, CIFF::CIFFFile& ciff) {
    int idx = index;

    char magic[4];
    getCIFFMagic(buffer, idx, magic);

    std::cout << "Magic: ";
    for (int i = 0; i < 4; i++) { // debug log
        std::cout << magic[i];
    }
    std::cout << std::endl;

    idx += 4;
    int headerLength = convert8ByteToInteger(buffer, idx);
    idx += 8;
    int contentLength = convert8ByteToInteger(buffer, idx);
    idx += 8;
    int width = convert8ByteToInteger(buffer, idx);
    idx += 8;
    int height = convert8ByteToInteger(buffer, idx);
    idx += 8;

    std::string caption = "";
    int captionLength = getCaption(buffer, idx, caption);
    idx += captionLength;
    std::cout << "caption: " << caption << std::endl;

    // todo: tags
    std::vector<std::string> tags;
    int tagsLength = headerLength - 4 - 8 - 8 - 8 - 8 - captionLength;
    idx += getTags(buffer, idx, tagsLength, tags);

    std::cout << "tags:" << std::endl;
    for (auto element : tags) {
        std::cout << "#" << element << std::endl;
    }

    std::cout << "position: " << idx << std::endl;
}

int handleHeader(const std::vector<unsigned char>& buffer, int index, CAFF::Block& block) {
    std::cout << "Handling CAFF header..." << std::endl;
    block.length = convert8ByteToInteger(buffer, index);
    return convert8ByteToInteger(buffer, index) + 8;
}

int handleCredits(const std::vector<unsigned char>& buffer, int index, CAFF::Block& block) {
    std::cout << "Handling credits..." << std::endl;
    block.length = convert8ByteToInteger(buffer, index);
    return convert8ByteToInteger(buffer, index) + 8;
}

int handleAnimation(const std::vector<unsigned char>& buffer, int index, CAFF::Block& block) {
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

void processCAFF(const std::vector<unsigned char>& buffer, CAFF::CAFFFile& caffFile) {
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