namespace LawfulBladeManager.Projects
{
    public class ProjectCreateArgs
    {
        public string Name         { get; set; } = string.Empty;        // Name of the project
        public string Description  { get; set; } = string.Empty;        // Description of the project
        public string Destination  { get; set; } = string.Empty;        // Destination of the project (file system)
        public string InstanceUUID { get; set; } = string.Empty;        // UUID of the instance that owns this project
        public bool CreateEmpty    { get; set; } = false;               // True when the project should be created without default files.
    }
}
