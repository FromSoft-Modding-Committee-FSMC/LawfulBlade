using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LawfulBladeManager.Tagging
{
    public struct Tag
    {
        public static readonly Tag UserProject   = new() { Text = "User Project",   BackgroundColour = Color.AliceBlue };
        public static readonly Tag SampleProject = new() { Text = "Sample Project", BackgroundColour = Color.GreenYellow };

        public static readonly Tag[] TagList = new Tag[]
        {
            // Project Tags
            UserProject,
            SampleProject

            // Instance Tags

            // Package Tags
        };

        public string Text;
        public Color BackgroundColour;
    }
}
