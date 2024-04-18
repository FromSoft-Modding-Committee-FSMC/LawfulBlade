namespace LawfulBladeManager.Tagging
{
    public struct Tag
    {
        public static readonly Tag Project = new() { Text = "Project", BackgroundColour = Color.AliceBlue };
        public static readonly Tag Sample  = new() { Text = "Sample", BackgroundColour = Color.GreenYellow };
        public static readonly Tag Managed = new() { Text = "Managed", BackgroundColour = Color.RebeccaPurple };
        public static readonly Tag Legacy  = new() { Text = "Legacy", BackgroundColour = Color.Orange };

        public static readonly Tag[] TagList = new Tag[]
        {
            // Project Tags
            Project,
            Sample,

            // Instance Tags

            // Package Tags

            // Misc Tags
            Legacy,
            Managed
        };

        public string Text;
        public Color BackgroundColour;
    }
}
