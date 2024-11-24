using UnityEngine;
using Unity.Collections;

namespace Lawful.Resource
{
    public class TextureResource : BaseResource<Texture2D>
    {
        public NativeArray<byte> PixelBuffer { get; private set; }
        public int width;
        public int height;

        /// <summary>
        /// Load a sample buffer into the audio resource.
        /// </summary>
        public void LoadPixels(NativeArray<byte> pixelBuffer, int width, int height)
        {
            PixelBuffer = pixelBuffer;
            this.width  = width;
            this.height = height;

            ResourceState = ResourceState.WaitingForTransfer;
        }

        /// <summary>
        /// Grab the internal resource and return it.<br/>
        /// If the resource is waiting for transfer, it will be transferred first so expect some delay.
        /// </summary>
        /// <returns>The resource.</returns>
        public override Texture2D Get()
        {
            ReferenceCount++;

            // If the resource is ready, return it immediately
            if (ResourceState == ResourceState.Ready)
                return resource;

            // If the resource is not ready, and is waiting for transfer 
            if (ResourceState == ResourceState.WaitingForTransfer)
            {
                // Create the unity resource
                try
                {
                    resource = new Texture2D(width, height, TextureFormat.RGBA32, false, false, true);
                    resource.SetPixelData(PixelBuffer, 0);
                    resource.filterMode = FilterMode.Point;

                    resource.Apply();
                }
                catch
                {
                    // Handle our error by forcing the resource state to unloaded
                    ResourceState = ResourceState.Unloaded;

                    // rethrow the exception without changing the stack location
                    throw;
                }

                // We can free our native memory ?
                PixelBuffer.Dispose();
                ResourceState = ResourceState.Ready;

                return resource;
            }

            // Return null or a default in any other case
            return null;
        }
    }
}
