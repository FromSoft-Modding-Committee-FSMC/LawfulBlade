using LawfulBladeManager.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace LawfulBladeManager.Instances
{
    public class InstanceManager
    {
        // Instance Storage
        public readonly string InstancesFile = Path.Combine(ProgramContext.AppDataPath, @"instances.json");

        // This array stores instance icon images... We use fixed icons so we can have shortcuts (to-do)
        public readonly Image[] InstanceIcons = new Image[]
        {
            Properties.Resources._256xInstMoonlightSword,
            Properties.Resources._256xInstDarkSlayer,
            Properties.Resources._256xInstDragonFlower,
            Properties.Resources._256xInstExcellector,
            Properties.Resources._256xInstIdolOfSorrow,
            Properties.Resources._256xInstKingsCrown,
            Properties.Resources._256xInstLightCrystal,
            Properties.Resources._256xInstLawfulBlade,
            Properties.Resources._256xInstSkull,
            Properties.Resources._256xInstTripleFang,
            Properties.Resources._256xInstWaveCrasher
        };

        public List<Instance> Instances { get; private set; }
        public int InstanceCount => Instances.Count;

        /// <summary>
        /// Constructor is responsible for loading instances from disk.
        /// </summary>
        public InstanceManager()
        {
            Instances = new List<Instance>();

            if (!LoadInstances())
                Logger.ShortWarn("Failed to load any instances!");

            // Save instances on shutdown
            Program.OnShutdown += SaveInstances;
        }

        /// <summary>
        /// Load all instances from the instance declaration file.
        /// </summary>
        /// <returns>True on success, False otherwise.</returns>
        bool LoadInstances()
        {
            Logger.ShortInfo("Loading instances...");

            if (!File.Exists(InstancesFile))
                return false;

            // If we already have loaded instances, clear them.
            if (Instances.Count > 0)
                Instances.Clear();

            // De-Serialize the instances.json file
            Instance[]? maybeInstances = JsonSerializer.Deserialize<Instance[]>(File.ReadAllText(InstancesFile), JsonSerializerOptions.Default);

            if (maybeInstances == null)
                return false;

            // Store each loaded instance inside the Instances list
            foreach (Instance instance in maybeInstances)
                Instances.Add(instance);

            Logger.ShortInfo($"Loaded {Instances.Count} instance(s).");

            return true;
        }

        /// <summary>
        /// Save all instances to the instance declaration file.
        /// </summary>
        /// <returns>True on success, False otherwise.</returns>
        public bool SaveInstances()
        {
            Logger.ShortInfo($"Saving {Instances.Count} instance(s)!");

            // Serialize the instances.json file, and save it to disk
            File.WriteAllText(InstancesFile, JsonSerializer.Serialize(Instances, JsonSerializerOptions.Default));

            Logger.ShortInfo($"Done!");

            return true;
        }

        /// <summary>
        /// Performs a scan of all instances to find any outdated packages.
        /// </summary>
        /// <returns>True if there are outdated packages, false otherwise.</returns>
        public bool CheckInstancesForOutdatedPackages()
        {
            int numberOfInstancesWithOutdatedPackages = 0;

            // Check each instance
            foreach (Instance instance in Instances)
                if (instance.CheckForOutdatedPackages() > 0)
                    numberOfInstancesWithOutdatedPackages++;

            // Exit now if there are no instances with outdated packages.
            if (numberOfInstancesWithOutdatedPackages == 0)
                return false;

            Logger.ShortWarn($"{numberOfInstancesWithOutdatedPackages} instances have outdated packages, update them with the package manager!");
            return true;
        }


        /// <summary>
        /// Creates a new instance according to the arguments provided.
        /// </summary>
        /// <param name="args">Instance creation settings</param>
        public void CreateInstance(InstanceCreateArgs args)
        {
            // Construct the absolute path of the project
            string absolutePath = Path.Combine(args.Destination, args.Name);

            // Make sure the directory exists
            if (!Directory.Exists(absolutePath))
                Directory.CreateDirectory(absolutePath);

            // Store the project information
            Instances.Add(new Instance
            {
                Name        = args.Name,
                Description = args.Description,
                IconID      = args.IconID,
                UUID        = new Guid(MD5.HashData(Encoding.UTF8.GetBytes($"{DateTime.Now}{args.Name}{args.Description}"))).ToString(),
                StoragePath = absolutePath,
                Tags        = new string[] { "Instance", "Managed" }
            });

            // Save Instances File
            SaveInstances();
        }

        /// <summary>
        /// Imports a legacy instance located at the absolute path
        /// </summary>
        /// <param name="absolutePath">Path to an instance</param>
        public void ImportInstance(string absolutePath)
        {
            // Store the project information
            Instances.Add(new Instance
            {
                Name        = Path.GetFileName(absolutePath),
                Description = $"Legacy Sword of Moonlight instance.",
                IconID      = 0,
                UUID        = new Guid(MD5.HashData(Encoding.UTF8.GetBytes($"{DateTime.Now}{Path.GetFileName(absolutePath)}{absolutePath}"))).ToString(),
                StoragePath = absolutePath,
                Tags        = new string[] { "Instance", "Legacy" }
            });

            // Save Instances File
            SaveInstances();
        }

        /// <summary>
        /// Finds an instance and returns it.
        /// </summary>
        /// <param name="UUID">The UUID of an instance</param>
        /// <param name="instance">[OUT] the found instance</param>
        /// <returns>True on success, False otherwise</returns>
        public bool FindInstanceByUUID(string UUID, out Instance? instance)
        {
            foreach(Instance i in Instances)
            {
                // If it's not our instance skip this iteration...
                if (i.UUID != UUID)
                    continue;

                // Found the instance
                instance = i;
                return true;
            }

            // Couldn't find instance
            instance = null;
            return false;
        }
    }
}
