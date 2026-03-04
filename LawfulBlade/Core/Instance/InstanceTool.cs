using System.Text.Json.Serialization;

namespace LawfulBlade.Core
{
    public struct InstanceTool
    {
        /// <summary>The path to the command in the instance context menu</summary>
        [JsonInclude]
        public string MenuPath;

        /// <summary>The command to execute when running the tool</summary>
        [JsonInclude]
        public InstanceToolCommand Command;
    }

    public struct InstanceToolCommand
    {
        /// <summary>Executable path relative to the root of the instance</summary>
        [JsonInclude]
        public string Executable;

        /// <summary>An array of arguments to pass to the tool</summary>
        [JsonInclude]
        public string[] Arguments;
    }
}
