# .NET with CMake

This project contains a .NET dummy application that can be build with CMake. It demonstrates different project types and how to configure them.

Note that .NET projects require Visual Studio to be build. CMake only manages and sets up the required build scripts for the your Visual Studio version.

- [Project Structure](#project-structure)
- [General Advice (Rule of Thumb)](#general-advice-rule-of-thumb)
- [Uncovered use-cases](#uncovered-use-cases)
    - [Referencing unmanaged libraries](#referencing-unmanaged-libraries)
    - [Building managed Assemblies as `AnyCPU`](#building-managed-assemblies-as-anycpu)
- [Building](#building)
    - [From Command Line](#from-command-line)
    - [Using Visual Studio CMake Integration](#using-visual-studio-cmake-integration)
    - [Automatically Restoring NuGet Packages](#automatically-restoring-nuget-packages)

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

### Building managed Assemblies as `AnyCPU`

The top-level `CMakeLists.txt` file checks the generator used to build the project and sets the target platform accordingly. Note that `AnyCPU` assemblies might cause problems when loaded from an unmanaged context. This is why this template explicitly sets the target platform to either `x64` or `x86`, depending on which generator platform is used. In case an unsupported platform is detected or none is provided (using the command line parameter `-A`), the script will issue a warning and default to `AnyCPU`.

If you know what you are doing and want to explicitly build `AnyCPU` assemblies anyway, you have to set the `${CMAKE_CSharp_FLAGS}` accordingly:

    SET(CMAKE_CSharp_FLAGS "/platform:AnyCPU")

## Building

.NET support for CMake is closely tied to Windows and Visual Studio environments, hence there might be some "incompatibilities" for the general purpose. The template has not been tested with Mono or .NET Core / Linux environments.

### From Command Line

Building the template from command line is straightforward:

    cmake . -B build/ -A x64
    cmake --build build/

If you want to instead create an `x86` build, change the `-A` parameter to `Win32`.

### Using Visual Studio CMake Integration

The template contains a pre-defined `CMakeSettings.json` file that you can use in your own project, if you want to use Visual Studios integrated CMake support. 

When you are doing this on your own, you have to explicitly specify the `generator`, since Ninja (the default generator) currently [does not support .NET](https://gitlab.kitware.com/cmake/cmake/-/issues/16865). When trying to build any managed assemblies using Ninja, you will receive an error similar to this:

> CMake Error: CMAKE_CSharp_COMPILER not set, after EnableLanguage.

### Automatically Restoring NuGet Packages

Since Visual Studio 2019, `msbuild` can be configured to automatically restore NuGet packages. [CMake 3.15](https://gitlab.kitware.com/cmake/cmake/-/merge_requests/3389) simplified specifying NuGet package references. [CMake 3.18](https://gitlab.kitware.com/cmake/cmake/-/issues/20764) fixed an issue that occured when restoring NuGet packages. So if you are running Visual Studio 2019 with CMake 3.18, you can try to automatically resolve and restore NuGet dependencies. In order to add a package reference to your project, specify the following property:

    SET_PROPERTY(TARGET ${PROJECT_NAME} PROPERTY VS_PACKAGE_REFERENCES "Serilog_2.9.0;Serilog.Sinks.Console_3.1.1")
    
This will define a package reference inside a C# or C++/CLI project. When building from command line, MSBuild should detect the `.csproj` target and automatically restore the packages. When using the Visual Studio CMake integration, it is possible to tell MSBuild to restore package dependencies before building by specifying the [`-r`](https://docs.microsoft.com/de-de/visualstudio/msbuild/msbuild-command-line-reference) switch. You can do this by adding a build argument in the `CMakeSettings.json` file:

```json
{
  "configurations": [
    {
      "name": "x64-Debug",
      "generator": "Visual Studio 16 2019 Win64",
      "configurationType": "Debug",
      "inheritEnvironments": [ "msvc_x64" ],
      "buildRoot": "${projectDir}\\..\\out\\build\\${name}",
      "installRoot": "${projectDir}\\..\\out\\install\\${name}",
      "buildCommandArgs": "-r"
    }
  ]
}
```
