#include "CAFFHandler.h"
#include "CIFFHandler.h"

#include <iostream>
#include <fstream>

int main() {
    std::ifstream source("1.caff", std::ios_base::binary);
    std::vector<unsigned char> buffer(std::istreambuf_iterator<char>(source), {});
    int count = 0;
    for (auto element : buffer) {
        std::cout << "count: " << count << " " << element << std::endl;
        count++;
    }
    return 0;
}