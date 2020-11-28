#pragma once

#include <stdint.h>
#include <string>

namespace ParserExceptions {

    class ParserException : public std::exception {
    std::string message;
	const char* file;
    int line;
    const char* func;
    const char* info;
    
    public:
    	ParserException(const std::string& msg, const char* file_, int line_, const char* func_);
        
        const std::string& get_message() const { return message; }
        const char* get_file() const { return file; }
        int get_line() const { return line; }
        const char* get_func() const { return func; }

    };

} // namespace ParserExceptions