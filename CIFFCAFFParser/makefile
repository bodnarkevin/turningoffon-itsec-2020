all: main.exe

main.exe: main.o
		g++ -o main.exe main.o

main.o: Main.cpp
		g++ -c Main.cpp

clean:
		rm main.o main.exe