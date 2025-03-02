namespace LawfulBladeSDK.Generator
{
    public interface IRuntimeGenerator
    {
        /// <summary>
        /// Property list for the generator.
        /// </summary>
        public GeneratorProperty[] Properties { get; set; }

        /// <summary>
        /// Event fired when a file is given to the 
        /// </summary>
        public event Action<string> UpdateStatus;

        /// <summary>
        /// Cores supported by the generator
        /// </summary>
        public string[] SupportedCores { get; }

        /// <summary>
        /// The name of the generator
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Called when the runtime generator is first loaded
        /// </summary>
        public void OnLoad(GeneratorLoadArgs args);

        /// <summary>
        /// Called to begin runtime generation...
        /// </summary>
        public void StartGenerator(GeneratorStartArgs args);
    }
}
