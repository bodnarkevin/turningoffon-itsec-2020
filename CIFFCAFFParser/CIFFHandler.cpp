#include "CIFFHandler.h"
#include <iostream>
#include "BytesToIntConverter.h"

#include "Logger.h"

namespace CIFF {

    void CIFFHandler::parseCIFF(std::vector<unsigned char>& buffer, CIFF::CIFFFile& ciff) {
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

        int headerLength = Converter::BytesToIntConverter::convert8BytesToInteger(buffer, 0);

        // Remove the parsed 8 bytes from the buffer
        Log::Logger::logBytesProcessed(8);
        std::vector<unsigned char>(buffer.begin() + 8, buffer.end()).swap(buffer);

        int contentLength = Converter::BytesToIntConverter::convert8BytesToInteger(buffer, 0);

        // Remove the parsed 8 bytes from the buffer
        Log::Logger::logBytesProcessed(8);
        std::vector<unsigned char>(buffer.begin() + 8, buffer.end()).swap(buffer);

        int width = Converter::BytesToIntConverter::convert8BytesToInteger(buffer, 0);

        // Remove the parsed 8 bytes from the buffer
        Log::Logger::logBytesProcessed(8);
        std::vector<unsigned char>(buffer.begin() + 8, buffer.end()).swap(buffer);

        int height = Converter::BytesToIntConverter::convert8BytesToInteger(buffer, 0);

        // Remove the parsed 8 bytes from the buffer
        Log::Logger::logBytesProcessed(8);
        std::vector<unsigned char>(buffer.begin() + 8, buffer.end()).swap(buffer);

        std::string caption = "";
        getCaption(buffer, 0, caption);
        int captionLength = caption.length();
        Log::Logger::logMessage("  Caption: " + caption);
        // Remove the parsed captionLength bytes from the buffer
        Log::Logger::logBytesProcessed(captionLength);
        std::vector<unsigned char>(buffer.begin() + captionLength, buffer.end()).swap(buffer);

        std::vector<std::string> tags;
        int tagsLength = headerLength - 4 - 8 - 8 - 8 - 8 - captionLength;
        getTags(buffer, 0, tagsLength, tags);
        std::string tagsMessage = "  Tags: ";
        for (auto element : tags) {
            std::string tag = "#" + element + " ";
            tagsMessage += tag;
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

        // Not logging pixels...
    }

    void CIFFHandler::getPixels(const std::vector<unsigned char>& buffer, int index, int contentLength, std::vector<uint8_t>& result) {
        Log::Logger::logMessage("  Getting pixels ...");

        for (int i = index; i < index + contentLength; i++) {
            result.push_back(static_cast<uint8_t>(buffer[i]));
        }
    }

    void CIFFHandler::getTags(const std::vector<unsigned char>& buffer, int index, int headerLength, std::vector<std::string>& result) {
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

    void CIFFHandler::getCIFFMagic(const std::vector<unsigned char>& buffer, int index, char* result) {
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

    void CIFFHandler::getCaption(const std::vector<unsigned char>& buffer, int index, std::string& result){
        int idx = index;

        while (buffer[idx] != '\n') {
            result += buffer[idx];
            idx++;
        }
    }

} // namespace CIFF
