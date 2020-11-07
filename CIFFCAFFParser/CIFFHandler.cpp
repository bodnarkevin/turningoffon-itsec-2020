#include "CIFFHandler.h"
#include <iostream>
#include "BytesToIntConverter.h"
#include "ParserExceptions.h"

#include "Logger.h"

using namespace ParserExceptions;

namespace CIFF {

    // The order of parsing the different parts of the file is relevant, do NOT modify that!
    CIFFFile CIFFHandler::parseCIFF(std::vector<unsigned char>& buffer) {
        CIFF::CIFFFile ciff;
        Converter::BytesToIntConverter bytesToIntConverter;

        std::cout << std::endl << "Handling CIFF content..." << std::endl;

        char magic[4];
        getCIFFMagic(buffer, magic);

        std::string magicString = "    Magic: ";
        for (int i = 0; i < 4; i++) { // debug log
            magicString += magic[i];
        }
        Log::Logger::logMessage(magicString);

        int headerLength = bytesToIntConverter.convert8BytesToInteger(buffer);

        int contentLength = bytesToIntConverter.convert8BytesToInteger(buffer);

        int width = bytesToIntConverter.convert8BytesToInteger(buffer);

        int height = bytesToIntConverter.convert8BytesToInteger(buffer);

        // Validation: content size must be width*heigth*3
        if (contentLength != width * height * 3) {
            std::string message = "ERROR: Invalid CIFF content length! Content length is " + std::to_string(contentLength) + ", but should be " + std::to_string(width * height * 3) + ". ";
            throw ParserException(message.c_str(), "CIFFHandler", 33, "parseCIFF");
        }

        int captionLength = 0;
        std::string caption = getCaption(buffer, captionLength);
        Log::Logger::logMessage("  Caption: " + caption);

        // header - magic(the 4 bytes) - headerlength(the 8 bytes that represents this length) - height(the 8 bytes) - contentlength(the 8 bytes) - width(the 8 bytes)
        int tagsLength = headerLength - 4 - 8 - 8 - 8 - 8 - captionLength;
        std::vector<std::string> tags = getTags(buffer, tagsLength);
        std::string tagsMessage = "  Tags: ";
        for (auto element : tags) {
            tagsMessage += "#" + element + " ";
        }
        Log::Logger::logMessage(tagsMessage);
        
        std::vector<uint8_t> pixels = getPixels(buffer, contentLength);
        Log::Logger::logMessage("  Number of pixels: " + std::to_string(pixels.size()));

        // Validation: If height or with is zero, there should be no pixels present in the file
        if ((width == 0 || height == 0) && pixels.size() > 0) {
            throw ParserException("ERROR: No pixels should be present in the CIFF file! (height or with is zero)", "CIFFHandler", 59, "parseCIFF");
        }

        // Validation: The number of pixels must be equal to the content size
        if (pixels.size() != contentLength) {
            throw ParserException("ERROR: The number of pixels must be equal to the content size!", "CIFFHandler", 64, "parseCIFF");
        }

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

        std::cout << "Handled CIFF content" << std::endl << std::endl;

        return ciff;
    }

    std::vector<uint8_t> CIFFHandler::getPixels(std::vector<unsigned char>& buffer, int contentLength) {
        Log::Logger::logMessage("  Getting pixels ...");

        if (buffer.size() < contentLength) {
            std::string message = "ERROR while parsing CIFF pixels: Buffer is too small! " + std::to_string(buffer.size()) + ". ";
            throw ParserException(message.c_str(), "CIFFHandler", 90, "getPixels");
        }

        std::vector<uint8_t> result;
        for (int i = 0; i < contentLength; i++) {
            result.push_back(static_cast<uint8_t>(buffer[i]));
        }

        // Remove the parsed contentLength bytes from the buffer
        Log::Logger::logBytesProcessed(contentLength);
        std::vector<unsigned char>(buffer.begin() + contentLength, buffer.end()).swap(buffer);

        return result;
    }

    std::vector<std::string> CIFFHandler::getTags(std::vector<unsigned char>& buffer, int headerLength) {

        if (buffer.size() < headerLength) {
            std::string message = "ERROR while parsing CIFF tags: Buffer is too small! " + std::to_string(buffer.size()) + ". ";
            throw ParserException(message.c_str(), "CIFFHandler", 107, "getTags");
        }

        std::vector<std::string> result;
        for(int idx = 0; idx < headerLength; idx++) {
            std::string tag = "";
            while (buffer[idx] != '\0') {
                tag += buffer[idx];
                idx++;

            }
            if(tag.find("\n") != std::string::npos){
                throw ParserException("ERROR: Not allowed multi line tags", "CIFFHandler", 123, "getTags");
            }
            result.push_back(tag);
        }


        // Remove the parsed headerLength bytes from the buffer
        Log::Logger::logBytesProcessed(headerLength);
        std::vector<unsigned char>(buffer.begin() + headerLength, buffer.end()).swap(buffer);

        return result;
    }

    void CIFFHandler::getCIFFMagic(std::vector<unsigned char>& buffer, char* result) {
        char temp[4];
        int count = 0;

        if (buffer.size() < 4) {
            std::string message = "ERROR while parsing CIFF magic: Buffer is too small! " + std::to_string(buffer.size()) + ". ";
            throw ParserException(message.c_str(), "CIFFHandler", 135, "getCIFFMagic");
        }

        for (int i = 0; i < 4; i++) {
            temp[count] = static_cast<char>(buffer[i]);
            count++;
        }

        if (temp[0] != 'C' || temp[1] != 'I' || temp[2] != 'F' || temp[3] != 'F') {
            throw ParserException("ERROR: CIFF magic word not found.", "CIFFHandler", 149, "getCIFFMagic");
        }

        for (int i = 0; i < 4; i++) {
            result[i] = temp[i];
        }

        // Remove the parsed 4 bytes from the buffer
        Log::Logger::logBytesProcessed(4);
        std::vector<unsigned char>(buffer.begin() + 4, buffer.end()).swap(buffer);
    }

    std::string CIFFHandler::getCaption(std::vector<unsigned char>& buffer, int& captionLength){
        int idx = 0;
        std::string result = "";

        while (buffer[idx] != '\n') {
            result += buffer[idx];
            idx++;
            captionLength++;
        }

        captionLength++;

        // Remove the parsed captionLength bytes from the buffer (+1 for the \n)
        Log::Logger::logBytesProcessed(captionLength);
        std::vector<unsigned char>(buffer.begin() + captionLength, buffer.end()).swap(buffer);

        return result;
    }

} // namespace CIFF
