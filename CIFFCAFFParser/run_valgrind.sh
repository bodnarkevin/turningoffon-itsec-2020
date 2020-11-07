#! /bin/sh
make
valgrind --leak-check=full --error-exitcode=-1 ./Main.exe
make clean
