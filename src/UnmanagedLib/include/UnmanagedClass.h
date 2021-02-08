#pragma once

#if !defined (UNMANAGED_LIB_API)
#  if defined(UnmanagedLib_EXPORTS) && (defined _WIN32 || defined WINCE)
#    define UNMANAGED_LIB_API __declspec(dllexport)
#  elif (defined(UnmanagedLib_EXPORTS) || defined(__APPLE__)) && defined __GNUC__ && __GNUC__ >= 4
#    define UNMANAGED_LIB_API __attribute__ ((visibility ("default")))
#  elif !defined(UnmanagedLib_EXPORTS) && (defined _WIN32 || defined WINCE)
#    define UNMANAGED_LIB_API __declspec(dllimport)
#  endif
#endif

#ifndef UNMANAGED_LIB_API
#  define UNMANAGED_LIB_API
#endif

class UNMANAGED_LIB_API CUnmanagedClass {
public:
    static int getInt() noexcept;
};