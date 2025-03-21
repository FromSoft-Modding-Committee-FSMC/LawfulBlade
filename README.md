![image](LawfulBlade/Resource/576x128_lawfulbladeheader.png)\
_A new way to use Sword of Moonlight, inspired by sane and modern game engines._

## Brief
Lawful Blade is a package manager for Sword of Moonlight, it was created to work around some of the many, many problems and inconviniences encounted while using Sword of Moonlight.
Below are a few of the goals.

* **Multiple Installations**: Lawful Blade allows you to have multiple "instances" of Sword of Moonlight, which you can switch between freely - this effectively disables asset limitations imposed by the original engine, and means you can work on many projects with custom assets at the same time without the risk of overwriting something used for another project.

* **Package Mangement**: Lawful Blade allows the installation (and creation) of packages on to an instance, this is significantly faster than the older installers - and has a substantially smaller 

* **Runtime Generation**: Lawful Blade is capable of creating runtimes for SoM projects, meaning after 25 years we finally have a _legal_ way to distribute SoM games in English. Runtime generation is additionally substantially expanded, and allows the installation of fixes for modern hardware, or totally different runtime enviroments to be exported for.

* **What else, oh great Battenberg?**: Project and instance shortcuts, quick explorer navigation... The list honestly goes on and on.

## Other Included Projects
Because of an extreme lack of will to create more repositories at the moment, a few other projects are included inside this one.

* **Lawful Runtime**: An early days implementation of KF-Like mechanics in Unity, which is capable of loading (some*) SoM formats.
* **Unsealer**: A Detouring library to hook some truly awful code in SoM, and enable lots of nice features such as key binding.
* **Sealed Sword Stone**: A launcher which allows UI configuration of Unsealer, and the injection + launching process of SoM games it is bundled with.

## Road map
Here's a very loose roadmap for future features which are expected to be included in Lawful Blade, or as part of the Lawful family of projects.
- 2025:
  - 3Q: SomEx Core + SomEx Packages + SomEx Runtime Generation
- 2026:
  - Lawful Editor
- 2027: 
  - Lawful Runtime (Desktop - Windows, Linux)
- 2028: 
  - Chaosfun Runethyme (Console - PSX)

## Third Party Software Declaration
Lawful Blade is using the following third party software packages to handle some of it's features.
* [Make for Windows](https://gnuwin32.sourceforge.net/packages/make.htm): Included for package creation, as that's all handled on the command line.
* [FFMPEG](https://www.gyan.dev/ffmpeg/builds/): Included for (optionally) transcoding videos into a common codec on Windows (H264) during runtime generation.
* [rcedit](https://github.com/electron/rcedit): Included for altering runtime executable properties during runtime generation.
* [PECHKSUM]([https://www.gammadyne.com/pe_checksum.htm): Included to calculate the executable checksum after wrecking it with RC edit...
