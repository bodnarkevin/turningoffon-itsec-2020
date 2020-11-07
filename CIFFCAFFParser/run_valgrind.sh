#! /bin/sh
make
valgrind --leak-check=yes ./Main.exe
make clean
