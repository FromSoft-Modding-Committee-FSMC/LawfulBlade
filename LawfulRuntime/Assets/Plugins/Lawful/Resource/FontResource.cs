using TMPro;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;

namespace Lawful.Resource
{
    public class FontResource : BaseResource<TMP_FontAsset>
    {
        public void LoadFont(string fontPath)
        {
            resource = TMP_FontAsset.CreateFontAsset(fontPath, 0, 96, 8, GlyphRenderMode.SDFAA, 1024, 1024);

            ResourceState = ResourceState.WaitingForTransfer;
        }

        /// <summary>
        /// Grab the internal resource and return it.<br/>
        /// If the resource is waiting for transfer, it will be transferred first so expect some delay.
        /// </summary>
        /// <returns>The resource.</returns>
        public override TMP_FontAsset Get()
        {
            ReferenceCount++;

            // If the resource is ready, return it immediately
            if (ResourceState == ResourceState.Ready)
                return resource;

            // If the resource is not ready, and is waiting for transfer 
            if (ResourceState == ResourceState.WaitingForTransfer)
            {
                ResourceState = ResourceState.Ready;
                return resource;
            }

            // Return null or a default in any other case
            return null;
        }
    }
}
