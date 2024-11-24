using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

using UnityEngine;

using Lawful.Resource.Factory;


namespace Lawful.Resource
{
    /// <summary>
    /// Generic implementation of a resource manager
    /// </summary>
    public static class ResourceManager
    {
        // Paths
        public static readonly string GamePath    = Path.Combine(Path.GetFullPath(Application.streamingAssetsPath));

        // Factory References
        static readonly AudioFactory audioFactory;
        static readonly TextureFactory textureFactory;
        static readonly ModelFactory modelFactory;
        static readonly FontFactory fontFactory;

        /// <summary>
        /// Default Constructor.<br/>
        /// </summary>
        static ResourceManager()
        {
            // Initialize our factories
            audioFactory = new AudioFactory();
            textureFactory = new TextureFactory();
            modelFactory = new ModelFactory();
            fontFactory = new FontFactory();
        }

        /// <summary>
        /// Load a file.
        /// </summary>
        /// <typeparam name="T">The resource type to load</typeparam>
        /// <param name="path">The relative path we expect to find the resource at</param>
        public static ulong Load<T>(string path)
        {
            // Truly ingenious code
            switch (typeof(T))
            {
                case Type audioResource when audioResource == typeof(AudioResource):
                    return audioFactory.Load(path);

                case Type textureResource when textureResource == typeof(TextureResource):
                    return textureFactory.Load(path);

                case Type modelResource when modelResource == typeof(ModelResource):
                    return modelFactory.Load(path);

                case Type fontResource when fontResource == typeof(FontResource):
                    return fontFactory.Load(path);
            }

            throw new Exception("Invalid resource type!");
        }

        /// <summary>
        /// Load a file.
        /// </summary>
        /// <typeparam name="T">The resource type to load</typeparam>
        /// <param name="path">The relative path we expect to find the resource at</param>
        public static async void LoadAsync<T>(string path, Action<ulong> onComplete = null)
        {
            Task<ulong> LoadingTask;

            // Do all our format wrangling on a background thread
            try
            {
                LoadingTask = new(() => Load<T>(path));
                LoadingTask.Start();

                await LoadingTask;
            } catch
            {
                throw;
            }

            // We can now execute the users provided callback
            onComplete?.Invoke(LoadingTask.Result);
        }

        /// <summary>
        /// Get a file
        /// </summary>
        /// <typeparam name="T">The resource type to load</typeparam>
        /// <param name="name">The returned name from loading a file</param>
        public static T Get<T>(ulong name)
        {
            // also truly ingenious code... On a serious note, this is safer than it looks. We _know_ the return is the type we want, so C# complaining when we cast
            // directly to T is silly. Undefined behaviour?
            switch (typeof(T))
            {
                case Type audioResource when audioResource == typeof(AudioResource):
                    return (T)(object)audioFactory.Get(name);

                case Type textureResource when textureResource == typeof(TextureResource):
                    return (T)(object)textureFactory.Get(name);

                case Type modelResource when modelResource == typeof(ModelResource):
                    return (T)(object)modelFactory.Get(name);

                case Type fontResource when fontResource == typeof(FontResource):
                    return (T)(object)fontFactory.Get(name);
            }

            throw new Exception("Invalid resource type!");
        }
    }
}
