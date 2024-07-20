using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawfulBladeManager.Instances
{
    public class InstanceCreateArgs
    {
        public string Destination { get; set; } = string.Empty; // Destination of the instance (file system)
        public string Name        { get; set; } = string.Empty; // Name of the instance
        public string Description { get; set; } = string.Empty; // Description of the instance
        public int IconID         { get; set; } = 0;            // Icon ID of the instance
    }
}
