#pragma once

#include <stdint.h>
#include <string>

namespace ParserExceptions {

    class ParserException : public std::exception {
        const char* file;
        int line;
        const char* func;
        const char* info;

    public:
        ParserException(const char* msg, const char* file_, int line_, const char* func_);

        const char* get_file() const;
        int get_line() const;
        const char* get_func() const;

    };

} // namespace ParserExceptions