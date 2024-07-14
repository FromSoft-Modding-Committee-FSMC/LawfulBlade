using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawfulBladeManager.Projects
{
    public class ProjectCreationArgs
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public string InstanceUUID { get; set; } = string.Empty;
        public bool CreateEmpty { get; set; } = false;
    }
}
