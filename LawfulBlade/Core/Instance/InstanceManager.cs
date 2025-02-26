using System.IO;

namespace LawfulBlade.Core.Instance
{
    public static class InstanceManager
    {
        /// <summary>
        /// Event invoked when an instance is added or removed
        /// </summary>
        public static event Action InstancesChanged;

        /// <summary>
        /// List of all avaliable instances
        /// </summary>
        public static List<Instance> Instances { get; private set; }

        /// <summary>
        /// The number of instances
        /// </summary>
        public static int Count => Instances.Count;

        /// <summary>
        /// Initializes the instance manager, loading all instances in to memory
        /// </summary>
        public static void Initialize()
        {
            // Create the instance list
            Instances = [];

            if (!Directory.Exists(App.InstancePath))
                Directory.CreateDirectory(App.InstancePath);

            // Get a directory info for the instances root.
            DirectoryInfo instanceDirInfo = new (App.InstancePath);

            // Go through each subdirectory of the instances folder...
            foreach (DirectoryInfo instanceDir in instanceDirInfo.GetDirectories())
            {
                // If the folder does not contain 'instance.json' we will not process it.
                if (!File.Exists(Path.Combine(instanceDir.FullName, "instance.json")))
                    continue;

                // The instance.json file does exist, we can load this as an instance...
                Instance loadedInstance = Instance.Load(instanceDir.FullName);

                // If the instance returned null, do not add it to the list...
                if (loadedInstance != null)
                    Instances.Add(loadedInstance);
            }
        }

        /// <summary>
        /// Gets an instance from the list of managed isntances by the UUID
        /// </summary>
        public static Instance GetInstanceByUUID(string UUID)
        {
            foreach (Instance instance in Instances)
            {
                if (instance.UUID == UUID)
                    return instance;
            }

            return null;
        }

        /// <summary>
        /// Adds an instance to the list of managed instances...
        /// </summary>
        public static void AddInstance(Instance instance)
        {
            // Add instance
            Instances.Add(instance);

            // Raise the instance changed event...
            InstancesChanged?.Invoke();
        }

        /// <summary>
        /// Removes an instance to the list of managed instances...
        /// </summary>
        /// <param name="instance"></param>
        public static void RemoveInstance(Instance instance)
        {
            // Remove instance
            Instances.Remove(instance);

            // Raise the instance changed event...
            InstancesChanged?.Invoke();
        }

        /// <summary>
        /// Call to shutdown the instance manager
        /// </summary>
        public static void Shutdown()
        {
            // Attempt to save all our instances...
            foreach (Instance instance in Instances)
                instance.Save();

            // Clear the instances list
            Instances.Clear();
            Instances = null;
        }
    }
}
