namespace LawfulBladeManager.Packages
{
    public interface IPackageTarget
    {
        /// <summary>
        /// Returns a list of compatible package types.
        /// </summary>
        public string[] CompatiblePackages { get; }

        /// <summary>
        /// Install a package to the package target.
        /// </summary>
        /// <param name="package">The package to install</param>
        /// <returns>True on success, False otherwise</returns>
        public bool InstallPackage(Package package);

        /// <summary>
        /// Check if the package target is currently renting (contains) a package.
        /// </summary>
        /// <param name="package">The package to check for</param>
        /// <returns>True if the package is rented, false otherwise.</returns>
        public bool RentingPackage(Package package);
    }
}
