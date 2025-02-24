using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawfulBlade.Core.Package
{
    public struct PackageReference
    {
        /// <summary>The UUID of the package</summary>
        public string UUID { get; private set; }

        /// <summary>The version of the package</summary>
        public string Version { get; private set; }
    }
}
