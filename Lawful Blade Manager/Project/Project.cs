using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LawfulBladeManager.Project
{
    public struct Project
    {
        [JsonInclude]
        public string Name;

        [JsonInclude]
        public string Description;

        [JsonInclude]
        public string Author;

        [JsonInclude]
        public string InstanceUUID;

        [JsonInclude]
        public string LastEditData;

        [JsonInclude]
        public string StoragePath;

        [JsonInclude]
        public bool IsManaged;

        [JsonInclude]
        public List<int> TagIDs;
    }
}
