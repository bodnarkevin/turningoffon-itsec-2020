#include "CAFFHandler.h"
#include "CIFFHandler.h"

#include <iostream>
#include <fstream>
#include <math.h>

int convertLength(std::vector<unsigned char> buffer, int index) {
    if (buffer.size() < index + 8) {
        std::cout << "Size is wrong " << buffer.size() << std::endl;
        return 0;
    }

    std::vector<int> lengthVector(8);
    int count = 0;
    for (int i = index; i < index + 8; i++) {
       lengthVector[count] = static_cast<int>(buffer[i]);
       count++;
    }

    const int valami = 256;
    int result = lengthVector[0] * pow(valami, 0) + lengthVector[1] * pow(valami, 1) + lengthVector[2] * pow(valami, 2) + lengthVector[3] * pow(valami, 3)
               + lengthVector[4] * pow(valami, 4) + lengthVector[5] * pow(valami, 5) + lengthVector[6] * pow(valami, 6) + lengthVector[7] * pow(valami, 7);

    return result;
}

int handleHeader(std::vector<unsigned char> buffer, int index) {
    std::cout << "Handling header" << std::endl;
    return convertLength(buffer, index) + 8;
}

int handleCredits(std::vector<unsigned char> buffer, int index) {
    std::cout << "Handling credits " << std::endl;
    return convertLength(buffer, index) + 8;
}

int handleAnimation(std::vector<unsigned char> buffer, int index) {
    std::cout << "Handling animation" << std::endl;
    return convertLength(buffer, index) + 8;
}

void processCAFF(std::vector<unsigned char> buffer) {
    int index = 0;
    while (index < buffer.size()) {
        int identifier = static_cast<int>(buffer[index]);
        int jump = 1;
        switch (identifier) {
            case CAFF::BlockType::HEADER:
                jump = handleHeader(buffer, index + 1);
                break;
            case CAFF::BlockType::CREDITS:
                jump = handleCredits(buffer, index + 1);
                break;
            case CAFF::BlockType::ANIMATION:
                jump = handleAnimation(buffer, index + 1);
                break;
            default:
                std::cout << "Unknown identifier. Stopping..." << std::endl;
                return;
        }
        index += jump + 1;
    }
    std::cout << "Process was succesful" << std::endl;
}

int main() {
   std::ifstream source("1.caff", std::ios_base::binary);
   std::vector<unsigned char> buffer(std::istreambuf_iterator<char>(source), {});
   int count = 0;
   for (auto element : buffer) {
      std::cout << "count: " << count << " " << static_cast<int>(element) << std::endl;
      if (count > 180) {
         break;
      }
      count++;
   }

    processCAFF(buffer);

   // Feldolgozas
   // >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
//    std::cout << "--------------------------------------------------" << std::endl;

//    int firstID = static_cast<int>(buffer.at(0));
//    int firstSize = static_cast<int>(buffer.at(1));

//     int convertedLength = convertLength(buffer, 1);
//     std::cout << "convertedLength " << convertedLength << std::endl;

//    const int lengthFieldSize = 8;
//    int i = lengthFieldSize + 1;
//    for (i; i < firstSize + lengthFieldSize + 1; i++) {
//       std::cout << "***: " << i << "      " << static_cast<int>(buffer.at(i)) << std::endl;
//    }
//    int nextId = static_cast<int>(buffer.at(i));
//    i++;
//    int nextSize = static_cast<int>(buffer.at(i));
//    int current = i + lengthFieldSize;
//    i = i + lengthFieldSize;
//    std::cout << "henloooo " << current << "     " << nextSize << std::endl;
//    for (i; i < current + nextSize + 1; i++) {
//       std::cout << "*hhh**: " << i << "      " << static_cast<int>(buffer.at(i)) << std::endl;
//    }

//    std::cout << "kövi     : " << static_cast<int>(buffer.at(i)) << std::endl;
   // <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
   return 0;
}