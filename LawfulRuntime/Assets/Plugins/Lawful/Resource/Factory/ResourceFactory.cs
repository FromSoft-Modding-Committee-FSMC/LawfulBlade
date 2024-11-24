using System.Collections.Concurrent;
using System.Collections.Generic;
using System;

using Lawful.Resource.FileFormat;

namespace Lawful.Resource.Factory
{
    /// <summary>
    /// A resource factory is responsible for handling import, export and storage of resources in a specific format
    /// </summary>
    public abstract class ResourceFactory<T>
    {
        protected List<FileFormatHandler<T>> registeredFormatHandlers;
        protected ConcurrentDictionary<ulong, T> resourceCache;

        /// <summary>
        /// Default constructor.<br/>
        /// Will create default containers.
        /// </summary>
        protected ResourceFactory()
        {
            registeredFormatHandlers = new List<FileFormatHandler<T>>();
            resourceCache = new ConcurrentDictionary<ulong, T>();
        }

        /// <summary>
        /// Enumurates all registered format handlers which match the filter provided.
        /// </summary>
        /// <param name="filter">The filter to use.</param>
        /// <returns></returns>
        public List<FileFormatHandler<T>> EnumerateFormatHandlers(FileFormatFilter filter = FileFormatFilter.AllSafe)
        {
            // We accumulate our format handlers that match the filter in this list
            List<FileFormatHandler<T>> handlers = new();
            
            // Scan through our registered format handlers and find ones that match our filter...
            foreach(FileFormatHandler<T> handler in registeredFormatHandlers)
            {
                // Check import capabilities vs filter. Inclusive.
                if (!handler.Capabilities.allowImport && ((filter & FileFormatFilter.Importable) > 0))
                    continue;

                // Check export capabilities vs filter. Inclusive.
                if (!handler.Capabilities.allowExport && ((filter & FileFormatFilter.Exportable) > 0))
                    continue;

                // Check deprecated vs filter. Exclusive.
                if (handler.Capabilities.deprecated && ((filter & FileFormatFilter.Deprecated) == 0))
                    continue;

                // Check experimental vs filter. Exclusive.
                if (handler.Capabilities.experimental && ((filter & FileFormatFilter.Experimental) == 0))
                    continue;

                // Now the handler has passed all checks, it can be appended to the list
                handlers.Add(handler);
            }

            return handlers;
        }

        /// <summary>
        /// Registers a format handler.
        /// </summary>
        /// <param name="formatHandler">The format handler to register</param>
        public void RegisterFormatHandler(FileFormatHandler<T> formatHandler) =>
            registeredFormatHandlers.Add(formatHandler);

        /// <summary>
        /// Tries to find a handler for a format with the extension given.
        /// </summary>
        /// <param name="extension">The common extension of the format e.g. '.tga', '.wav'</param>
        /// <returns>The handler if one can be found, or null if it can not.</returns>
        public FileFormatHandler<T> GetFormatHandler(string extension)
        {
            // Scan through our list of registered handlers looking for the extension
            foreach (FileFormatHandler<T> handler in registeredFormatHandlers)
            {
                foreach(string handlerExtension in handler.Metadata.extensions)
                {
                    if (handlerExtension.Equals(extension, StringComparison.InvariantCultureIgnoreCase))
                        return handler;
                }
            }

            // Failed.
            return null;
        }

        /// <summary>
        /// Gets a resource from the resource cache
        /// </summary>
        /// <param name="name">The name of the resource</param>
        public T Get(ulong name) =>
            resourceCache[name];

        /// <summary>
        /// Frees a resource from the resource cache
        /// </summary>
        /// <param name="name"></param>
        public void Free(ulong name) =>
            throw new NotImplementedException();

        /// <summary>
        /// Returns the amount of assets currently stored in the cache
        /// </summary>
        public int GetCacheSize() =>
            resourceCache.Count;
    }
}