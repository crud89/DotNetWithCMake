# Setup project.
PROJECT(WpfApp VERSION 1.0.0.0 LANGUAGES CSharp)

# Setup project to use C# utilities.
INCLUDE(CSharpUtilities)

# Create assembly info with current version.
CONFIGURE_FILE("Properties/AssemblyInfo.cs.template" "Properties/AssemblyInfo.cs")

# Add executable project.
ADD_EXECUTABLE(WpfApp
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

# Setup project dependencies.
ADD_DEPENDENCIES(WpfApp CommonLib)
ADD_DEPENDENCIES(WpfApp CppCliLib)
ADD_DEPENDENCIES(WpfApp CSharpLib)

# Setup project properties.
SET_PROPERTY(SOURCE App.xaml PROPERTY VS_XAML_TYPE "ApplicationDefinition")
SET_PROPERTY(TARGET WpfApp PROPERTY VS_DOTNET_TARGET_FRAMEWORK_VERSION "v4.7.2")
SET_PROPERTY(TARGET WpfApp PROPERTY WIN32_EXECUTABLE TRUE)
SET_PROPERTY(TARGET WpfApp PROPERTY VS_DOTNET_REFERENCES
	"Microsoft.CSharp"
	"PresentationCore"
	"PresentationFramework"
	"System"
	"System.Core"
	"System.Data"
	"System.Data.DataSetExtensions"
	"System.Net.Http"
	"System.Xaml"
	"System.Xml"
	"System.Xml.Linq"
    "WindowsBase"
)