namespace LawfulBladeManager.Packages
{
    public interface IPackageTarget
    {
        /// <summary>
        /// Returns a list of compatible package types.
        /// </summary>
        public string[] CompatiblePackages { get; }

        /// <summary>
        /// Uninstalls a package from the package target.
        /// </summary>
        /// <param name="package">The package to uninstall</param>
        /// <returns>True on success, False otherwise</returns>
        public bool UninstallPackage(Package package);

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
        /// <returns>True if the package is rented, false otherwise</returns>
        public bool RentingPackage(Package package);

        /// <summary>
        /// Check if the package target has any outdated packages.
        /// </summary>
        /// <returns>The number of outdated packages</returns>
        public int CheckForOutdatedPackages();
    }
}
