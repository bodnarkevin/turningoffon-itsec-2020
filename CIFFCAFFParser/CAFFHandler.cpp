#include "CAFFHandler.h"
#include "Logger.h"
#include <iostream>
#include "BytesToIntConverter.h"

namespace CAFF {

    Credits CAFFHandler::handleCredits(std::vector<unsigned char>& buffer, CAFF::Block& block) {
        // TODO: Anetta & Soma & Kevin: fill credits with actual data
        Credits credits;
        
        Converter::BytesToIntConverter bytesToIntConverter;
        std::cout << std::endl << "Handling credits... " << std::endl;
        int length = bytesToIntConverter.convert8BytesToInteger(buffer);
        block.length = length;
        int fullCreditsLength = length + 8;

        // Remove the processed fullCreditsLength bytes from the buffer
        Log::Logger::logBytesProcessed(fullCreditsLength);
        std::vector<unsigned char>(buffer.begin() + fullCreditsLength, buffer.end()).swap(buffer);

        std::cout << "Handled credits block" << std::endl << std::endl;
        return credits;
    }

    Animation CAFFHandler::handleAnimation(std::vector<unsigned char>& buffer, CAFF::Block& block) {
        Animation animation;
        Converter::BytesToIntConverter bytesToIntConverter;
        std::cout << std::endl << "Handling animation..." << std::endl;
        int animationLength = bytesToIntConverter.convert8BytesToInteger(buffer);
        Log::Logger::logMessage("  Length of animation block: " + std::to_string(animationLength));
        // Remove the parsed 8 bytes from the buffer
        Log::Logger::logBytesProcessed(8);
        std::vector<unsigned char>(buffer.begin()+8, buffer.end()).swap(buffer);

        int duration = bytesToIntConverter.convert8BytesToInteger(buffer);
        Log::Logger::logMessage("  Duration of ciff: " + std::to_string(duration) + " ms");
        // Remove the parsed 8 bytes from the buffer
        Log::Logger::logBytesProcessed(8);
        std::vector<unsigned char>(buffer.begin()+8, buffer.end()).swap(buffer);


        CIFF::CIFFFile ciff;
        CIFF::CIFFHandler ciffHandler;
        ciffHandler.parseCIFF(buffer, ciff);
        // todo block.animation_data += ciff ?

        std::cout << "Handled animation block" << std::endl << std::endl;
        return animation;
    }

    Header CAFFHandler::handleHeader(std::vector<unsigned char>& buffer, CAFF::Block& block) {
        // TODO: Anetta & Soma & Kevin: fill credits with actual data
        Header header;

        Converter::BytesToIntConverter bytesToIntConverter;
        std::cout << std::endl << "Handling CAFF header..." << std::endl;
        int length = bytesToIntConverter.convert8BytesToInteger(buffer);

        // Remove the parsed 8 bytes from the buffer
        Log::Logger::logBytesProcessed(8);
        std::vector<unsigned char>(buffer.begin() + 8, buffer.end()).swap(buffer);

        block.length = length;

        // Remove the parsed length bytes from the buffer
        Log::Logger::logBytesProcessed(length);
        std::vector<unsigned char>(buffer.begin() + length, buffer.end()).swap(buffer);

        std::cout << "Handled CAFF header" << std::endl << std::endl;
        return header;
    }

    CAFFFile CAFFHandler::processCAFF(std::vector<unsigned char>& buffer, CAFF::CAFFFile& caffFile) {
        std::vector<CAFF::Block> blocks;
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
                    header = handleHeader(buffer, block);
                    block.header_data = header;
                    break;
                case CAFF::BlockType::CREDITS:
                    credits = handleCredits(buffer, block);
                    block.credits_data = credits;
                    break;
                case CAFF::BlockType::ANIMATION:
                    animation = handleAnimation(buffer, block);

                    block.animation_data = animation;
                    break;
                default:
                    Log::Logger::logMessage("Unknown identifier. " + std::to_string(identifier) + " Stopping...");
                    throw "Could not parse identifier!";
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