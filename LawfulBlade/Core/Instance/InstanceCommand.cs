using System.Text.Json.Serialization;

namespace LawfulBlade.Core
{
    public struct InstanceCommand
    {
        // regex to extract vars...
        // #var:([A-Za-z]+[A-Za-z0-9]*);
        // Avaliable Vars:
        //   instancePath, projectPath, programPath

        [JsonInclude]
        public string   Execute;  // The command to run when launched...

        [JsonInclude]
        public string[] Arguments;
        // #var:instancePath;\tool\SOM_MAIN.exe
    }
}
