# .NET with CMake

This project contains a .NET dummy application that can be build with CMake. It demonstrates different project types and how to configure them.

Note that .NET projects require Visual Studio to be build. CMake only manages and sets up the required build scripts for the your Visual Studio version.

## Project Structure

There are two top-level projects: *WinFormsApp* and *WpfApp*. Both projects create executables and depend on the projects *CSharpLib* and *CppCliLib*, which create managed DLL assemblies. Both of them depend on a common *CommonLib* project, which also is a managed DLL assembly and demonstrates is used to demonstrate how to call a C# library from a C++/CLI library.

## General Advice (Rule of Thumb)

When working with managed project, follow these rules to prevent typical pitfalls:

- Always define managed libraries as `SHARED`, i.e. do not use `MODULE`, except for libraries that are optional (like PlugIns) and do not need to be copied to the output directory.
- Use `ADD_DEPENDENCIES` to model a reference to a managed library.
- Only when you have two C++/CLI projects: use `TARGET_LINK_LIBRARIES` (and `INCLUDE_DIRECTORIES`) additionally to `ADD_DEPENDENCIES`, if you also want to link unmanaged symbols (also see *Uncovered Cases* below). Note that the build will fail, if the referenced project does not export any unmanaged symbols.

For more information, also take a look at [this issue](https://gitlab.kitware.com/cmake/cmake/issues/19814).

## Uncovered use-cases

Some use-cases are not explicitly covered in the examples.

### Referencing unmanaged libraries

C++/CLI can be used to create wrapper libraries for unmanaged code. Handling unmanaged library (static or shared) dependencies for C++/CLI projects works the same way as for classic C++ libraries. Basically all you have to do is specify the dependency and include directory and specify the link libraries.

    TARGET_LINK_LIBRARIES(CliCppLib UnmanagedLib)
    INCLUDE_DIRECTORIES(CliCppLib ${UnmanagedLib_SOURCE_DIR})

Alternatively, you can also use `FIND_PACKAGE`:

    FIND_PACKAGE(MyPackage REQUIRED)
    TARGET_LINK_LIBRARIES(CliCppLib ${MYPACKAGE_LIBRARY})
    TARGET_INCLUDE_DIRECTORIES(CliCppLib PUBLIC ${MYPACKAGE_HEADER})

### Referencing NuGet packages

Many .NET projects depend on NuGet packages. Defining nuget dependencies is not very hard. First of all, create a `packages.config` file in the respective project folder. After configuring the `AssemblyInfo.cs` file, also copy it over to the build directory:

    CONFIGURE_FILE("Properties/AssemblyInfo.cs.template" "Properties/AssemblyInfo.cs")
    CONFIGURE_FILE("packages.config" "packages.config")

If you want to, you can try to restore the NuGet packages while running CMake like so:

    FIND_PROGRAM(NUGET nuget)

    IF(NUGET)
	    EXECUTE_PROCESS(COMMAND ${NUGET} restore "packages.config" -SolutionDirectory ${CMAKE_BINARY_DIR} WORKING_DIRECTORY ${CMAKE_BINARY_DIR})
    ENDIF(NUGET)

This is not required, since Visual Studio automatically restores the packages before building the solution. If this does not work, right-click the solution in Visual Studio's project explorer and choose *Restore NuGet packages*.

Finally you have to tell CMake that you want to add a reference to the NuGet packages from the `package.config` file to your project. Unfortunately, you have to do this for each package manually. Here is an example for adding [Serilog](https://serilog.net/):

    SET_PROPERTY(TARGET MyLib PROPERTY VS_DOTNET_REFERENCE_Serilog "${CMAKE_BINARY_DIR}/packages/Serilog.2.8.0/lib/net46/Serilog.dll")