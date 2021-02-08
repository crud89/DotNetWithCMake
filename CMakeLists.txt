# Lowest version with C# support is 3.8.
# Lowest version that supports VS 2019 is 3.14.
CMAKE_MINIMUM_REQUIRED(VERSION 3.14)
PROJECT(Example LANGUAGES CSharp CXX)

# Set global .NET Framework version.
SET(CMAKE_DOTNET_TARGET_FRAMEWORK_VERSION "v4.7.2")

# Set the C# language version for all projects.
SET(CMAKE_CSharp_FLAGS "/langversion:6")

# Explicitly set the `PlatformTarget` for C# projects, since AnyCPU can result in 
# System.BadImageFormatException throws, when trying to load C++/CLI assemblies.
IF(CMAKE_GENERATOR_PLATFORM STREQUAL "x64")
  SET(CMAKE_CSharp_FLAGS "/platform:x64")
ELSEIF(CMAKE_GENERATOR_PLATFORM STREQUAL "Win32")
    SET(CMAKE_CSharp_FLAGS "/platform:x86")
ELSE()
  MESSAGE(WARNING "Generator platform is set to '${CMAKE_GENERATOR_PLATFORM}', which is not supported by managed projects. Defaulting to 'AnyCPU'...")
  SET(CMAKE_CSharp_FLAGS "/platform:AnyCPU")
ENDIF()

# Set the C++ standard for all projects.
SET(CMAKE_CXX_STANDARD 17)

# Define runtime intermediate build directory, so that debugging the Visual Studio solution works as expected.
SET(CMAKE_RUNTIME_OUTPUT_DIRECTORY "${CMAKE_BINARY_DIR}/binaries/")

# Set export directory for CMake config.
SET(CMAKE_INSTALL_BINARY_DIR    "bin/")
SET(CMAKE_INSTALL_LIBRARY_DIR   "bin/")
SET(CMAKE_INSTALL_INCLUDE_DIR   "include/")
SET(CMAKE_INSTALL_EXPORT_DIR    "cmake/")

# Include sub-projects.
ADD_SUBDIRECTORY(src/UnmanagedLib)
ADD_SUBDIRECTORY(src/CommonLib)
ADD_SUBDIRECTORY(src/CppCliLib)
ADD_SUBDIRECTORY(src/CSharpLib)
ADD_SUBDIRECTORY(src/WinFormsApp)
ADD_SUBDIRECTORY(src/WpfApp)