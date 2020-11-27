#pragma once

#ifdef __linux__
extern "C" 
{
	char* parseToJson(unsigned char* pArray, int nSize, unsigned char** data, int* size, bool* isError);
}
#elif _WIN32

#ifdef BUILD_DLL
#define CAFFPARSER_API __declspec(dllexport)
#else 
#define CAFFPARSER_API __declspec(dllimport)
#endif
extern "C" 
{
	CAFFPARSER_API char* parseToJson(unsigned char* pArray, int nSize, unsigned char** data, int* size, bool* isError);
}
#endif