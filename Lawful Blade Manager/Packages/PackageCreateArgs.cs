namespace LawfulBladeManager.Packages
{
    public struct PackageCreateArgs
    {
        public string SourceDirectory;  // Where the source files are located
        public string TargetFile;       // Where the file will be output
        public string Name;             // The name of the package
        public string Description;      // The description of the package
        public string Version;          // The version of the package
        public string[] Authors;        // Any authors of the package
        public string[] Tags;           // Any tags for the package
        public Bitmap Icon;             // The icon data of the package.
    }
}
