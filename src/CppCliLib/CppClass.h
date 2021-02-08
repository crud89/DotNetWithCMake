#pragma once

using namespace System;

namespace Example {

    public ref class CppCliClass : public IHello {
    public:
        virtual String^ SayHello();
        virtual int AnswerEverything();
    };
};