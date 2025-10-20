using LawfulBladeSDK.Generator;
using System.IO;
using System.Reflection;

namespace LawfulBlade.Core
{
    public static class RuntimeManager
    {
        /// <summary>List of generator implementations</summary>
        public static List<IRuntimeGenerator> Generators { get; private set; }

        /// <summary>The number of runtime generators loaded</summary>
        public static int Count => Generators.Count;

        public static void Initialize()
        {
            // List of runtime generators...
            Generators = [];

            if (!Directory.Exists(App.RuntimeGenPath))
                Directory.CreateDirectory(App.RuntimeGenPath);

            // Scan for all generator implementations and load them...
            foreach (FileInfo potentialGenerator in new DirectoryInfo(App.RuntimeGenPath).GetFiles("*.dll"))
            {
                try
                {
                    // Load the assembly file
                    Assembly assemblyFile = Assembly.LoadFrom(potentialGenerator.FullName);

                    // See if this dll implements the IRuntimeGenerator...
                    foreach (Type type in assemblyFile.GetTypes())
                    {
                        // Skip the type if it's not implementing IRuntimeGenerator _or_ it's the interface itself
                        if (!typeof(IRuntimeGenerator).IsAssignableFrom(type) || type.IsInterface)
                            continue;

                        // Create an instance of the runtime generator
                        IRuntimeGenerator generatorInst = (IRuntimeGenerator)Activator.CreateInstance(type);

                        // If it's null, we need to complain at the user...
                        if (generatorInst == null)
                        {
                            Debug.Critical($"Couldn't load runtime generator from file: '{potentialGenerator.FullName}'");
                            continue;
                        }

                        // Call the OnLoad function
                        generatorInst.OnLoad(new GeneratorLoadArgs
                        {
                            ProgramPath   = App.ProgramPath,
                            TemporaryPath = App.TemporaryPath
                        });

                        // Store the generator inside list
                        Generators.Add(generatorInst);
                    }
                }
                catch (Exception ex)
                {
                    Debug.Critical("Failed to load generator assembly!");
                    Debug.Critical(ex.Message);
                }
            }
        }

        public static void Shutdown() => throw new NotImplementedException();
    }
}
