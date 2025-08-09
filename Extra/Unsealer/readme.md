In order to compile, you'll need MSVS 2022. You'll also need the following dependencies:
[Microsoft Detours](https://github.com/microsoft/Detours)
[FMOD](https://www.fmod.com/download)
[json](https://github.com/nlohmann/json)  (You want the single header version)

These dependencies are included in this git repository as submodules and include directories
set up in the Visual Studio project file for them to work.

If you cannot see any files in `sdk/detours`, `sdk/fmod` and `sdk/json`, try running
`git submodule update --init --recursive` to check these out too.
