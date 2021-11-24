# Setup project.
PROJECT(WpfApp VERSION 1.0.0.0 LANGUAGES CSharp)

# Setup project to use C# utilities.
INCLUDE(CSharpUtilities)

# Create assembly info with current version.
CONFIGURE_FILE("${CMAKE_SOURCE_DIR}/src/AssemblyInfo.cs.template" "${CMAKE_BINARY_DIR}/Config/${PROJECT_NAME}/AssemblyInfo.cs")

# Add executable project.
ADD_EXECUTABLE(${PROJECT_NAME}
    "app.config"

	"App.xaml"
	"App.xaml.cs"
	"MainWindow.xaml"
	"MainWindow.xaml.cs"
    
    "${CMAKE_BINARY_DIR}/Config/${PROJECT_NAME}/AssemblyInfo.cs"
	"Properties/Resources.Designer.cs"
	"Properties/Resources.resx"
	"Properties/Settings.Designer.cs"
    "Properties/Settings.settings"
)

# Set designer and XAML properties.
CSHARP_SET_DESIGNER_CS_PROPERTIES(
    "${CMAKE_BINARY_DIR}/Config/${PROJECT_NAME}/AssemblyInfo.cs"
	"Properties/Resources.Designer.cs"
    "Properties/Resources.resx"
	"Properties/Settings.Designer.cs"
    "Properties/Settings.settings"
)

CSHARP_SET_XAML_CS_PROPERTIES(
	"App.xaml"
	"App.xaml.cs"
	"MainWindow.xaml"
    "MainWindow.xaml.cs"
)

SET_PROPERTY(SOURCE App.xaml PROPERTY VS_XAML_TYPE "ApplicationDefinition")

# Define dependencies.
TARGET_LINK_LIBRARIES(${PROJECT_NAME} 
    PUBLIC CommonLib CppCliLib CSharpLib
)

# Set CLR assembly properties.
SET_TARGET_PROPERTIES(${PROJECT_NAME} PROPERTIES
	WIN32_EXECUTABLE TRUE
    VS_DOTNET_REFERENCES "System;System.Core;System.Data;System.Data.DataSetExtensions;System.Drawing;System.Net.Http;System.Xaml;System.Xml.Linq;Microsoft.CSharp;PresentationCore;PresentationFramework;WindowsBase"
    VS_GLOBAL_ROOTNAMESPACE ${PROJECT_NAME}
)

# Optional: enable native code debugging to allow stepping into unmanaged sources.
# This is only required by the top level executable project.
IF(NOT CMAKE_BUILD_TYPE OR CMAKE_BUILD_TYPE MATCHES "Debug|RelWithDebInfo")
    SET_PROPERTY(TARGET ${PROJECT_NAME} PROPERTY VS_GLOBAL_EnableUnmanagedDebugging "true")
ENDIF (NOT CMAKE_BUILD_TYPE OR CMAKE_BUILD_TYPE MATCHES "Debug|RelWithDebInfo")

# Setup installer.
INSTALL(TARGETS ${PROJECT_NAME} EXPORT ${PROJECT_NAME}Config
    ARCHIVE DESTINATION ${CMAKE_INSTALL_LIBRARY_DIR}
    LIBRARY DESTINATION ${CMAKE_INSTALL_LIBRARY_DIR}
    RUNTIME DESTINATION ${CMAKE_INSTALL_BINARY_DIR}
    INCLUDES DESTINATION ${CMAKE_INSTALL_INCLUDE_DIR}
)

INSTALL(FILES "app.config" DESTINATION ${CMAKE_INSTALL_BINARY_DIR} RENAME "${PROJECT_NAME}${CMAKE_EXECUTABLE_SUFFIX}.config")
# Also covers targets with different OUTPUT_NAME properties. Not yet supported, but will be added in near future: https://gitlab.kitware.com/cmake/cmake/-/issues/19903
#INSTALL(FILES "app.config" DESTINATION ${CMAKE_INSTALL_BINARY_DIR} RENAME "$<TARGET_FILE_NAME:${PROJECT_NAME}>.config")

# Export config.
INSTALL(EXPORT ${PROJECT_NAME}Config DESTINATION ${CMAKE_INSTALL_EXPORT_DIR})
EXPORT(TARGETS ${PROJECT_NAME} FILE ${PROJECT_NAME}Config.cmake)