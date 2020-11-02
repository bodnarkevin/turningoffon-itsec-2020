#include "CAFFHandler.h"
#include "CIFFHandler.h"
#include "Logger.h"

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
    int count = 0;
    char temp[4];
    for (int i = index; i < index + 4; i++) {
        temp[count] = static_cast<char>(buffer[i]);
        count++;
    }

    if (temp[0] != 'C' && temp[1] != 'I' && temp[2] != 'F' && temp[3] != 'F') {
        Log::Logger::logMessage("ERROR: CIFF magic word not found.");
        return;
    }

    for (int i = 0; i < 4; i++) {
        result[i] = temp[i];
    }
}

int getCaption(const std::vector<unsigned char>& buffer, int index, std::string& result) {
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

void getTags(const std::vector<unsigned char>& buffer, int index, int headerLength, std::vector<std::string>& result) {
    int idx = index;

    while (idx < index + headerLength) {
        std::string tag = "";
        while (buffer[idx] != '\0') {
            tag += buffer[idx];
            idx++;

        }

        idx++;
        result.push_back(tag);
    }
}

void getPixels(const std::vector<unsigned char>& buffer, int index, int contentLength, std::vector<uint8_t>& result) {
    Log::Logger::logMessage("  Getting pixels ...");

    for (int i = index; i < index + contentLength; i++) {
        result.push_back(static_cast<uint8_t>(buffer[i]));
    }
}

void parseCIFF(std::vector<unsigned char>& buffer, CIFF::CIFFFile& ciff) {
    char magic[4];
    getCIFFMagic(buffer, 0, magic);

    std::string magicString = "    Magic: ";
    for (int i = 0; i < 4; i++) { // debug log
        magicString += magic[i];
    }
    Log::Logger::logMessage(magicString);

    // Remove the parsed 4 bytes from the buffer
    Log::Logger::logBytesProcessed(4);
    std::vector<unsigned char>(buffer.begin() + 4, buffer.end()).swap(buffer);

    int headerLength = convert8ByteToInteger(buffer, 0);

    // Remove the parsed 8 bytes from the buffer
    Log::Logger::logBytesProcessed(8);
    std::vector<unsigned char>(buffer.begin() + 8, buffer.end()).swap(buffer);

    int contentLength = convert8ByteToInteger(buffer, 0);

    // Remove the parsed 8 bytes from the buffer
    Log::Logger::logBytesProcessed(8);
    std::vector<unsigned char>(buffer.begin() + 8, buffer.end()).swap(buffer);

    int width = convert8ByteToInteger(buffer, 0);

    // Remove the parsed 8 bytes from the buffer
    Log::Logger::logBytesProcessed(8);
    std::vector<unsigned char>(buffer.begin() + 8, buffer.end()).swap(buffer);

    int height = convert8ByteToInteger(buffer, 0);

    // Remove the parsed 8 bytes from the buffer
    Log::Logger::logBytesProcessed(8);
    std::vector<unsigned char>(buffer.begin() + 8, buffer.end()).swap(buffer);

    std::string caption = "";
    int captionLength = getCaption(buffer, 0, caption);
    Log::Logger::logMessage("  Caption: " + caption);
    // Remove the parsed captionLength bytes from the buffer
    Log::Logger::logBytesProcessed(captionLength);
    std::vector<unsigned char>(buffer.begin() + captionLength, buffer.end()).swap(buffer);

    std::vector<std::string> tags;
    int tagsLength = headerLength - 4 - 8 - 8 - 8 - 8 - captionLength;
    getTags(buffer, 0, tagsLength, tags);
    std::string tagsMessage = "  Tags: ";
    for (auto element : tags) {
        tagsMessage += "#" + element + " ";
    }
    Log::Logger::logMessage(tagsMessage);
    // Remove the parsed tagsLength bytes from the buffer
    Log::Logger::logBytesProcessed(tagsLength);
    std::vector<unsigned char>(buffer.begin() + tagsLength, buffer.end()).swap(buffer);

    std::vector<uint8_t> pixels;
    getPixels(buffer, 0, contentLength, pixels);
    Log::Logger::logMessage("  Number of pixels: " + std::to_string(pixels.size()));
    // Remove the parsed contentLength bytes from the buffer
    Log::Logger::logBytesProcessed(contentLength);
    std::vector<unsigned char>(buffer.begin() + contentLength, buffer.end()).swap(buffer);

    //not logging pixels
}

void handleHeader(std::vector<unsigned char>& buffer, CAFF::Block& block) {
    std::cout << std::endl << "Handling CAFF header..." << std::endl;
    int length = convert8ByteToInteger(buffer, 0);

    // Remove the parsed 8 bytes from the buffer
    Log::Logger::logBytesProcessed(8);
    std::vector<unsigned char>(buffer.begin() + 8, buffer.end()).swap(buffer);

    block.length = length;

    // Remove the parsed length bytes from the buffer
    Log::Logger::logBytesProcessed(length);
    std::vector<unsigned char>(buffer.begin() + length, buffer.end()).swap(buffer);

    std::cout << "Handled CAFF header" << std::endl << std::endl;

}

void handleCredits(std::vector<unsigned char>& buffer, CAFF::Block& block) {
    std::cout << std::endl << "Handling credits... " << std::endl;
    int length = convert8ByteToInteger(buffer, 0);
    block.length = length;
    int fullCreditsLength = length + 8;

    // Remove the processed fullCreditsLength bytes from the buffer
    Log::Logger::logBytesProcessed(fullCreditsLength);
    std::vector<unsigned char>(buffer.begin() + fullCreditsLength, buffer.end()).swap(buffer);

    std::cout << "Handled credits block" << std::endl << std::endl;
}

void handleAnimation(std::vector<unsigned char>& buffer, CAFF::Block& block) {
    std::cout << std::endl << "Handling animation..." << std::endl;
    int animationLength = convert8ByteToInteger(buffer, 0);
    Log::Logger::logMessage("  Length of animation block: " + std::to_string(animationLength));
    // Remove the parsed 8 bytes from the buffer
    Log::Logger::logBytesProcessed(8);
    std::vector<unsigned char>(buffer.begin()+8, buffer.end()).swap(buffer);

    int duration = convert8ByteToInteger(buffer, 0);
    Log::Logger::logMessage("  Duration of ciff: " + std::to_string(duration) + " ms");
    // Remove the parsed 8 bytes from the buffer
    Log::Logger::logBytesProcessed(8);
    std::vector<unsigned char>(buffer.begin()+8, buffer.end()).swap(buffer);


    CIFF::CIFFFile ciff;
    parseCIFF(buffer, ciff);
    // todo block.animation_data += ciff ?

    std::cout << "Handled animation block" << std::endl << std::endl;
}

void processCAFF(std::vector<unsigned char>& buffer, CAFF::CAFFFile& caffFile) {
    while (buffer.size() > 0) {
        int identifier = static_cast<int>(buffer[0]);

        Log::Logger::logMessage("Parsed block identifier: " + std::to_string(identifier));
        // Remove the processed 1 byte from the buffer
        Log::Logger::logBytesProcessed(1);
        std::vector<unsigned char>(buffer.begin() + 1, buffer.end()).swap(buffer);

        CAFF::Block block;
        CAFF::Header header;
        CAFF::Credits credits;
        CAFF::Animation animation;
        block.id = identifier;

        switch (identifier) {
            case CAFF::BlockType::HEADER:
                block.header_data = &header;
                handleHeader(buffer, block);
                break;
            case CAFF::BlockType::CREDITS:
                block.credits_data = &credits;
                handleCredits(buffer, block);
                break;
            case CAFF::BlockType::ANIMATION:
                block.animation_data = &animation;
                handleAnimation(buffer, block);
                break;
            default:
                Log::Logger::logMessage("Unknown identifier. " + std::to_string(identifier) + " Stopping...");
                return;
        }
    }

    Log::Logger::logSuccess();

}

int main() {
    std::ifstream source("1.caff", std::ios_base::binary);
    std::vector<unsigned char> buffer(std::istreambuf_iterator<char>(source), {});
    int count = 0;
    for (auto element : buffer) {
        std::cout << "Index: " << count << " " << static_cast<int>(element) << std::endl;
        if (count > 180) {
            break;
        }
        count++;
    }
    std::cout << std::endl;
    CAFF::CAFFFile caffFile;
    processCAFF(buffer, caffFile);

    return 0;
}