namespace Lawful.Resource
{
    /// <summary>
    /// Resource state tells us if a resource is ready to be used or not.
    /// </summary>
    public enum ResourceState
    {
        /// <summary>The resource has not yet been loaded</summary>
        Unloaded,

        /// <summary>The resource is ready for transfer</summary>
        WaitingForTransfer,

        /// <summary>The resource is ready for use</summary>
        Ready
    }
}
