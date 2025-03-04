using System;

using UnityEngine;
using Unity.Collections;

namespace Lawful.Resource
{
    public class AudioResource : BaseResource<AudioClip>
    {
        /// <summary>The buffer containing samples</summary>
        public NativeArray<float> SampleBuffer { get; private set; }

        /// <summary>Sample Rate of the audio clip</summary>
        public int SampleRate                  { get; private set; }

        /// <summary>The number of channels </summary>
        public int ChannelCount                { get; private set; }

        /// <summary>
        /// Load a sample buffer into the audio resource.
        /// </summary>
        /// <param name="sampleBuffer">A native array of samples</param>
        /// <param name="sampleRate">The sample rate</param>
        /// <param name="channelCount">The channel count</param>
        public void LoadSamples(NativeArray<float> sampleBuffer, int sampleRate, int channelCount)
        {
            SampleBuffer = sampleBuffer;
            SampleRate   = sampleRate;
            ChannelCount = channelCount;

            ResourceState = ResourceState.WaitingForTransfer;
        }

        /// <summary>
        /// Grab the internal resource and return it.<br/>
        /// If the resource is waiting for transfer, it will be transferred first so expect some delay.
        /// </summary>
        /// <returns>The resource.</returns>
        public override AudioClip Get()
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
                    resource = AudioClip.Create(ResourceOrigin, SampleBuffer.Length, ChannelCount, SampleRate, false);
                    resource.SetData(SampleBuffer, 0);
                } 
                catch
                {
                    // Handle our error by forcing the resource state to unloaded
                    ResourceState = ResourceState.Unloaded;

                    // rethrow the exception without changing the stack location
                    throw;
                }

                // We can free our native memory ?
                SampleBuffer.Dispose();
                ResourceState = ResourceState.Ready;

                return resource;
            }

            // Return null or a default in any other case
            return null;
        }
    }
}
