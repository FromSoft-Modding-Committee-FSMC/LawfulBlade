using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LawfulBlade.Core.Package
{
    public struct PackageFile
    {
        [JsonInclude]
        public string Path;

        [JsonInclude]
        public ulong FNV64;
    }
}
