using LawfulBladeSDK.Generator;

namespace Som2kRuntime
{
    public class GeneratorImpl : IRuntimeGenerator
    {
        public GeneratorProperty[] Properties { get; set; }

        /// <summary>
        /// Som2k runtime **only supports Som2k games**!!!
        /// </summary>
        public string[] SupportedCores { get; } = [ "24d8608f-8dc0-4b21-8333-05f097fd1823" ];

        /// <summary>
        /// Som2k runtime name
        /// </summary>
        public string Name { get; } = "SoM2k Classic Runtime";

        /// <summary>
        /// Event raised when a file is given by lawful blade to complete generation.
        /// </summary>
        public event Action<string> UpdateStatus;

        // Generation Data
        string ProgramPath;
        string TemporaryPath;
        string ProjectPath;
        string InstancePath;
        string PublishPath;

        /// <summary>
        /// Called when the generator is first loaded.
        /// </summary>
        public void OnLoad(GeneratorLoadArgs args)
        {
            ProgramPath   = args.ProgramPath;
            TemporaryPath = args.TemporaryPath;

            /*
            Properties =
            [
                new GeneratorProperty
                {
                    Name  = "Test",
                    Type  = typeof(string),
                    Value = string.Empty
                }
            ];
            */
        }

        public void StartGenerator(GeneratorStartArgs args)
        {
            // Stage 1: Create Output Directories...
            if (!Directory.Exists(args.PublishPath))
                Directory.CreateDirectory(args.PublishPath);

            try
            {
                // Parameters Copy...
                UpdateStatus?.Invoke("Processing Parameters...");

                string PubParam = Path.Combine(args.PublishPath, "PARAM");
                string PrjParam = Path.Combine(args.ProjectPath, "PARAM");

                if (!Directory.Exists(PubParam))
                    Directory.CreateDirectory(PubParam);
                
                File.Copy(Path.Combine(PrjParam, "ENEMY.PR2"), Path.Combine(PubParam, "ENEMY.PR2"), true);
                File.Copy(Path.Combine(PrjParam, "ENEMY.PRM"), Path.Combine(PubParam, "ENEMY.PRM"), true);
                File.Copy(Path.Combine(PrjParam, "ITEM.PR2"), Path.Combine(PubParam, "ITEM.PR2"), true);
                File.Copy(Path.Combine(PrjParam, "ITEM.PRM"), Path.Combine(PubParam, "ITEM.PRM"), true);
                File.Copy(Path.Combine(PrjParam, "MAGIC.PR2"), Path.Combine(PubParam, "MAGIC.PR2"), true);
                File.Copy(Path.Combine(PrjParam, "MAGIC.PRM"), Path.Combine(PubParam, "MAGIC.PRM"), true);
                File.Copy(Path.Combine(PrjParam, "NPC.PR2"), Path.Combine(PubParam, "NPC.PR2"), true);
                File.Copy(Path.Combine(PrjParam, "NPC.PRM"), Path.Combine(PubParam, "NPC.PRM"), true);
                File.Copy(Path.Combine(PrjParam, "OBJ.PR2"), Path.Combine(PubParam, "OBJ.PR2"), true);
                File.Copy(Path.Combine(PrjParam, "OBJ.PRM"), Path.Combine(PubParam, "OBJ.PRM"), true);

                UpdateStatus?.Invoke("Processing System Settings...");
                File.Copy(Path.Combine(PrjParam, "SYS.DAT"), Path.Combine(PubParam, "SYS.DAT"), true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to publish project: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
