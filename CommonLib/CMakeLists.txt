# Setup project.
PROJECT(CommonLib VERSION 1.0.0.0 LANGUAGES CSharp)

# Setup project to use C# utilities.
INCLUDE(CSharpUtilities)

# Create assembly info with current version.
CONFIGURE_FILE("Properties/AssemblyInfo.cs.template" "Properties/AssemblyInfo.cs")

# Add shared library project.
ADD_LIBRARY(CommonLib SHARED
    "Properties/AssemblyInfo.cs"
    "Common.cs"
)

# Set designer and XAML properties.
CSHARP_SET_DESIGNER_CS_PROPERTIES(
    "Properties/AssemblyInfo.cs"
)

# NOTE: If you want to restore NuGet packages (see README.md), do it here!

# Setup the .NET Framework version.
SET_PROPERTY(TARGET CommonLib PROPERTY VS_DOTNET_TARGET_FRAMEWORK_VERSION "v4.7.2")

# Setup the default assembly references.
SET_PROPERTY(TARGET CommonLib PROPERTY VS_DOTNET_REFERENCES
	"System"
    "System.Core"
)

