CC = g++ -std=c++11
CFLAGS = -shared -fPIC
WINFLAG = -DBUILD_DLL
SOURCE = CAFFHandler.cpp Logger.cpp CIFFHandler.cpp BytesToIntConverter.cpp ParserExceptions.cpp lodepng.cpp

check: Main.exe
	valgrind --leak-check=full --error-exitcode=1 ./Main.exe

build-dll:
	mkdir Library
	$(CC) $(WINFLAG) $(CFLAGS) $(SOURCE) -o Library\CaffParser.dll

clean-dll:
	del Library\CaffParser.dll

build-so:
	mkdir -p  Library
	$(CC) $(CFLAGS) $(SOURCE) -o Library/CaffParser.so

clean-so:
	rm -f Library/CaffParser.so

build-test:
	mkdir -p  Library
	$(CC) $(CFLAGS) $(SOURCE) -o Library/LibCaff.so

clean-test:
	rm -f Library/LibCaff.so

Main.exe:
	g++ $(SOURCE) Main.cpp -o Main.exe

clean:
	rm Main.exe
