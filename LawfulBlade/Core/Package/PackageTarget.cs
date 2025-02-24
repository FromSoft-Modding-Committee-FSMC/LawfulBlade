using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LawfulBlade.Core.Package
{
    public abstract class PackageTarget
    {
        /// <summary>
        /// The root data location of the package target
        /// </summary>
        [JsonIgnore]
        public string Root { get; protected set; }

        /// <summary>
        /// If the target is dirty, and needs to be saved.
        /// </summary>
        [JsonIgnore]
        public bool Dirty { get; set; } = false;

        /// <summary>
        /// Any package references currently in the instance...
        /// </summary>
        [JsonIgnore]
        public List<PackageReference> Packages { get; protected set; }
    }
}
