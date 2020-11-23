#pragma once
#include "CAFFHandler.h"
#include <ocidl.h>

#ifdef CAFF_EXPORTS
#define CAFF_API __declspec(dllexport)
#else
#define CAFF_API __declspec(dllimport)
#endif

extern "C" CAFF_API char* parseToJson(BYTE* pArray, int nSize, unsigned char** data, int* size, bool* isError);