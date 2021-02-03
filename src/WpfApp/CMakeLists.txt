# Setup project.
PROJECT(WpfApp VERSION 1.0.0.0 LANGUAGES CSharp)

# Setup project to use C# utilities.
INCLUDE(CSharpUtilities)

# Create assembly info with current version.
CONFIGURE_FILE("Properties/AssemblyInfo.cs.template" "Properties/AssemblyInfo.cs")

# Add executable project.
ADD_EXECUTABLE(${PROJECT_NAME}
    "App.config"

	"App.xaml"
	"App.xaml.cs"
	"MainWindow.xaml"
	"MainWindow.xaml.cs"

    "Properties/AssemblyInfo.cs"
	"Properties/Resources.Designer.cs"
	"Properties/Resources.resx"
	"Properties/Settings.Designer.cs"
    "Properties/Settings.settings"
)

# Set designer and XAML properties.
CSHARP_SET_DESIGNER_CS_PROPERTIES(
    "Properties/AssemblyInfo.cs"
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