PROJECT(CppCliLib VERSION 1.0.0.0 LANGUAGES CXX)

# Collect header & source files.
SET(CppCliLib_HEADERS
    "CppClass.h"
)

SET(CppCliLib_SOURCES
    "CppClass.cpp"
)

# Add shared library project.
# NOTE: Library type is `MODULE`, since no symbols are exported. `SHARED` libraries *must* 
#       always export symbols! (https://cmake.org/cmake/help/latest/command/add_library.html)
ADD_LIBRARY(CppCliLib MODULE
    ${RENDERING_HEADERS}
    ${RENDERING_SOURCES}
)

# Add a managed dependency.
ADD_DEPENDENCIES(CppCliLib CommonLib)

# Optional: Create source groups for better code organization.
SOURCE_GROUP(TREE ${CMAKE_CURRENT_SOURCE_DIR} FILES ${RENDERING_HEADERS} ${RENDERING_SOURCES})

# Define the assembly to be mixed-mode (managed + unmanaged).
SET_PROPERTY(TARGET CppCliLib PROPERTY COMMON_LANGUAGE_RUNTIME "")

# Setup project properties.
SET_PROPERTY(TARGET CppCliLib PROPERTY VS_DOTNET_TARGET_FRAMEWORK_VERSION "v4.7.2")
SET_PROPERTY(TARGET CppCliLib PROPERTY VS_DOTNET_REFERENCES
	"System"
    "System.Core"
)