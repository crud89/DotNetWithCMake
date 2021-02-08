#include "CppClass.h"
#include <UnmanagedClass.h>

using namespace Example;

String^ CppCliClass::SayHello() {
    return "I am a C++/CLI class!";
}

int CppCliClass::AnswerEverything() {
    return CUnmanagedClass::getInt();
}