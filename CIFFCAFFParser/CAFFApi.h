#pragma once
#include <ocidl.h>

#ifdef __linux__
/**
* @param tempDir: Path to the CAFF file to be parsed
* @param previewDir: Path where we write the preview image parsed from the CAFF file
* @param jsonDir: Path to write the JSON file to, containing caption and tags
*/
extern "C" 
{
	char* parseToJson(BYTE* pArray, int nSize, unsigned char** data, int* size, bool* isError);
}
#elif _WIN32

#ifdef BUILD_DLL
#define CAFFPARSER_API __declspec(dllexport)
#else 
#define CAFFPARSER_API __declspec(dllimport)
#endif
/**
* @param tempDir: Path to the CAFF file to be parsed
* @param previewDir: Path where we write the preview image parsed from the CAFF file
* @param jsonDir: Path to write the JSON file to, containing caption and tags
*/
extern "C" 
{
	CAFFPARSER_API char* parseToJson(BYTE* pArray, int nSize, unsigned char** data, int* size, bool* isError);
}
#endif