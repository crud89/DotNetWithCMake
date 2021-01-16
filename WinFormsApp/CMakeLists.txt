# Setup project.
PROJECT(WinFormsApp VERSION 1.0.0.0 LANGUAGES CSharp)

# Setup project to use C# utilities.
INCLUDE(CSharpUtilities)

# Create assembly info with current version.
CONFIGURE_FILE("Properties/AssemblyInfo.cs.template" "Properties/AssemblyInfo.cs")

# Add executable project.
ADD_EXECUTABLE(WinFormsApp
	#"App.config"
	
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

# Setup project dependencies.
ADD_DEPENDENCIES(WinFormsApp CommonLib)
ADD_DEPENDENCIES(WinFormsApp CppCliLib)
ADD_DEPENDENCIES(WinFormsApp CSharpLib)

# Setup project properties.
SET_PROPERTY(TARGET WinFormsApp PROPERTY VS_DOTNET_TARGET_FRAMEWORK_VERSION "v4.7.2")
SET_PROPERTY(TARGET WinFormsApp PROPERTY WIN32_EXECUTABLE TRUE)
SET_PROPERTY(TARGET WinFormsApp PROPERTY VS_DOTNET_REFERENCES
	"Microsoft.CSharp"
	"System"
	"System.Core"
	"System.Data"
	"System.Drawing"
	"System.Windows.Forms"
)

# Setup root namespace for the project.
set_property(TARGET WinFormsApp PROPERTY VS_GLOBAL_ROOTNAMESPACE ${PROJECT_NAME})
