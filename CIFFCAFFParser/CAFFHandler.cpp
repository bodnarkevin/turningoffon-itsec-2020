#include "CAFFHandler.h"
#include "CAFFApi.h"
#include "Logger.h"
#include <iostream>
#include "BytesToIntConverter.h"
#include "ParserExceptions.h"
#include <fstream>
#include <cstring>

using namespace ParserExceptions;

namespace CAFF {

Credits CAFFHandler::handleCredits(std::vector<unsigned char>& buffer, CAFF::Block& block) {
    Converter::BytesToIntConverter bytesToIntConverter;
    Credits credits;
    std::string creator_name = "";

    credits.date.year = bytesToIntConverter.convert2BytesToInteger(buffer);
    credits.date.month = bytesToIntConverter.convert1ByteToInteger(buffer);
    credits.date.day = bytesToIntConverter.convert1ByteToInteger(buffer);
    credits.date.hour = bytesToIntConverter.convert1ByteToInteger(buffer);
    credits.date.minute = bytesToIntConverter.convert1ByteToInteger(buffer);

    if(credits.date.year <= 0){
        throw ParserException("ERROR: Wrong year format", "CAFFHandler", __LINE__, __FUNCTION__);
    }
    if(credits.date.month < 1 || credits.date.month > 12 ){
        throw ParserException("ERROR: Wrong month format", "CAFFHandler", __LINE__, __FUNCTION__);
    }
    if(credits.date.day < 1 || credits.date.day > 31){
        throw ParserException("ERROR: Wrong day format", "CAFFHandler", __LINE__, __FUNCTION__);
    }
    if(credits.date.hour > 23){
        throw ParserException("ERROR: Wrong hour format", "CAFFHandler", __LINE__, __FUNCTION__);
    }
    if(credits.date.minute > 59){
        throw ParserException("ERROR: Wrong minute format", "CAFFHandler", __LINE__, __FUNCTION__);
    }


    credits.creator_len = bytesToIntConverter.convert8BytesToInteger(buffer);

    for(int idx = 0; idx < credits.creator_len; idx++) {
        creator_name += buffer[idx];
    }
    credits.creator = creator_name;

    std::vector<unsigned char>(buffer.begin() + credits.creator_len, buffer.end()).swap(buffer);

    return credits;
}

Animation CAFFHandler::handleAnimation(std::vector<unsigned char>& buffer) {
    Animation animation;
    Converter::BytesToIntConverter bytesToIntConverter;

    int duration = bytesToIntConverter.convert8BytesToInteger(buffer);

    CIFF::CIFFHandler ciffHandler;
    CIFF::CIFFFile ciff = ciffHandler.parseCIFF(buffer);
    animation.duration = duration;
    animation.ciff_file = ciff;

    return animation;
}

void CAFFHandler::getCAFFMagic(std::vector<unsigned char>& buffer, char* result) {
    char temp[4];
    int count = 0;

    if (buffer.size() < 4) {
        std::string message = "ERROR while parsing CAFF magic: Buffer is too small! " + std::to_string(buffer.size()) + ". ";
        throw ParserException(message.c_str(), "CAFFHandler", __LINE__, __FUNCTION__);
    }

    for (int i = 0; i < 4; i++) {
        temp[count] = static_cast<char>(buffer[i]);
        count++;
    }

    if (temp[0] != 'C' || temp[1] != 'A' || temp[2] != 'F'|| temp[3] != 'F') {
        throw ParserException("ERROR: CAFF magic word not found.", "CAFFHandler", 65, "getCAFFMagic");
    }

    for (int i = 0; i < 4; i++) {
        result[i] = temp[i];
    }

    // Remove the parsed 4 bytes from the buffer
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
    return header;
}

CAFFFile CAFFHandler::processCAFF(std::vector<unsigned char>& buffer) {
    CAFFFile caffFile;
    std::vector<Block> blocks;
    Converter::BytesToIntConverter bytesToIntConverter;
    bool headerBlockParsed = false;
    bool creditsBlockParsed = false;

    while (!buffer.empty()) {
        int identifier = static_cast<int>(buffer[0]);

        std::vector<unsigned char>(buffer.begin() + 1, buffer.end()).swap(buffer);
        int length = bytesToIntConverter.convert8BytesToInteger(buffer);

        CAFF::Block block;
        block.id = identifier;
        block.length = length;

        switch (identifier) {
            case CAFF::BlockType::HEADER:
                if (!headerBlockParsed) {
                    block.header_data = handleHeader(buffer, block);
                    headerBlockParsed = true;
                    break;
                } else {
                    std::string message = "ERROR while parsing HEADER: HEADER already parsed.";
                    throw ParserException(message.c_str(), "CAFFHandler", __LINE__, __FUNCTION__);
                }
            case CAFF::BlockType::CREDITS:
                if (!creditsBlockParsed) {
                    block.credits_data = handleCredits(buffer, block);
                    creditsBlockParsed = true;
                    break;
                } else {
                    std::string message = "ERROR while parsing CREDITS: CREDITS already parsed.";
                    throw ParserException(message.c_str(), "CAFFHandler", __LINE__, __FUNCTION__);
                }
            case CAFF::BlockType::ANIMATION:
                block.animation_data = handleAnimation(buffer);
                break;
            default:
                std::string message = "ERROR while parsing integer: Unkown identifier." + std::to_string(identifier);
                throw ParserException(message.c_str(), "CAFFHandler", __LINE__, __FUNCTION__);
        }

        blocks.push_back(block);
    }

    caffFile.blocks = new CAFF::Block[blocks.size()];
    caffFile.count = blocks.size();

    for (int i = 0; i < caffFile.count; i++) { // fill up blocks from vector
        caffFile.blocks[i] = blocks[i];
    }

    if (!verifyNumAnim(caffFile)) {
        throw ParserException("ERROR: Animation count missmatch.", "CAFFHandler", 150, "processCAFF");
    }

    if (!buffer.empty()){
        throw ParserException("ERROR: All data parsed, but buffer not empty", "Main", __LINE__, __FUNCTION__);
    }

    return caffFile;
}

bool CAFFHandler::verifyNumAnim(const CAFFFile& caffFile) {
    int total_anim = 0;
    int anim_count = 0;

    for (int i = 0; i < caffFile.count; i++) {
        if (caffFile.blocks[i].id == static_cast<uint8_t>(1)) {
            total_anim += caffFile.blocks[i].header_data.num_anim;
        } else if (caffFile.blocks[i].id == static_cast<uint8_t>(3)) {
            anim_count++;
        }
    }

    return total_anim == anim_count;
}

} // namespace CAFF

static void addToJson(std::string& str, const std::string& attr, const std::string& value, bool last) {
    str.append("\"" + attr + "\": ");
    if (!last) {
        str.append(value + ",");
    }
    else {
        str.append(value);
    }
}

char* parseToJson(unsigned char* pArray, int nSize, unsigned char** data, int* size, bool* isError) {
    CAFF::CAFFFile caffFile;
    try {
        std::vector<unsigned char> buffer;
        for (int i = 0; i < nSize; i++) buffer.push_back(pArray[i]);

        CAFF::CAFFHandler caffHandler;
        caffFile = caffHandler.processCAFF(buffer);
    }
    catch (const ParserException& e) {
        *isError = true;
        char* errorMessage = new char[e.get_message().size()];
        memcpy(errorMessage, e.get_message().c_str(), e.get_message().size());
        data = nullptr;
        size = 0;

        return errorMessage;
    }

    *isError = false;

    //To JSON
    std::string str_magic = "";
    std::string str = "";
    std::string creator = "";
    int count = 0;
    CAFF::CaffDate date;
    bool hasPrew = false;
    CIFF::CIFFFile ciff;

    for (int i = 0; i < caffFile.count; i++) {
        if (caffFile.blocks[i].id == 2) {
            creator = caffFile.blocks[i].credits_data.creator;
            date = caffFile.blocks[i].credits_data.date;
        }
        else if (caffFile.blocks[i].id == 3 && !hasPrew) {
            ciff = caffFile.blocks[i].animation_data.ciff_file;
            hasPrew = true;
        }
    }

    str.append("{");
    addToJson(str, "creator", "\"" + creator + "\"", false);
    addToJson(str, "creation", "\"" +
        std::to_string(date.year) +
        "-" +
        (date.month < 10 ? "0" + std::to_string(date.month) : std::to_string(date.month)) +
        "-" +
        (date.day < 10 ? "0" + std::to_string(date.day) : std::to_string(date.day)) +
        "T" + std::to_string(date.hour) + ":" + std::to_string(date.minute) + ":00" + "\"", false);

    str.append("\"animations\": [");

    for (int i = 0; i < caffFile.count; i++) {
        if (caffFile.blocks[i].id == 3) {
            str.append("{");
            addToJson(str, "order", std::to_string(count), false);
            addToJson(str, "duration", std::to_string(caffFile.blocks[i].animation_data.duration), false);
            count++;
            str.append("\"ciffData\":");
            str.append("{");
            addToJson(str, "width", std::to_string(caffFile.blocks[i].animation_data.ciff_file.header.width), false);
            addToJson(str, "height", std::to_string(caffFile.blocks[i].animation_data.ciff_file.header.height), false);
            addToJson(str, "caption", "\"" + caffFile.blocks[i].animation_data.ciff_file.header.caption + "\"", false);
            str.append("\"tags\": [");
            for (auto element : caffFile.blocks[i].animation_data.ciff_file.header.tags) {
                str.append("\"" + element + "\",");
            }
            str.append("]");
            str.append("},");
            str.append("},");
        }
    }
    str.append("]");
    str.append("}");


    //Interop prew data
    auto sizePrev = ciff.pixels.size() * sizeof(uint8_t);
    *size = ciff.pixels.size();
    *data = new uint8_t[sizePrev];
    memcpy(*data, ciff.pixels.data(), sizePrev);

    const char* array = str.c_str();
    unsigned long ulSize = strlen(array) + sizeof(char);
    char* pszReturn = new char[ulSize];
    // Copy the contents of szSampleString
    // to the memory pointed to by pszReturn.
    memcpy(pszReturn, array, ulSize);
    // Return pszReturn.

    delete[] caffFile.blocks;
    return pszReturn;
}