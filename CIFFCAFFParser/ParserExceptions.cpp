#include <iostream>
#include "ParserExceptions.h"


namespace ParserExceptions {

    ParserException::ParserException(const std::string& msg, const char* file_, int line_, const char* func_) :
        message (msg),
        file    (file_),
        line    (line_),
        func    (func_)
    {
    }

} // namespace ParserExceptions