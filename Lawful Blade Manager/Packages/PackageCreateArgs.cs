namespace LawfulBladeManager.Packages
{
    public class PackageCreateArgs
    {
        public string SourceDirectory { get; set; } = string.Empty;             // Location of the input file(s)
        public string TargetFile      { get; set; } = string.Empty;             // Location of the output file
        public string Name            { get; set; } = string.Empty;             // Name of the package
        public string Description     { get; set; } = string.Empty;             // Description of the package
        public string Version         { get; set; } = string.Empty;             // Version of the package
        public string[] Authors       { get; set; } = Array.Empty<string>();    // Author(s) of the package
        public string[] Tags          { get; set; } = Array.Empty<string>();    // Tag(s) associated with the package
        public string[] Dependencies  { get; set; } = Array.Empty<string>();    // Dependencies which the package... Depends on.
        public Bitmap? IconSource     { get; set; } = null;                     // Source of the icon for the package
        public bool ExpectOverwrites  { get; set; } = false;                    // True when the package should expect to overwrite other files.
    }
}
