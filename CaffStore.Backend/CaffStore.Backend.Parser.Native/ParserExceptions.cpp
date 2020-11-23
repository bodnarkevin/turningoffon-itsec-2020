#include "pch.h"
#include <iostream>
#include "ParserExceptions.h"


namespace ParserExceptions {

    ParserException::ParserException(const char* msg, const char* file_, int line_, const char* func_) :
        file(file_),
        line(line_),
        func(func_)
    {
        throw std::runtime_error(msg);
    }

    const char* ParserException::get_file() const { return file; }
    int ParserException::get_line() const { return line; }
    const char* ParserException::get_func() const { return func; }


} // namespace ParserExceptions