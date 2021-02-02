# Setup project.
PROJECT(CSharpLib VERSION 1.0.0.0 LANGUAGES CSharp)

# Setup project to use C# utilities.
INCLUDE(CSharpUtilities)

# Create assembly info with current version.
CONFIGURE_FILE("Properties/AssemblyInfo.cs.template" "Properties/AssemblyInfo.cs")

# Add shared library project.
ADD_LIBRARY(${PROJECT_NAME} SHARED
    "Properties/AssemblyInfo.cs"
    "CSharpClass.cs"
)

# Set designer and XAML properties.
CSHARP_SET_DESIGNER_CS_PROPERTIES(
    "Properties/AssemblyInfo.cs"
)

# NOTE: If you want to restore NuGet packages (see README.md), do it here!

# Setup the .NET Framework version.
SET_PROPERTY(TARGET ${PROJECT_NAME} PROPERTY VS_DOTNET_TARGET_FRAMEWORK_VERSION "v4.7.2")

# Setup the default assembly references.
SET_PROPERTY(TARGET ${PROJECT_NAME} PROPERTY VS_DOTNET_REFERENCES
	"System"
    "System.Core"
)

# Setup root namespace for the project.
SET_PROPERTY(TARGET ${PROJECT_NAME} PROPERTY VS_GLOBAL_ROOTNAMESPACE ${PROJECT_NAME})