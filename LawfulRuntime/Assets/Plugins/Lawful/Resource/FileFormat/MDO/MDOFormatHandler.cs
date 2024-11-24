using System;

using UnityEngine;
using Unity.Collections;

using Lawful.IO;
using System.Text;
using System.IO;
using UnityEngine.Rendering;
using System.Collections.Generic;

namespace Lawful.Resource.FileFormat.MDO
{
    public class MDOFormatHandler : FileFormatHandler<ModelResource>
    {
        public override FileFormatCapabilities Capabilities => new()
        {
            allowExport  = false,
            allowImport  = true,
            deprecated   = false,   // Sadly...
            experimental = false    
        };

        public override FileFormatMetadata Metadata => new()
        {
            name        = "Sword of Moonlight [M]o[D]el [O]bject (*.MDO)",
            description = "Proprietary model file format created for Sword of Moonlight: King's Field Making Tool",
            version     = "1.0",
            authors     = new string[] { "FromSoftware" },
            extensions  = new string[] { ".MDO" }
        };

        /// <summary>
        /// Validates the content of a stream as an MDO file.
        /// </summary>
        /// <param name="finStream">A stream containing the data to check</param>
        /// <returns>True if it is, false if it is not</returns>
        public override bool Validate(FileInputStream finStream) => true;   // TO-DO

        /// <summary>
        /// Parses an MDO file
        /// </summary>
        public override bool Load(FileInputStream finStream, in ModelResource resource)
        {
            // The stream is reused from the validation pass, so it's good practice to seek to the start
            finStream.SeekBegin(0);

            //
            // Reading
            //

            // Get the Shift-JIS encoding. This could be slow, and could be cached somewhere? Does every PC support this code page?..
            Encoding shiftJis = Encoding.GetEncoding(932);

            // Texture File References
            int textureCount      = finStream.ReadS32();

            string[] textureFiles = new string[textureCount];
            for (int i = 0; i < textureCount; ++i)
            {
                // SOM has this annoying shit bag habit of storing a BMP extension for fucking TXR files!!! WHY FROMSOFT!!!!!!!!
                textureFiles[i] = Path.ChangeExtension(finStream.ReadTerminatedString(shiftJis), "txr"); 
            }     

            finStream.Align(4); // We must align to 4 after reading the texture file data.

            // Materials
            int materialCount = finStream.ReadS32();

            MDOMaterial[] materials = new MDOMaterial[materialCount];
            for (int i = 0; i < materialCount; ++i)
            {
                materials[i] = new MDOMaterial
                {
                    diffuseR = finStream.ReadF32(),
                    diffuseG = finStream.ReadF32(),
                    diffuseB = finStream.ReadF32(),
                    diffuseA = finStream.ReadF32(),
                    emissiveR = finStream.ReadF32(),
                    emissiveG = finStream.ReadF32(),
                    emissiveB = finStream.ReadF32(),
                    emissiveX = finStream.ReadF32()
                };
            }

            // Control Points - Skip for now
            Vector3[] controlPoints = new Vector3[4];
            for (int i = 0; i < 4; ++i)
                controlPoints[i] = finStream.ReadVector3();

            // Meshes
            int meshCount = finStream.ReadS32();
            MDOMesh[] meshes = new MDOMesh[meshCount];
            for (int i = 0; i < meshCount; ++i)
            {
                MDOMesh mesh = new MDOMesh
                {
                    renderFlags = finStream.ReadU32(),
                    textureID = finStream.ReadS16(),
                    materialID = finStream.ReadS16(),
                    indexCount = finStream.ReadU16(),
                    vertexCount = finStream.ReadU16(),
                    indicesOffset = finStream.ReadU32(),
                    verticesOffset = finStream.ReadU32()
                };

                // Read Mesh Indices
                finStream.Jump(mesh.indicesOffset);
                mesh.indices = finStream.ReadU16Array(mesh.indexCount);

                finStream.Return();

                // Read mesh vertices
                finStream.Jump(mesh.verticesOffset);
                mesh.vertices = new MDOVertex[mesh.vertexCount];

                for (int j = 0; j < mesh.vertexCount; ++j)
                {
                    mesh.vertices[j] = new MDOVertex
                    {
                        position = finStream.ReadVector3(),
                        normal   = finStream.ReadVector3(),
                        texcoord = finStream.ReadVector2()
                    };
                }
                finStream.Return();

                meshes[i] = mesh;
            }

            //
            // Parsing - this is likely super slow
            //

            List<ModelResource.MeshVertex> meshVertices = new List<ModelResource.MeshVertex>();
            List<ModelResource.PoseVertex> poseVertices = new List<ModelResource.PoseVertex>();
            List<uint> indices = new List<uint>();

            ModelResource.MeshDefinition[] meshDefinitions = new ModelResource.MeshDefinition[meshes.Length];

            for (int i = 0; i < meshes.Length; ++i)
            {
                // Grab our mesh...
                MDOMesh mesh = meshes[i];

                // Create our mesh definition
                meshDefinitions[i] = new ModelResource.MeshDefinition
                {
                    startIndex = indices.Count,
                    indexCount = mesh.indexCount,
                    materialID = 0                  // 0 for now...
                };

                // Now we do das dirty data dance of indices
                for (int j = 0; j < mesh.indexCount; ++j)
                    indices.Add((uint)(meshVertices.Count + mesh.indices[j]));

                // Our vertices need some special logic to seperate mesh and pose components...
                for (int j = 0; j < mesh.vertexCount; ++j)
                {
                    meshVertices.Add(new ModelResource.MeshVertex
                    {
                        texcoord    = mesh.vertices[j].texcoord,    // MDO doesn't have most of these components...
                        colour      = new Vector4(1.0f, 0.5f, 0.5f, 1.0f),
                        boneIndex1  = 0, boneIndex2 = 0, boneIndex3 = 0, boneIndex4 = 0,
                        boneWeights = new Vector4(0.0f, 0.0f, 0.0f, 0.0f)
                    });

                    poseVertices.Add(new ModelResource.PoseVertex
                    {
                        position = mesh.vertices[j].position,
                        normal   = mesh.vertices[j].normal
                    });
                }
            }

            //
            // Storing - likely super non concrete API
            //
            resource.indexBuffer = indices.ToArray();
            resource.genericPose = meshVertices.ToArray();
            resource.bindPose    = poseVertices.ToArray();

            resource.meshDefinitions = meshDefinitions;

            resource.SubmisionComplete();

            return true;
        }

        /// <summary>
        /// Material layout for MDO files
        /// </summary>
        struct MDOMaterial
        {
            public float diffuseR;
            public float diffuseG;
            public float diffuseB;
            public float diffuseA;
            public float emissiveR;
            public float emissiveG;
            public float emissiveB;
            public float emissiveX;
        }
        
        /// <summary>
        /// Vertex layout for MDO files
        /// </summary>
        struct MDOVertex
        {
            public Vector3 position;
            public Vector3 normal;
            public Vector2 texcoord;
        }

        /// <summary>
        /// Mesh layout for MDO files
        /// </summary>
        struct MDOMesh
        {
            public uint renderFlags;
            public short textureID;
            public short materialID;
            public ushort indexCount;
            public ushort vertexCount;
            public uint indicesOffset;
            public uint verticesOffset;

            public ushort[] indices;
            public MDOVertex[] vertices;
        }
    }
}