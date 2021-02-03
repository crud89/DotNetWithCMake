# .NET with CMake

This project contains a .NET dummy application that can be build with CMake. It demonstrates different project types and how to configure them.

Note that .NET projects require Visual Studio to be build. CMake only manages and sets up the required build scripts for the your Visual Studio version.

- [Project Structure](#project-structure)
- [Building](#building)
    - [From Command Line](#from-command-line)
    - [Using Visual Studio CMake Integration](#using-visual-studio-cmake-integration)
        - [Note on IntelliSense](#note-on-intellisense)
    - [Automatically Restoring NuGet Packages](#automatically-restoring-nuget-packages)
        - [Using NuGet-Packages in C++/CLI projects](#using-nuget-packages-in-ccli-projects)
    - [Building managed Assemblies as `AnyCPU`](#building-managed-assemblies-as-anycpu)
- [Debugging](#debugging)

## Project Structure

There are two top-level projects: *WinFormsApp* and *WpfApp*. Both projects create executables and depend on the projects *CSharpLib* and *CppCliLib*, which create managed DLL assemblies. Both of them depend on a common *CommonLib* project, which also is a managed DLL assembly and demonstrates is used to demonstrate how to call a C# library from a C++/CLI library.

The *CommonLib* defines the `IHello` interface, which looks like this:

```cs
public interface IHello
{
    string SayHello();
}
```

Both, the *CSharpLib* and *CppCliLib* implement a class that returns a string from this interface. The *WpfApp* and *WinFormsApp* both create instances of those classes and call them using the `IHello` interface to display the result inside a message box.

## Building

.NET support for CMake is closely tied to Windows and Visual Studio environments, hence there might be some "incompatibilities" for the general purpose. The template has not been tested with Mono or .NET Core / Linux environments.

### From Command Line

Building the template from command line is straightforward:

```shell
cmake . -B build/ -A x64
cmake --build build/
```

If you want to instead create an `x86` build, change the `-A` parameter to `Win32`.

### Using Visual Studio CMake Integration

The template contains a pre-defined `CMakeSettings.json` file that you can use in your own project, if you want to use Visual Studios integrated CMake support. 

When you are doing this on your own, you have to explicitly specify the `generator`, since Ninja (the default generator) currently [does not support .NET](https://gitlab.kitware.com/cmake/cmake/-/issues/16865). When trying to build any managed assemblies using Ninja, you will receive an error similar to this:

> CMake Error: CMAKE_CSharp_COMPILER not set, after EnableLanguage.

#### Note on IntelliSense

The VS-integrated CMake support (i.e. the CMake target view) does not work with IntelliSense for managed CMake projects. The workaround is to open the generated solution file (*build/[env]/Example.sln*) from a second Visual Studio instance. I've created a [bug report](https://developercommunity2.visualstudio.com/t/1328932) for this issue.

### Automatically Restoring NuGet Packages

Since Visual Studio 2019, `msbuild` can be configured to automatically restore NuGet packages. [CMake 3.15](https://gitlab.kitware.com/cmake/cmake/-/merge_requests/3389) simplified specifying NuGet package references. [CMake 3.18](https://gitlab.kitware.com/cmake/cmake/-/issues/20764) fixed an issue that occured when restoring NuGet packages. So if you are running Visual Studio 2019 with CMake 3.18, you can try to automatically resolve and restore NuGet dependencies. In order to add a package reference to your project, specify the following property:

```cmake
SET_PROPERTY(TARGET ${PROJECT_NAME} PROPERTY VS_PACKAGE_REFERENCES "Serilog_2.9.0;Serilog.Sinks.Console_3.1.1")
```

This will define a package reference inside a C# or C++/CLI project. When building from command line, MSBuild should detect the `.csproj` target and automatically restore the packages. When using the Visual Studio CMake integration, it is possible to tell MSBuild to restore package dependencies before building by specifying the [`-r`](https://docs.microsoft.com/de-de/visualstudio/msbuild/msbuild-command-line-reference) switch. You can do this by adding a build argument in the `CMakeSettings.json` file:

```json
{
  "configurations": [
    {
      "name": "x64-Debug",
      "generator": "Visual Studio 16 2019 Win64",
      "configurationType": "Debug",
      "inheritEnvironments": [ "msvc_x64" ],
      "buildRoot": "${projectDir}\\build\\${name}",
      "installRoot": "${projectDir}\\install\\${name}",
      "buildCommandArgs": "-r"
    }
  ]
}
```

#### Using NuGet-Packages in C++/CLI projects

Whilst in theory it is possible for C++/CLI projects to use NuGet packages that target the same .NET Framework version, it is currently [not properly implemented by Microsoft](https://developercommunity.visualstudio.com/idea/899866/ccli-cant-reference-nuget-packages.html). The naïve approach of defining a `VS_PACKAGE_REFERENCE`, as you would do for a C# project, will result in NuGet not beeing able to resolve the target framework for the project:

> You are trying to install this package into a project that targets 'native,Version=v0.0', but the package does not contain any assembly references or content files that are compatible with that framework.

There are two possible workarounds for this issue, both of which require an intermediate C# project that has a `VS_PACKAGE_REFERENCE` set, so that MSBuild is able to restore the package.

1. Write a wrapper class for all the interfaces you wish to call. Obviously not very feasible.
2. Use a hard reference (i.e. `VS_DOTNET_REFERENCE_*`) to the package assembly. However, this approach is not ideal, since you have to know where the current NuGet package cache resides, which might cause upwards compatibility problems.

For more information, I've created [this issue](https://github.com/Aschratt/DotNetWithCMake/issues/4) to track a possible solution for this problem.

### Building managed Assemblies as `AnyCPU`

The top-level `CMakeLists.txt` file checks the generator used to build the project and sets the target platform accordingly. Note that `AnyCPU` assemblies might cause problems when loaded from an unmanaged context. This is why this template explicitly sets the target platform to either `x64` or `x86`, depending on which generator platform is used. In case an unsupported platform is detected or none is provided (using the command line parameter `-A`), the script will issue a warning and default to `AnyCPU`.

If you know what you are doing and want to explicitly build `AnyCPU` assemblies anyway, you have to set the `CMAKE_CSharp_FLAGS` accordingly:

```cmake
SET(CMAKE_CSharp_FLAGS "/platform:AnyCPU")
```

## Debugging

In order to step into mixed-mode C++/CLI assemblies or set breakpoints, the linker options `/DEBUG` and `/ASSEMBLYDEBUG` need to be set for the C++/CLI projects:

```cmake
TARGET_LINK_OPTIONS(${PROJECT_NAME} PUBLIC /DEBUG /ASSEMBLYDEBUG)
```

The *CppCliLib* project uses a generator expression to only use those options in *Debug* and *RelWithDebInfo* builds.
