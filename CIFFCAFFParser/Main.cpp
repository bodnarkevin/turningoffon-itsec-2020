#include "CAFFHandler.h"
#include "CIFFHandler.h"
#include "Logger.h"

#include <iostream>
#include <fstream>
#include <math.h>
#include "BytesToIntConverter.h"

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
    CAFF::CAFFHandler::processCAFF(buffer, caffFile);

    return 0;
}