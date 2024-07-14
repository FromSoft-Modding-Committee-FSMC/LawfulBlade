using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace LawfulBladeManager.Instances
{
    public class InstanceManager
    {
        // Instance Storage
        public readonly string InstancesFile = Path.Combine(ProgramContext.AppDataPath, @"instances.json");
        public List<Instance> Instances { get; private set; }
        
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
        bool SaveInstances()
        {
            Logger.ShortInfo($"Saving {Instances.Count} instance(s)!");

            // Serialize the instances.json file, and save it to disk
            File.WriteAllText(InstancesFile, JsonSerializer.Serialize(Instances, JsonSerializerOptions.Default));

            Logger.ShortInfo($"Done!");

            return true;
        }

        /// <summary>
        /// Finds an instance and returns it.
        /// </summary>
        /// <param name="instance">[OUT] the found instance</param>
        /// <param name="UUID">The UUID of an instance</param>
        /// <returns>True on success, False otherwise</returns>
        public bool FindInstanceByUUID(out Instance? instance, string UUID)
        {
            foreach(Instance i in Instances)
            {
                // If it's not our instance skip this iteration...
                if (i.InstanceUUID != UUID)
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
