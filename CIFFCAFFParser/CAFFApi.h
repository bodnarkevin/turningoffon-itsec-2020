#pragma once

extern "C"
{
	char* parseToJson(unsigned char* pArray, int nSize, unsigned char** data, int* size, bool* isError);
}