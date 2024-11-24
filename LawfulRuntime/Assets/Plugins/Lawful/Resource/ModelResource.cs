using UnityEngine;
using UnityEngine.Rendering;
using System.Runtime.InteropServices;

namespace Lawful.Resource
{
    public class ModelResource : BaseResource<Mesh>
    {
        /// <summary>
        /// Pose vertex format
        /// </summary>
        readonly static VertexAttributeDescriptor[] VertexFormat =
        {
            new VertexAttributeDescriptor(VertexAttribute.Position,     VertexAttributeFormat.Float32, 3, 0),   // Position #1
            new VertexAttributeDescriptor(VertexAttribute.Normal,       VertexAttributeFormat.Float32, 3, 0),   // Normal #1

            new VertexAttributeDescriptor(VertexAttribute.TexCoord4,    VertexAttributeFormat.Float32, 3, 1),   // Position #2
            new VertexAttributeDescriptor(VertexAttribute.TexCoord5,    VertexAttributeFormat.Float32, 3, 1),   // Normal #2

            new VertexAttributeDescriptor(VertexAttribute.TexCoord0,    VertexAttributeFormat.Float32, 2, 2),   // Texture Coordinates
            new VertexAttributeDescriptor(VertexAttribute.TexCoord1,    VertexAttributeFormat.Float32, 4, 2),   // Colour
            new VertexAttributeDescriptor(VertexAttribute.TexCoord6,    VertexAttributeFormat.UInt32,  4, 2),   // Bone Indices
            new VertexAttributeDescriptor(VertexAttribute.TexCoord7,    VertexAttributeFormat.Float32, 4, 2)    // Bone Weights
        };

        /// <summary>
        /// Vertex used for defining the base pose and frames
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct PoseVertex
        {
            public Vector3 position;
            public Vector3 normal;
        }

        /// <summary>
        /// Vertex used to define basic per vertex mesh properties
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct MeshVertex
        {
            public Vector2 texcoord;
            public Vector4 colour;
            public uint boneIndex1, boneIndex2, boneIndex3, boneIndex4;
            public Vector4 boneWeights;
        }

        /// <summary>
        /// Used to define submeshes inside the model
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct MeshDefinition
        {
            public int startIndex;
            public int indexCount;
            public int materialID;   
        }

        /// <summary>Store the generic pose information of the model (texcoord, colour etc...)</summary>
        public MeshVertex[] genericPose;

        /// <summary>Store the initial frame of the model...</summary>
        public PoseVertex[] bindPose;

        /// <summary>Stores an index buffer of the model</summary>
        public uint[] indexBuffer;

        public MeshDefinition[] meshDefinitions;

        public void SubmisionComplete() =>
            ResourceState = ResourceState.WaitingForTransfer;

        /// <summary>
        /// Grab the internal resource and return it.<br/>
        /// If the resource is waiting for transfer, it will be transferred first so expect some delay.
        /// </summary>
        /// <returns>The resource.</returns>
        public override Mesh Get()
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
                    resource = new Mesh();

                    // Load Bind Pose #1, Bind Pose #2, Generic Pose
                    resource.SetVertexBufferParams(bindPose.Length, VertexFormat);
                    resource.SetVertexBufferData(bindPose, 0, 0, bindPose.Length, 0);
                    resource.SetVertexBufferData(bindPose, 0, 0, bindPose.Length, 1);
                    resource.SetVertexBufferData(genericPose, 0, 0, genericPose.Length, 2);

                    // Load the index buffer data
                    resource.SetIndexBufferParams(indexBuffer.Length, IndexFormat.UInt32);
                    resource.SetIndexBufferData(indexBuffer, 0, 0, indexBuffer.Length, MeshUpdateFlags.Default);

                    // Load our submesh definitions
                    resource.subMeshCount = meshDefinitions.Length;
                    for (int i = 0; i < resource.subMeshCount; ++i)
                        resource.SetSubMesh(i, new SubMeshDescriptor(meshDefinitions[i].startIndex, meshDefinitions[i].indexCount, MeshTopology.Triangles));

                    ResourceState = ResourceState.Ready;

                    // resource.UploadMeshData(false);
                }
                catch
                {
                    // Handle our error by forcing the resource state to unloaded
                    ResourceState = ResourceState.Unloaded;

                    // rethrow the exception without changing the stack location
                    throw;
                }

                // We can free our native memory ?
                ResourceState = ResourceState.Ready;

                return resource;
            }

            // Return null or a default in any other case
            return null;
        }
    }
}
