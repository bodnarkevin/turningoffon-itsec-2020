#include "CAFFHandler.h"
#include "Logger.h"
#include <iostream>
#include "BytesToIntConverter.h"
#include "ParserExceptions.h"

using namespace ParserExceptions;

namespace CAFF {

    Credits CAFFHandler::handleCredits(std::vector<unsigned char>& buffer, CAFF::Block& block) {
        Converter::BytesToIntConverter bytesToIntConverter;
        Credits credits;
        uint64_t idx = 0;
        std::string creator_name = "";
        std::cout << std::endl << "Handling credits... " << std::endl;

        credits.date.year = bytesToIntConverter.convert2BytesToInteger(buffer);
        credits.date.month = bytesToIntConverter.convert1ByteToInteger(buffer);
        credits.date.day = bytesToIntConverter.convert1ByteToInteger(buffer);
        credits.date.hour = bytesToIntConverter.convert1ByteToInteger(buffer);
        credits.date.minute = bytesToIntConverter.convert1ByteToInteger(buffer);

        credits.creator_len = bytesToIntConverter.convert8BytesToInteger(buffer);

        while (idx < credits.creator_len) {
            creator_name += buffer[idx];
            idx++;
        }
        credits.creator = creator_name;

        Log::Logger::logMessage("  Date year " + std::to_string(credits.date.year));
        Log::Logger::logMessage("  Creator name " + credits.creator);

        std::vector<unsigned char>(buffer.begin() + credits.creator_len, buffer.end()).swap(buffer);

        std::cout << "Handled credits block" << std::endl << std::endl;
        return credits;
    }

    Animation CAFFHandler::handleAnimation(std::vector<unsigned char>& buffer) {
        Animation animation;
        Converter::BytesToIntConverter bytesToIntConverter;

        std::cout << std::endl << "Handling animation..." << std::endl;

        int duration = bytesToIntConverter.convert8BytesToInteger(buffer);
        Log::Logger::logMessage("  Duration of ciff: " + std::to_string(duration) + " ms");

        CIFF::CIFFHandler ciffHandler;
        CIFF::CIFFFile ciff = ciffHandler.parseCIFF(buffer);
        animation.ciff_file = ciff;

        std::cout << "Handled animation block" << std::endl << std::endl;
        return animation;
    }

        void CAFFHandler::getCAFFMagic(std::vector<unsigned char>& buffer, char* result) {
        char temp[4];
        int count = 0;

        if (buffer.size() < 4) {
            std::string message = "ERROR while parsing CAFF magic: Buffer is too small! " + std::to_string(buffer.size()) + ". ";
            throw ParserException(message.c_str(), "CAFFHandler", 56, __FUNCTION__);
        }

        for (int i = 0; i < 4; i++) {
            temp[count] = static_cast<char>(buffer[i]);
            count++;
        }

        if (temp[0] != 'C' && temp[1] != 'A' && temp[2] != 'F' && temp[3] != 'F') {
            throw ParserException("ERROR: CAFF magic word not found.", "CAFFHandler", 65, "getCAFFMagic");
        }

        for (int i = 0; i < 4; i++) {
            result[i] = temp[i];
        }

        // Remove the parsed 4 bytes from the buffer
        Log::Logger::logBytesProcessed(4);
        std::vector<unsigned char>(buffer.begin() + 4, buffer.end()).swap(buffer);
    }

    Header CAFFHandler::handleHeader(std::vector<unsigned char>& buffer, CAFF::Block& block) {
        Header header;
        char magic[4];
        Converter::BytesToIntConverter bytesToIntConverter;

        /* magic */
        CAFFHandler::getCAFFMagic(buffer,magic);
        for (int i = 0; i < 4; i++)
        {
            header.magic[i] = magic[i];
        }

        header.header_size = bytesToIntConverter.convert8BytesToInteger(buffer);

        header.num_anim = bytesToIntConverter.convert8BytesToInteger(buffer);

        Log::Logger::logMessage("SIZE: " + std::to_string(header.header_size));
        Log::Logger::logMessage("NUM ANIM: " + std::to_string(header.num_anim));
        // todo: content...

        std::cout << "Handled CAFF header" << std::endl << std::endl;
        return header;
    }

    CAFFFile CAFFHandler::processCAFF(std::vector<unsigned char>& buffer) {
        CAFF:CAFFFile caffFile;
        std::vector<CAFF::Block> blocks;
        Converter::BytesToIntConverter bytesToIntConverter;
        
        while (buffer.size() > 0) {
            int identifier = static_cast<int>(buffer[0]);

            Log::Logger::logMessage("Parsed block identifier: " + std::to_string(identifier));
            // Remove the processed 1 byte from the buffer
            Log::Logger::logBytesProcessed(1);
            std::vector<unsigned char>(buffer.begin() + 1, buffer.end()).swap(buffer);

            std::cout << std::endl << "Handling block ..." << std::endl;
            int length = bytesToIntConverter.convert8BytesToInteger(buffer);
            
            CAFF::Block block;
            CAFF::Header header;
            CAFF::Credits credits;
            CAFF::Animation animation;
            block.id = identifier;
            block.length = length;

            switch (identifier) {
                case CAFF::BlockType::HEADER:
                    header = handleHeader(buffer, block);
                    block.header_data = header;
                    break;
                case CAFF::BlockType::CREDITS:
                    credits = handleCredits(buffer, block);
                    block.credits_data = credits;
                    break;
                case CAFF::BlockType::ANIMATION:
                    Log::Logger::logMessage("  Length of animation block: " + std::to_string(block.length));
                    animation = handleAnimation(buffer);
                    block.animation_data = animation;
                    break;
                default:
                    std::string message = "ERROR while parsing integer: Unkown identifier." + std::to_string(identifier);
                    throw ParserException(message.c_str(), "CAFFHandler", 68, "processCAFF");
            }

            blocks.push_back(block);
        }

        caffFile.blocks = new CAFF::Block[blocks.size()];
        caffFile.count = blocks.size();

        Log::Logger::logSuccess();
        Log::Logger::logMessage("Block count: " + std::to_string(blocks.size()));

        return caffFile;
    }

} // namespace CAFF