all: Main.exe

lodepng.o: lodepng.cpp
		g++ -c lodepng.cpp

Logger.o: Logger.cpp
		g++ -c Logger.cpp

CIFFHandler.o: CIFFHandler.cpp
		g++ -c CIFFHandler.cpp

CAFFHandler.o: CAFFHandler.cpp
		g++ -c CAFFHandler.cpp

BytesToIntConverter.o: BytesToIntConverter.cpp
		g++ -c BytesToIntConverter.cpp

ParserExceptions.o: ParserExceptions.cpp
		g++ -c ParserExceptions.cpp

Main.exe: Main.o Logger.o CIFFHandler.o CAFFHandler.o BytesToIntConverter.o ParserExceptions.o lodepng.o
		g++ -o Main.exe Main.o Logger.o CIFFHandler.o CAFFHandler.o BytesToIntConverter.o ParserExceptions.o lodepng.o

Main.o: Main.cpp
		g++ -c Main.cpp

clean:
		rm Main.exe Main.o Logger.o CIFFHandler.o CAFFHandler.o BytesToIntConverter.o ParserExceptions.o lodepng.o
