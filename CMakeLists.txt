# Lowest version with C# support is 3.8.
# Lowest version that supports VS 2019 is 3.14.
CMAKE_MINIMUM_REQUIRED(VERSION 3.14)
PROJECT(Example LANGUAGES CSharp CXX)

# Set the C# language version for all projects.
SET(CMAKE_CSharp_FLAGS "/langversion:6")

# Set the C++ standard for all projects.
SET(CMAKE_CXX_STANDARD 17)

# Include sub-projects.
ADD_SUBDIRECTORY(CommonLib)
ADD_SUBDIRECTORY(CppCliLib)
ADD_SUBDIRECTORY(CSharpLib)
ADD_SUBDIRECTORY(WinFormsApp)
ADD_SUBDIRECTORY(WpfApp)