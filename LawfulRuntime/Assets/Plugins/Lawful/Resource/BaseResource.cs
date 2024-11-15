using UnityEngine;

namespace Lawful.Resource
{
    /// <summary>
    /// Base class for all lawful resources
    /// </summary>
    /// <typeparam name="T">Determines the internal data type of the resource</typeparam>
    public abstract class BaseResource<T>
    {
        /// <summary>Current state of the resource</summary>
        public ResourceState ResourceState   { get; set; }

        /// <summary>Where the resource came from</summary>
        public ResourceSource ResourceSource { get; set; }

        /// <summary>Either a relative file system path, or asset bundle path</summary>
        public string ResourceOrigin         { get; set; }

        /// <summary>The amount of references to the resource</summary>
        public int ReferenceCount            { get; set; }

        /// <summary>The actual final, ready to use resource</summary>
        public T resource                    { get; protected set; }
    }
}
