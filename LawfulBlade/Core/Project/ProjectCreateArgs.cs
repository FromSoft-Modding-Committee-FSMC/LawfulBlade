using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawfulBlade.Core
{
    public struct ProjectCreateArgs
    {
        public string Name;
        public string Description;
        public string Author;
        public string IconFile;
        public Instance Owner;
    }
}
