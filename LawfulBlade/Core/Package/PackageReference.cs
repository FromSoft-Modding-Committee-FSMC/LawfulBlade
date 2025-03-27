namespace LawfulBlade.Core.Package
{
    public struct PackageReference
    {
        /// <summary>The UUID of the package</summary>
        public string UUID { get; set; }

        /// <summary>The version of the package</summary>
        public string Version { get; set; }

        /// <summary>Any files added or overwritten by the package</summary>
        public PackageFile[] Files { get; set; }

        /// <summary>If there were conflicts when the package was installed.</summary>
        public bool InstalledWithConflicts { get; set; }
    }
}
