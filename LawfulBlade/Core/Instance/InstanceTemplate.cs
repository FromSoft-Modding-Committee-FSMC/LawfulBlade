using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LawfulBlade.Core
{
    public struct InstanceTemplate
    {
        [JsonInclude]
        public string Type;

        [JsonInclude]
        public string[] Content;
    }
}
