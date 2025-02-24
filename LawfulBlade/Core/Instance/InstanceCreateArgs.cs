using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawfulBlade.Core.Instance
{
    public struct InstanceCreateArgs
    {
        public string Name;
        public string Description;
        public string IconFilePath;
        public string[] Tags;
        public string CorePackageUUID;
    }
}
