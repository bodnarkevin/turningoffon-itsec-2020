CC = g++ -std=c++11
CFLAGS = -shared -fPIC
WINFLAG = -DBUILD_DLL
SOURCE = CAFFHandler.cpp Logger.cpp CIFFHandler.cpp BytesToIntConverter.cpp ParserExceptions.cpp lodepng.cpp

check: Main.exe
	valgrind --leak-check=full --error-exitcode=1 ./Main.exe

build-dll:
	mkdir -p Library
	$(CC) $(WINFLAG) $(CFLAGS) $(SOURCE) -o Library\CaffParser.dll

clean-dll:
	rm -f Library/CaffParser.dll

build-so:
	mkdir -p  Library
	$(CC) $(CFLAGS) $(SOURCE) -o Library/CaffParser.so

clean-so:
	rm -f Library/CaffParser.so

Main.exe:
	g++ $(SOURCE) Main.cpp -o Main.exe

clean:
	rm Main.exe
