#include "CAFFHandler.h"
#include "CIFFHandler.h"
#include "Logger.h"
#include "CAFFApi.h"

#include <iostream>
#include <fstream>
#include <math.h>
#include "BytesToIntConverter.h"
#include "ParserExceptions.h"

using namespace ParserExceptions;

int main() {
    std::ifstream source("1.caff", std::ios_base::binary);
    std::vector<unsigned char> buffer(std::istreambuf_iterator<char>(source), {});

    //Ez csak debug?
    //int count = 0;
    /*
    for (auto element : buffer) {
        std::cout << "Index: " << count << " " << static_cast<int>(element) << std::endl;
        if (count > 280) {
            break;
        }
        count++;
    }
    std::cout << std::endl;*/

    CAFF::CAFFHandler caffHandler;
    try
    {
        CAFF::CAFFFile caffFile = caffHandler.processCAFF(buffer);

        if(buffer.size() > 0){
             throw ParserException("ERROR: All data parsed, but buffer not empty", "Main", __LINE__, __FUNCTION__);
        }

        // send to C# backend ...
        unsigned char* array = &buffer[0];
        unsigned char* prev = nullptr;
        int prevSize = 0;
        bool error = true;
        char* json = parseToJson(array, buffer.size(), &prev, &prevSize, &error);

        delete[] caffFile.blocks;
        delete[] json;
        delete[] prev;
    }

    catch(const ParserException e)
    {
        std::cerr << e.what() << '\n';
        std::cerr << "In file:" << e.get_file() << '\n';
        std::cerr << "   Function" << e.get_func() << '\n';
        std::cerr << "   Line" << e.get_line() << '\n';
    }

    return 0;
}