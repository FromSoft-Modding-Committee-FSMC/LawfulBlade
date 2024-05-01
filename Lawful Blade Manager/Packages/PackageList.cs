using System.Text.Json.Serialization;

namespace LawfulBladeManager.Packages
{
    public struct PackageList
    {
        [JsonInclude]
        PackageListEntry[] packages;
    }
}
