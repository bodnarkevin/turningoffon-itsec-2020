all: main.exe

logger.o: Logger.cpp
		g++ -c Logger.cpp

ciffhandler.o: CIFFHandler.cpp
		g++ -c CIFFHandler.cpp

caffhandler.o: CAFFHandler.cpp
		g++ -c CAFFHandler.cpp

bytestointconverter.o: BytesToIntConverter.cpp
		g++ -c BytesToIntConverter.cpp

main.exe: main.o logger.o ciffhandler.o caffhandler.o bytestointconverter.o
		g++ -o main.exe main.o logger.o ciffhandler.o caffhandler.o bytestointconverter.o

main.o: Main.cpp
		g++ -c Main.cpp

clean:
		rm main.o main.exe