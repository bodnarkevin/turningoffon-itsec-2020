all: main.exe

logger.o: Logger.cpp
		g++ -c Logger.cpp

main.exe: main.o logger.o
		g++ -o main.exe main.o logger.o

main.o: Main.cpp
		g++ -c Main.cpp

clean:
		rm main.o main.exe