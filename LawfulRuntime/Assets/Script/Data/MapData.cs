using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using Lawful.IO;

[CreateAssetMenu(fileName = "MapData", menuName = "Lawful Runtime/Map Data")]
public class MapData : ScriptableObject
{
    [field: SerializeField] public List<MapFile> GameMaps { get; private set; }
    [field: SerializeField] public Dictionary<int, int> GameMapIDToIndex { get; private set; }

    public void LoadMapFiles()
    {
        // The base path for our map files
        string baseMapDir = Path.Combine(ResourceManager.GameDataPath, "MAP");

        // Create new list and dictionary for data storage
        GameMapIDToIndex = new Dictionary<int, int>();
        GameMaps = new List<MapFile>();

        for (int i = 0; i < 64; ++i)
        {
            // Create our paths for each potential map file
            string mapFileMPX = Path.Combine(baseMapDir, $"{i:D2}.mpx");
            string mapFileEVT = Path.Combine(baseMapDir, $"{i:D2}.evt");

            // Now we can check if they exist. If they don't, it's not an error - just that the map does not exist.
            if (!File.Exists(mapFileMPX) || !File.Exists(mapFileEVT))
                continue;

            // Construct our map object, and load the map.
            MapFile loadedMap = new MapFile
            {
                // Basic Map Information we already know at runtime...
                ID = i
            };

            // The actual load function. We could delay this later to only run when we actually try to access the map?..
            loadedMap.Load(mapFileMPX, mapFileEVT);

            // Store the map...
            GameMapIDToIndex.Add(i, GameMaps.Count);
            GameMaps.Add(loadedMap);

            break;
        }
    }
}

[Serializable]
public class MapFile
{
    [field: Header("Properties")]
    [field: SerializeField] public int ID { get; set; }
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public string SourceMPXFile { get; private set; }
    [field: SerializeField] public string SourceEVTFile { get; private set; }
    [field: SerializeField] public string IngameMusic  { get; private set; }
    [field: SerializeField] public MapPlayerStart playerStart { get; private set; }

    [field: Header("Camera Settings")]
    [field: SerializeField] public float cameraFoV { get; private set; }
    [field: SerializeField] public float cameraZNear { get; private set; }
    [field: SerializeField] public float cameraZFar { get; private set; }

    [field: Header("Sky Settings")]
    [field: SerializeField] public int skyMeshID { get; private set; }

    [field: Header("Fog Settings")]
    [field: SerializeField] public float fogDistance { get; private set; }
    [field: SerializeField] public Color fogColour { get; private set; }

    [field: Header("Lighting Settings")]
    [field: SerializeField] public Color ambientLight { get; private set; }
    [field: SerializeField] public MapDirectionalLight[] directionalLights { get; private set; }

    [field: Header("Tilemap")]
    [field: SerializeField] public MapTileLayer tilemap { get; private set; }

    [field: Header("Mesh Data")]
    [field: SerializeField] public string[] tileTextures { get; private set; }

    [field: Header("Map In-game Images")]
    [field: SerializeField] public string IngameImage1 { get; private set; }
    [field: SerializeField] public string IngameImage2 { get; private set; }
    [field: SerializeField] public string IngameImage3 { get; private set; }

    [field: Header("Instances")]
    [field: SerializeField] public List<MapObjectInstance> ObjectInstances;
    [field: SerializeField] public List<MapEnemyInstance> EnemyInstances;
    [field: SerializeField] public List<MapCharacterInstance> CharacterInstances;
    [field: SerializeField] public List<MapItemInstance> ItemInstances;

    public void Load(string mpxFile, string evtFile)
    {
        // Store paths locally
        SourceMPXFile = mpxFile;
        SourceEVTFile = evtFile;

        // Import our MPX data, general map properties
        ImportMPXData();

        // Import our EVT data, events and such
        ImportEVTData();
    }

    void ImportMPXData()
    {
        if (!File.Exists(SourceMPXFile))
            throw new Exception("Missing map MPX file!");

        // Open the MPX file for reading
        using FileInputStream ins = new (SourceMPXFile);

        // Basic Header
        uint mpxFlags = ins.ReadU32();
        Name = ins.ReadFixedString(32);

        // Music File
        IngameMusic = ins.ReadFixedString(32);

        // Ingame Map Images
        IngameImage1 = ins.ReadFixedString(32);
        IngameImage2 = ins.ReadFixedString(32);
        IngameImage3 = ins.ReadFixedString(32);

        // Camera Settings
        cameraFoV = ins.ReadF32();
        cameraZNear = ins.ReadF32();
        cameraZFar = ins.ReadF32();

        // Fog Settings
        fogDistance = ins.ReadF32();
        fogColour = ins.ReadColourRGBX32();

        // Lighting
        ambientLight = ins.ReadColourRGBX32();

        directionalLights = new MapDirectionalLight[3];
        for (int i = 0; i < 3; ++i)
        {
            directionalLights[i] = new MapDirectionalLight
            {
                colour    = ins.ReadColourRGBX32(),
                direction = ins.ReadVector3()
            };
        }

        // Padding?
        ins.ReadU32();  // 4 bytes of padding

        // Player Start Position
        playerStart = new MapPlayerStart
        {
            position   = ins.ReadVector3(),
            zDirection = ins.ReadF32()
        };

        //
        // Objects Block
        //
        ObjectInstances = new List<MapObjectInstance>();

        uint objectInstanceCount = ins.ReadU32();

        for (int i = 0; i < objectInstanceCount; ++i)
        {
            ObjectInstances.Add(new MapObjectInstance
            {
                classID = ins.ReadS16(),
                u8x02 = ins.ReadU8(),
                isAnimated = (ins.ReadU8() != 0),
                isVisible = (ins.ReadU8() != 0),
                u8x05 = ins.ReadU8(),
                u8x06 = ins.ReadU8(),
                u8x07 = ins.ReadU8(),
                position = ins.ReadVector3(),
                rotation = ins.ReadVector3(),
                uniformScale = ins.ReadF32(),
                flags = ins.ReadU8Array(32)
            });
        }

        //
        // Enemy Block
        //
        EnemyInstances = new List<MapEnemyInstance>();

        uint enemyInstanceCount = ins.ReadU32();

        for (int i = 0; i < enemyInstanceCount; ++i)
        {
            EnemyInstances.Add(new MapEnemyInstance
            {
                u8x00 = ins.ReadU8(),
                u8x01 = ins.ReadU8(),
                u8x02 = ins.ReadU8(),
                u8x03 = ins.ReadU8(),
                position = ins.ReadVector3(),
                rotation = ins.ReadVector3(),
                uniformScale = ins.ReadF32(),
                classID = ins.ReadS16(),
                flags = ins.ReadU8Array(18)
            });
        }

        //
        // Character Block
        //
        CharacterInstances = new List<MapCharacterInstance>();

        uint characterInstanceCount = ins.ReadU32();

        for (int i = 0; i < characterInstanceCount; ++i)
        {
            CharacterInstances.Add(new MapCharacterInstance
            {
                u8x00 = ins.ReadU8(),
                u8x01 = ins.ReadU8(),
                u8x02 = ins.ReadU8(),
                u8x03 = ins.ReadU8(),
                position = ins.ReadVector3(),
                rotation = ins.ReadVector3(),
                uniformScale = ins.ReadF32(),
                classID = ins.ReadS16(),
                flags = ins.ReadU8Array(18)
            });
        }

        //
        // Item Block
        //
        ItemInstances = new List<MapItemInstance>();

        uint itemInstanceCount = ins.ReadU32();

        for (int i = 0; i < itemInstanceCount; ++i)
        {
            ItemInstances.Add(new MapItemInstance
            {
                unkx00 = ins.ReadU8Array(4),
                position = ins.ReadVector3(),
                rotation = ins.ReadVector3(),
                classID = ins.ReadS16(),
                flags = ins.ReadU8Array(10)
            });
        }

        //
        // Skybox
        //
        skyMeshID = ins.ReadS32();

        //
        // Tilemap
        //
        uint layerID     = ins.ReadU32();
        uint layerWidth  = ins.ReadU32();
        uint layerHeight = ins.ReadU32();
        MapTile[,] tiles = new MapTile[layerWidth, layerHeight];

        for (int i = 0; i < layerWidth; ++i)
        {
            for (int j = 0; j < layerHeight; ++j)
            {
                tiles[i, (layerHeight-1) - j] = new MapTile
                {
                    collisionMeshID = ins.ReadS16(),
                    renderMeshID = ins.ReadS16(),
                    elevation = ins.ReadF32(),
                    flags = ins.ReadU32()
                };
            }
        }

        tilemap = new()
        {
            layerID     = layerID,
            layerWidth  = layerWidth,
            layerHeight = layerHeight,
            tiles       = tiles
        };

        //
        // BSP BULLSHIT INCOMING
        //
        uint BSPItem1Count = ins.ReadU32();
        MapBSPItem1[] BSPItem1s = new MapBSPItem1[BSPItem1Count];

        for (int i = 0; i < BSPItem1Count; ++i)
        {
            BSPItem1s[i] = new MapBSPItem1
            {
                u32x00 = ins.ReadU32(),
                xy1    = ins.ReadVector2(),
                xy2    = ins.ReadVector2(),
                itemCount = ins.ReadU32()
            };

            BSPItem1s[i].items = new MapBSPItem2[BSPItem1s[i].itemCount];

            for (int j = 0; j < BSPItem1s[i].itemCount; ++j)
            {
                BSPItem1s[i].items[j] = new MapBSPItem2
                {
                    u32x00 = ins.ReadU32(),
                    u32x04 = ins.ReadU32()
                };
            }
        }

        uint BSPItem3Count = ins.ReadU32();
        MapBSPItem3[] BSPItem3s = new MapBSPItem3[BSPItem3Count];

        for (int i = 0; i < BSPItem3Count; ++i)
        {
            BSPItem3s[i] = new MapBSPItem3
            {
                u32x00 = ins.ReadU32(),
                u32x04 = ins.ReadU32(),
                u32x08 = ins.ReadU32(),
                u32x0C = ins.ReadU32(),
                xy1 = ins.ReadVector2(),
                xy2 = ins.ReadVector2(),
                u32x20 = ins.ReadU32(),
                u32x24 = ins.ReadU32(),
                u32x28 = ins.ReadU32()
            };
        }

        uint BSPItem4Count = ins.ReadU32();
        MapBSPItem4[] BSPItem4s = new MapBSPItem4[BSPItem4Count];

        for (int i = 0; i < BSPItem4Count; ++i)
        {
            BSPItem4s[i] = new MapBSPItem4
            {
                xy1 = ins.ReadVector2(),
                xy2 = ins.ReadVector2(),
                u32x10 = ins.ReadU32(),
                u32x14 = ins.ReadU32()
            };
        }
        
        // Skip strange unknown BSP junk
        ins.ReadU8Array((int)((BSPItem1Count + 7) / 8 * BSPItem1Count));

        //
        // Textures (fucking tricky cunts)
        //
        uint textureCount = ins.ReadU32();

        string[] textureNames = new string[textureCount];

        for (int i = 0; i < textureCount; ++i)
            textureNames[i] = ins.ReadTerminatedString();

        tileTextures = textureNames;

        //
        // Finally, MESHES!
        //
    }

    void ImportEVTData()
    {
        if (!File.Exists(SourceEVTFile))
            throw new Exception("Missing map EVT file!");
    }
}

[Serializable]
public struct MapDirectionalLight
{
    public Color colour;
    public Vector3 direction;    
}

[Serializable]
public struct MapPlayerStart
{
    public Vector3 position;
    public float zDirection;
}

public struct MapObjectInstance
{
    public short classID;
    public byte u8x02;
    public bool isAnimated;
    public bool isVisible;
    public byte u8x05;
    public byte u8x06;
    public byte u8x07;
    public Vector3 position;
    public Vector3 rotation;
    public float uniformScale;
    public byte[] flags;    // 32
}

public struct MapEnemyInstance
{
    public byte u8x00;
    public byte u8x01;
    public byte u8x02;
    public byte u8x03;
    public Vector3 position;
    public Vector3 rotation;
    public float uniformScale;
    public short classID;

    public byte[] flags;    // 18
}

public struct MapCharacterInstance
{
    public byte u8x00;
    public byte u8x01;
    public byte u8x02;
    public byte u8x03;
    public Vector3 position;
    public Vector3 rotation;
    public float uniformScale;
    public short classID;

    public byte[] flags;    // 18
}

public struct MapItemInstance
{
    public byte[] unkx00;   // 4    (was 28)
    public Vector3 position;    // 12
    public Vector3 rotation;    // 12
    public short classID;
    public byte[] flags;    // 10
}

public struct MapTile
{
    public short collisionMeshID;
    public short renderMeshID;
    public float elevation;
    public uint flags;
}

public struct MapTileLayer
{
    public uint layerID;
    public uint layerWidth;
    public uint layerHeight;
    public MapTile[,] tiles;
}

// BSP
public struct MapBSPItem1
{
    public uint u32x00;
    public Vector2 xy1;
    public Vector2 xy2;
    public uint itemCount;
    public MapBSPItem2[] items;
}

public struct MapBSPItem2
{
    public uint u32x00;
    public uint u32x04;
}

public struct MapBSPItem3
{
    public uint u32x00;
    public uint u32x04;
    public uint u32x08;
    public uint u32x0C;
    public Vector2 xy1;
    public Vector2 xy2;
    public uint u32x20;
    public uint u32x24;
    public uint u32x28;
}

public struct MapBSPItem4
{
    public Vector2 xy1;
    public Vector2 xy2;
    public uint u32x10;
    public uint u32x14;
}

public struct MapVertex
{
    public Vector3 position;
    public Vector2 texcoord;
}