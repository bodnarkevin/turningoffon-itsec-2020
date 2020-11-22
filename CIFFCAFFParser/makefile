all: Main.exe CAFFParser.dll

check: Main.exe
		valgrind --leak-check=full --error-exitcode=1 ./Main.exe

lodepng.o: lodepng.cpp
		g++ -I include -DBDLL -c lodepng.cpp

Logger.o: Logger.cpp
		g++ -I include -DBDLL -c Logger.cpp

CIFFHandler.o: CIFFHandler.cpp
		g++ -I include -DBDLL -c CIFFHandler.cpp

CAFFHandler.o: CAFFHandler.cpp
		g++ -I include -DBDLL -c CAFFHandler.cpp

BytesToIntConverter.o: BytesToIntConverter.cpp
		g++ -I include -DBDLL -c BytesToIntConverter.cpp

ParserExceptions.o: ParserExceptions.cpp
		g++ -I include -DBDLL -c ParserExceptions.cpp

Main.exe: Main.o Logger.o CIFFHandler.o CAFFHandler.o BytesToIntConverter.o ParserExceptions.o lodepng.o
		g++ -o Main.exe Main.o Logger.o CIFFHandler.o CAFFHandler.o BytesToIntConverter.o ParserExceptions.o lodepng.o

Main.o: Main.cpp
		g++ -I include -DBDLL -c Main.cpp

CAFFParser.dll: Main.o Logger.o CIFFHandler.o CAFFHandler.o BytesToIntConverter.o ParserExceptions.o lodepng.o
		g++ -shared -o CAFFParser.dll Main.o Logger.o CIFFHandler.o CAFFHandler.o BytesToIntConverter.o ParserExceptions.o lodepng.o

clean:
		rm Main.exe Main.o Logger.o CIFFHandler.o CAFFHandler.o BytesToIntConverter.o ParserExceptions.o lodepng.o
