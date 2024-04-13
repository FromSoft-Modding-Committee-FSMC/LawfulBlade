using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LawfulBladeManager.Package
{
    public struct PackageListEntry
    {
        [JsonInclude]
        public string Name;

        [JsonInclude]
        public string SourceUri;
    }
}
