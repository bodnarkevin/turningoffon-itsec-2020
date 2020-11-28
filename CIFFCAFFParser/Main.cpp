#include "CAFFHandler.h"
#include "CIFFHandler.h"
#include "Logger.h"
#include "CAFFApi.h"

#include <iostream>
#include <fstream>
#include <math.h>
#include "BytesToIntConverter.h"
#include "ParserExceptions.h"
#include <cstring>

using namespace ParserExceptions;

int main() {
    std::ifstream source("1.caff", std::ios_base::binary);
    std::vector<unsigned char> buffer(std::istreambuf_iterator<char>(source), {});

    CAFF::CAFFHandler caffHandler;
    try
    {
        CAFF::CAFFFile caffFile = caffHandler.processCAFF(buffer);

        unsigned char* array = &buffer[0];
        unsigned char* prev = nullptr;
        int prevSize = 0;
        bool error = true;
        char* json = parseToJson(array, buffer.size(), &prev, &prevSize, &error);

        delete[] caffFile.blocks;

        delete[] json;
        delete[] prev;
    }

    catch(const ParserException& e)
    {
        std::cerr << e.what() << '\n';
        std::cerr << "In file:" << e.get_file() << '\n';
        std::cerr << "   Function" << e.get_func() << '\n';
        std::cerr << "   Line" << e.get_line() << '\n';
    }

    return 0;
}