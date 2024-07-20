using LawfulBladeManager.Control;
using LawfulBladeManager.Dialog;
using LawfulBladeManager.Instances;
using LawfulBladeManager.Projects;

namespace LawfulBladeManager.Forms
{
    // We use these (slightly awkward with Winforms) partial classes to define callbacks for various tabs
    public partial class MainForm
    {
        /// <summary>
        /// We use a cache so we're not hitting GC as much
        /// </summary>
        readonly Queue<InstanceControl> instanceControlCache = new();

        /// <summary>
        /// Called when the user chooses to create a new instance
        /// </summary>
        void OnInstanceNew(object sender, EventArgs args)
        {
            // Use the instance create dialog to make a new instance
            using InstanceCreateDialog icd = new();

            if (icd.ShowDialog() != DialogResult.OK || icd.CreationArguments == null)
                return;

            Program.InstanceManager.CreateInstance(icd.CreationArguments);

            EnumurateInstances();
        }

        /// <summary>
        /// Called when the user chooses to import a legacy instance
        /// </summary>
        void OnInstanceImport(object sender, EventArgs args)
        {
            // Search for instance to import with the folder browser dialog
            using FolderBrowserDialog fbd = new FolderBrowserDialog();

            if (fbd.ShowDialog() != DialogResult.OK)
                return;

            // A valid legacy instance must have a 'tool' and 'data' folder...
            bool IsValidInstace = true;
            IsValidInstace &= Directory.Exists(Path.Combine(fbd.SelectedPath, "tool"));
            IsValidInstace &= Directory.Exists(Path.Combine(fbd.SelectedPath, "data"));

            if (!IsValidInstace)
            {
                MessageBox.Show("Cannot verify the directory as a Sword of Moonlight Editor instance.", "Lawful Blade", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Import the instance...
            Program.InstanceManager.ImportInstance(fbd.SelectedPath);

            // Enumurate instances
            EnumurateInstances();
        }

        /// <summary>
        /// Called when the user chooses to delete an instance
        /// </summary>
        /// <param name="instance">The target instance</param>
        void OnInstanceDelete(Instance instance)
        {
            EnumurateInstances();
        }

        /// <summary>
        /// Called when the user chooses to manage an instances packages
        /// </summary>
        /// <param name="instance">The target instance</param>
        void OnInstancePackages(Instance instance)
        {
            // Is this a legacy instance? display a warning if so...
            if(instance.HasTag("Legacy"))
            {
                if (MessageBox.Show("You are attempting to install packages on a legacy instance!\nWhile this is allowed, it is not recommend.\n\nDo you want to continue?", "Lawful Blade", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                    return;
            }

            // Create and open the package manager
            using PackageManagerDialog pmd = new(instance);
            pmd.ShowDialog();
        }

        /// <summary>
        /// Called when the user chooses to open an instances
        /// </summary>
        /// <param name="instance">The target instance</param>
        void OnInstanceOpen(Instance instance) =>
            instance.Open();
    }
}
