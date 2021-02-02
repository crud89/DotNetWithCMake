PROJECT(CppCliLib VERSION 1.0.0.0 LANGUAGES CXX)

# Collect header & source files.
SET(CppCliLib_HEADERS
    "CppClass.h"
)

SET(CppCliLib_SOURCES
    "CppClass.cpp"
)

# Add shared library project.
ADD_LIBRARY(${PROJECT_NAME} SHARED
    ${CppCliLib_HEADERS}
    ${CppCliLib_SOURCES}
)

# Add a managed dependency.
ADD_DEPENDENCIES(${PROJECT_NAME} CommonLib)

# Optional: Create source groups for better code organization.
SOURCE_GROUP(TREE ${CMAKE_CURRENT_SOURCE_DIR} FILES ${CppCliLib_SOURCES} ${CppCliLib_SOURCES})

# Define the assembly to be mixed-mode (managed + unmanaged).
SET_PROPERTY(TARGET ${PROJECT_NAME} PROPERTY COMMON_LANGUAGE_RUNTIME "")

# Setup project properties.
SET_PROPERTY(TARGET ${PROJECT_NAME} PROPERTY VS_DOTNET_TARGET_FRAMEWORK_VERSION "v4.7.2")
SET_PROPERTY(TARGET ${PROJECT_NAME} PROPERTY VS_DOTNET_REFERENCES
	"System"
    "System.Core"
)

# Setup root namespace for the project.
SET_PROPERTY(TARGET ${PROJECT_NAME} PROPERTY VS_GLOBAL_ROOTNAMESPACE ${PROJECT_NAME})