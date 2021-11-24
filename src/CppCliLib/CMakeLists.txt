PROJECT(CppCliLib VERSION 1.0.0.0 LANGUAGES CXX)

# Create assembly info with current version.
CONFIGURE_FILE("${CMAKE_SOURCE_DIR}/src/AssemblyInfo.cpp.template" "${CMAKE_BINARY_DIR}/Config/${PROJECT_NAME}/AssemblyInfo.cpp")

# Collect header & source files.
SET(CppCliLib_HEADERS
    "CppClass.h"
)

SET(CppCliLib_SOURCES
    "CppClass.cpp"
)

# Optional: Create source groups for better code organization.
SOURCE_GROUP(TREE ${CMAKE_CURRENT_SOURCE_DIR} FILES ${CppCliLib_SOURCES} ${CppCliLib_SOURCES})

# Add shared library project.
ADD_LIBRARY(${PROJECT_NAME} SHARED
    "${CMAKE_BINARY_DIR}/Config/${PROJECT_NAME}/AssemblyInfo.cpp"
    ${CppCliLib_HEADERS}
    ${CppCliLib_SOURCES}
)

# Define dependencies.
TARGET_LINK_LIBRARIES(${PROJECT_NAME} 
    PUBLIC CommonLib 
    PRIVATE UnmanagedLib
)

# Set target link options (allows to step into and set breakpoints for mixed-mode C++ assemblies).
# This will not be enabled for Release or MinSizeRel builds.
TARGET_LINK_OPTIONS(${PROJECT_NAME} 
    PUBLIC $<$<OR:$<CONFIG:>,$<CONFIG:Debug>,$<CONFIG:RelWithDebInfo>>:/DEBUG /ASSEMBLYDEBUG>
)

# Define the assembly to be mixed-mode (managed + unmanaged).
SET_PROPERTY(TARGET ${PROJECT_NAME} PROPERTY COMMON_LANGUAGE_RUNTIME "")

# Set CLR assembly properties.
SET_TARGET_PROPERTIES(${PROJECT_NAME} PROPERTIES
    VS_DOTNET_REFERENCES "System;System.Core"
    VS_GLOBAL_ROOTNAMESPACE ${PROJECT_NAME}
)

# Setup installer.
INSTALL(TARGETS ${PROJECT_NAME} EXPORT ${PROJECT_NAME}Config
    ARCHIVE DESTINATION ${CMAKE_INSTALL_LIBRARY_DIR}
    LIBRARY DESTINATION ${CMAKE_INSTALL_LIBRARY_DIR}
    RUNTIME DESTINATION ${CMAKE_INSTALL_BINARY_DIR}
    INCLUDES DESTINATION ${CMAKE_INSTALL_INCLUDE_DIR}
)

# Export config.
INSTALL(EXPORT ${PROJECT_NAME}Config DESTINATION ${CMAKE_INSTALL_EXPORT_DIR})
EXPORT(TARGETS ${PROJECT_NAME} FILE ${PROJECT_NAME}Config.cmake)