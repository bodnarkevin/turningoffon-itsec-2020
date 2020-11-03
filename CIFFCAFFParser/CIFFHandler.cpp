#include "CIFFHandler.h"
#include <iostream>
#include "BytesToIntConverter.h"

#include "Logger.h"

namespace CIFF {

    CIFFFile CIFFHandler::parseCIFF(std::vector<unsigned char>& buffer, CIFF::CIFFFile& ciff) {
        Converter::BytesToIntConverter bytesToIntConverter;
        char magic[4];
        getCIFFMagic(buffer, magic);

        std::string magicString = "    Magic: ";
        for (int i = 0; i < 4; i++) { // debug log
            magicString += magic[i];
        }
        Log::Logger::logMessage(magicString);

        // Remove the parsed 4 bytes from the buffer
        Log::Logger::logBytesProcessed(4);
        std::vector<unsigned char>(buffer.begin() + 4, buffer.end()).swap(buffer);

        int headerLength = bytesToIntConverter.convert8BytesToInteger(buffer, 0);

        // Remove the parsed 8 bytes from the buffer
        Log::Logger::logBytesProcessed(8);
        std::vector<unsigned char>(buffer.begin() + 8, buffer.end()).swap(buffer);

        int contentLength = bytesToIntConverter.convert8BytesToInteger(buffer, 0);

        // Remove the parsed 8 bytes from the buffer
        Log::Logger::logBytesProcessed(8);
        std::vector<unsigned char>(buffer.begin() + 8, buffer.end()).swap(buffer);

        int width = bytesToIntConverter.convert8BytesToInteger(buffer, 0);

        // Remove the parsed 8 bytes from the buffer
        Log::Logger::logBytesProcessed(8);
        std::vector<unsigned char>(buffer.begin() + 8, buffer.end()).swap(buffer);

        int height = bytesToIntConverter.convert8BytesToInteger(buffer, 0);

        // Remove the parsed 8 bytes from the buffer
        Log::Logger::logBytesProcessed(8);
        std::vector<unsigned char>(buffer.begin() + 8, buffer.end()).swap(buffer);

        int captionLength = 0;
        std::string caption = getCaption(buffer, captionLength);
        // int ll = caption.length() + 1;
        Log::Logger::logMessage("  Caption: " + caption + "   " + std::to_string(captionLength));
        // Remove the parsed captionLength bytes from the buffer (+1 for the \n)
        Log::Logger::logBytesProcessed(captionLength);
        std::vector<unsigned char>(buffer.begin() + captionLength, buffer.end()).swap(buffer);

        // header - magic(the 4 bytes) - headerlength(the 8 bytes that represents this length) - height(the 8 bytes) - contentlength(the 8 bytes) - width(the 8 bytes)
        int tagsLength = headerLength - 4 - 8 - 8 - 8 - 8 - captionLength;
        std::vector<std::string> tags = getTags(buffer, tagsLength);
        std::string tagsMessage = "  Tags: ";

        for (auto element : tags) {
            tagsMessage += "#" + element + " ";
        }
        Log::Logger::logMessage(tagsMessage);
        // Remove the parsed tagsLength bytes from the buffer
        Log::Logger::logBytesProcessed(tagsLength);
        std::vector<unsigned char>(buffer.begin() + tagsLength, buffer.end()).swap(buffer);

        std::vector<uint8_t> pixels = getPixels(buffer, contentLength);
        Log::Logger::logMessage("  Number of pixels: " + std::to_string(pixels.size()));
        // Remove the parsed contentLength bytes from the buffer
        Log::Logger::logBytesProcessed(contentLength);
        std::vector<unsigned char>(buffer.begin() + contentLength, buffer.end()).swap(buffer);

        // Not logging pixels...
        Header ciffHeader;
        for (int i = 0; i < 4; i++)
        {
            ciffHeader.magic[i] = magic[i];
        }
        ciffHeader.header_size = headerLength;
        ciffHeader.content_size = contentLength;
        ciffHeader.caption = caption;
        ciffHeader.height = height;
        ciffHeader.width = width;
        ciffHeader.tags = tags;
        ciff.header = ciffHeader;
        ciff.pixels = pixels;
        return ciff;
    }

    std::vector<uint8_t> CIFFHandler::getPixels(const std::vector<unsigned char>& buffer, int contentLength) {
        Log::Logger::logMessage("  Getting pixels ...");

        std::vector<uint8_t> result;
        for (int i = 0; i < contentLength; i++) {
            result.push_back(static_cast<uint8_t>(buffer[i]));
        }

        return result;
    }

    std::vector<std::string> CIFFHandler::getTags(const std::vector<unsigned char>& buffer, int headerLength) {
        int idx = 0;
        std::vector<std::string> result;
        while (idx < headerLength) {
            std::string tag = "";
            while (buffer[idx] != '\0') {
                tag += buffer[idx];
                idx++;

            }

            idx++;
            result.push_back(tag);
        }

        return result;
    }

    void CIFFHandler::getCIFFMagic(const std::vector<unsigned char>& buffer, char* result) {
        char temp[4];
        int count = 0;
        for (int i = 0; i < 4; i++) {
            temp[count] = static_cast<char>(buffer[i]);
            count++;
        }

        if (temp[0] != 'C' && temp[1] != 'I' && temp[2] != 'F' && temp[3] != 'F') {
            Log::Logger::logMessage("ERROR: CIFF magic word not found.");
            throw "ERROR: CIFF magic word not found.";
        }

        for (int i = 0; i < 4; i++) {
            result[i] = temp[i];
        }
    }

    std::string CIFFHandler::getCaption(const std::vector<unsigned char>& buffer, int& captionLength){
        int idx = 0;
        std::string result = "";

        while (buffer[idx] != '\n') {
            result += buffer[idx];
            idx++;
            captionLength++;
        }

        captionLength++;
        return result;
    }

} // namespace CIFF
