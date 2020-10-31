#include "CAFFHandler.h"
#include "CIFFHandler.h"

#include <iostream>
#include <fstream>

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

   // Feldolgozas
   // >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
   std::cout << "--------------------------------------------------" << std::endl;

   int firstID = static_cast<int>(buffer.at(0));
   int firstSize = static_cast<int>(buffer.at(1));
   const int lengthFieldSize = 8;
   int i = lengthFieldSize + 1;
   for (i; i < firstSize + lengthFieldSize + 1; i++) {
      std::cout << "***: " << i << "      " << static_cast<int>(buffer.at(i)) << std::endl;
   }
   int nextId = static_cast<int>(buffer.at(i));
   i++;
   int nextSize = static_cast<int>(buffer.at(i));
   int current = i + lengthFieldSize;
   i = i + lengthFieldSize;
   std::cout << "henloooo " << current << "     " << nextSize << std::endl;
   for (i; i < current + nextSize + 1; i++) {
      std::cout << "*hhh**: " << i << "      " << static_cast<int>(buffer.at(i)) << std::endl;
   }

   std::cout << "kövi     : " << static_cast<int>(buffer.at(i)) << std::endl;
   // <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
   return 0;
}