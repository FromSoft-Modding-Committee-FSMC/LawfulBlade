using System.IO;
using System;

using UnityEngine;

using Lawful.Resource.FileFormat;
using Lawful.Utility;
using Lawful.IO;
using System.Threading.Tasks;

namespace Lawful.Resource.Factory
{
    public class AudioFactory : ResourceFactory<AudioResource>
    {

        /// <summary>
        /// Factory constructor initializes the resource factory and registers our handlers...
        /// </summary>
        public AudioFactory() : base()
        {
            
        }

        /// <summary>
        /// Loads a resource synchronously
        /// </summary>
        /// <param name="path">The relative path to the resource</param>
        /// <returns>internal name of the resource</returns>
        public ulong Load(string path)
        {
            // Calculate our hash name
            ulong name = HashThis.StringTo64(path);

            // Does this resource already exist in the cache? If so, return it without doing anything else.
            if (resourceCache.ContainsKey(name))
                return name;

            // Try to find a loader for this resource
            FileFormatHandler<AudioResource> handler = GetFormatHandler(Path.GetExtension(path)) ?? throw new Exception($"Couldn't find format handler for '{path}'!"); ;

            // Create the new resource 
            AudioResource resource = new ()
            {
                ResourceState  = ResourceState.Unloaded,
                ResourceSource = ResourceSource.FileSystem,
                ResourceOrigin = path,
                ReferenceCount = 1
            };

            using FileInputStream finStream = new (Path.Combine(Path.GetFullPath(Application.streamingAssetsPath), path));

            if (!handler.Load(finStream, resource))
                throw new Exception($"Failed to import '{path}' using handler '{handler.Metadata.name}'!");

            // Store the resource in our cache
            resourceCache[name] = resource;

            return name;
        }

        public async Task LoadAsync(string path, Action<ulong> onComplete = null)
        {
            // Do all our format wrangling on a background thread
            Task<ulong> LoadingTask = new(() =>
            {
                return Load(path);
            });
            
            await LoadingTask;

            // We can now execute the users provided callback
            onComplete?.Invoke(LoadingTask.Result);
        }
    }
}