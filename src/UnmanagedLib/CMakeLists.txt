# Setup project.
PROJECT(UnmanagedLib VERSION 1.0.0.0 LANGUAGES CXX)

# Add shared library project.
ADD_LIBRARY(${PROJECT_NAME} SHARED "include/UnmanagedClass.h" "src/UnmanagedClass.cpp")

# Define public header.
SET_TARGET_PROPERTIES(${PROJECT_NAME} PROPERTIES 
        PUBLIC_HEADER "include/UnmanagedClass.h"
)

# Setup target include directories.
TARGET_INCLUDE_DIRECTORIES(${PROJECT_NAME}
    PRIVATE
        "src/"
    PUBLIC
        $<BUILD_INTERFACE:${CMAKE_CURRENT_SOURCE_DIR}/include>
)

# Set target link options (allows to step into and set breakpoints for mixed-mode C++ assemblies).
# This will not be enabled for Release or MinSizeRel builds.
TARGET_LINK_OPTIONS(${PROJECT_NAME} 
    PUBLIC $<$<OR:$<CONFIG:>,$<CONFIG:Debug>,$<CONFIG:RelWithDebInfo>>:/DEBUG /ASSEMBLYDEBUG>
)

# Setup installer.
INSTALL(TARGETS ${PROJECT_NAME} EXPORT ${PROJECT_NAME}Config
    ARCHIVE DESTINATION ${CMAKE_INSTALL_LIBRARY_DIR}
    LIBRARY DESTINATION ${CMAKE_INSTALL_LIBRARY_DIR}
    RUNTIME DESTINATION ${CMAKE_INSTALL_BINARY_DIR}
    INCLUDES DESTINATION ${CMAKE_INSTALL_INCLUDE_DIR}
    PUBLIC_HEADER DESTINATION ${CMAKE_INSTALL_INCLUDE_DIR}
)

# Also install PDB files for debugging (optional).
INSTALL(FILES $<TARGET_PDB_FILE:${PROJECT_NAME}> DESTINATION ${CMAKE_INSTALL_BINARY_DIR} OPTIONAL)

# Export config.
INSTALL(EXPORT ${PROJECT_NAME}Config DESTINATION ${CMAKE_INSTALL_EXPORT_DIR})
EXPORT(TARGETS ${PROJECT_NAME} FILE ${PROJECT_NAME}Config.cmake)
