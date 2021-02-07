# Setup project.
PROJECT(WinFormsApp VERSION 1.0.0.0 LANGUAGES CSharp)

# Setup project to use C# utilities.
INCLUDE(CSharpUtilities)

# Create assembly info with current version.
CONFIGURE_FILE("Properties/AssemblyInfo.cs.template" "Properties/AssemblyInfo.cs")

# Add executable project.
ADD_EXECUTABLE(${PROJECT_NAME}
	"app.config"
	
	"Program.cs"

    "Properties/AssemblyInfo.cs"
	"Properties/Resources.Designer.cs"
	"Properties/Resources.resx"
	"Properties/Settings.Designer.cs"
    "Properties/Settings.settings"
)

# Set Windows Forms properties
CSHARP_SET_WINDOWS_FORMS_PROPERTIES(
	"Program.cs"
	
    "Properties/AssemblyInfo.cs"
	"Properties/Resources.Designer.cs"
    "Properties/Resources.resx"
	"Properties/Settings.Designer.cs"
    "Properties/Settings.settings"
)

# Define dependencies.
TARGET_LINK_LIBRARIES(${PROJECT_NAME} 
    PUBLIC CommonLib CppCliLib CSharpLib
)

# Set CLR assembly properties.
SET_TARGET_PROPERTIES(${PROJECT_NAME} PROPERTIES
	WIN32_EXECUTABLE TRUE
    VS_DOTNET_REFERENCES "System;System.Core;System.Data;System.Drawing;System.Windows.Forms;Microsoft.CSharp"
    VS_GLOBAL_ROOTNAMESPACE ${PROJECT_NAME}
)

# Setup installer.
INSTALL(TARGETS ${PROJECT_NAME} EXPORT ${PROJECT_NAME}Config
    ARCHIVE DESTINATION ${CMAKE_INSTALL_LIBRARY_DIR}
    LIBRARY DESTINATION ${CMAKE_INSTALL_LIBRARY_DIR}
    RUNTIME DESTINATION ${CMAKE_INSTALL_BINARY_DIR}
    INCLUDES DESTINATION ${CMAKE_INSTALL_INCLUDE_DIR}
)

INSTALL(FILES "app.config" DESTINATION ${CMAKE_INSTALL_BINARY_DIR} RENAME "${PROJECT_NAME}.config")
# Also covers targets with different OUTPUT_NAME properties. Not yet supported, but will be added in near future: https://gitlab.kitware.com/cmake/cmake/-/issues/19903
#INSTALL(FILES "app.config" DESTINATION ${CMAKE_INSTALL_BINARY_DIR} RENAME "$<TARGET_FILE_NAME:${PROJECT_NAME}>.config")

# Export config.
INSTALL(EXPORT ${PROJECT_NAME}Config DESTINATION ${CMAKE_INSTALL_EXPORT_DIR})
EXPORT(TARGETS ${PROJECT_NAME} FILE ${PROJECT_NAME}Config.cmake)