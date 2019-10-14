# Lowest version with C# support is 3.8.
# Lowest version that supports VS 2019 is 3.14.
CMAKE_MINIMUM_REQUIRED(VERSION 3.14)
PROJECT(Example LANGUAGES CSharp CXX)

# Set the C# language version for all projects.
SET(CMAKE_CSharp_FLAGS "/langversion:6")

# Explicitly set the `PlatformTarget` for C# projects, since AnyCPU can result in 
# System.BadImageFormatException throws, when trying to load C++/CLI assemblies.
IF(CMAKE_GENERATOR_PLATFORM STREQUAL "x64")
  SET(CMAKE_CSharp_FLAGS "/platform:x64")
ELSEIF(CMAKE_GENERATOR_PLATFORM STREQUAL "x86")
    SET(CMAKE_CSharp_FLAGS "/platform:x86")
ELSE()
  MESSAGE(WARNING "Generator platform is set to '${CMAKE_GENERATOR_PLATFORM}', which is not supported by managed projects. Defaulting to 'AnyCPU'...")
  SET(CMAKE_CSharp_FLAGS "/platform:AnyCPU")
ENDIF()

# Set the C++ standard for all projects.
SET(CMAKE_CXX_STANDARD 17)

# Include sub-projects.
ADD_SUBDIRECTORY(CommonLib)
ADD_SUBDIRECTORY(CppCliLib)
ADD_SUBDIRECTORY(CSharpLib)
ADD_SUBDIRECTORY(WinFormsApp)
ADD_SUBDIRECTORY(WpfApp)