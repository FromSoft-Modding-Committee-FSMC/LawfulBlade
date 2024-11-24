using System.IO;
using System;

using Lawful.Utility;

namespace Lawful.Resource.Factory
{
    public class FontFactory : ResourceFactory<FontResource>
    {
        public ulong Load(string path)
        {
            // Calculate our hash name
            ulong name = HashThis.StringTo64(path);

            // Does this resource already exist in the cache? If so, return it without doing anything else.
            if (resourceCache.ContainsKey(name))
                return name;

            // Lets create our absolute path now
            string absolutePath = Path.Combine(ResourceManager.GamePath, path);

            if (!File.Exists(absolutePath))
                throw new Exception($"Failed to import '{path}' located '{absolutePath}'!\nFile does not exist");

            // Create the new resource 
            FontResource resource = new()
            {
                ResourceState  = ResourceState.Unloaded,
                ResourceSource = ResourceSource.FileSystem,
                ResourceOrigin = path,
                ReferenceCount = 0
            };

            // Try loading the font on the main thread...
            resource.LoadFont(absolutePath);

            // Store the resource in our cache
            resourceCache[name] = resource;

            return name;
        }
    }
}