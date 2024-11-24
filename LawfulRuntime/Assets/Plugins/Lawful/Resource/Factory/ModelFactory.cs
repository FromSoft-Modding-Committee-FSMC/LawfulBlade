using System.IO;
using System;

using Lawful.Resource.FileFormat.MDO;
using Lawful.Resource.FileFormat;
using Lawful.Utility;
using Lawful.IO;


namespace Lawful.Resource.Factory
{
    public class ModelFactory : ResourceFactory<ModelResource>
    {
        /// <inheritdoc/>
        public ModelFactory() : base()
        {
            RegisterFormatHandler(new MDOFormatHandler());
        }

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

            // Try to find a loader for this resource
            FileFormatHandler<ModelResource> handler = GetFormatHandler(Path.GetExtension(path)) ?? throw new Exception($"Couldn't find format handler for '{path}'!");

            // Create the stream we will use to load the file
            using FileInputStream finStream = new(absolutePath);

            // Now we can validate it
            if (!handler.Validate(finStream))
                throw new Exception($"Failed to import '{path}' using handler '{handler.Metadata.name}'!");

            // Create the new resource 
            ModelResource resource = new()
            {
                ResourceState  = ResourceState.Unloaded,
                ResourceSource = ResourceSource.FileSystem,
                ResourceOrigin = path,
                ReferenceCount = 0
            };

            if (!handler.Load(finStream, resource))
                throw new Exception($"Failed to import '{path}' using handler '{handler.Metadata.name}'!\nUnknown error.");

            // Store the resource in our cache
            resourceCache[name] = resource;

            return name;
        }
    }
}