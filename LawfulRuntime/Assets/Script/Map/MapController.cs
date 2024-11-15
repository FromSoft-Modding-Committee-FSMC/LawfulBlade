using UnityEngine;
using Unity.Entities;
using Unity.Scenes;

public class MapController : MonoBehaviour
{
    [Header("References (External)")]
    [SerializeField] GameInformation gameInformationTemp;
    [SerializeField] MapData mapData;

    [Header("Object Configuration")]
    [SerializeField] Transform objectRoot;
    [SerializeField] GameObject objectInstanceBase;

    [Header("Enemy Configuration")]
    [SerializeField] Transform enemyRoot;
    [SerializeField] GameObject enemyInstanceBase;

    [Header("Character Configuration")]
    [SerializeField] Transform characterRoot;
    [SerializeField] GameObject characterInstanceBase;

    [Header("Item Configuration")]
    [SerializeField] Transform itemRoot;
    [SerializeField] GameObject itemInstanceBase;

    [Header("Tilemap Configuration")]
    [SerializeField] Transform tilemapRoot;
    [SerializeField] GameObject tileInstanceBase;

    /// <summary>
    /// MonoBehaviour Implementation<br/>
    /// </summary>
    void Awake()
    {
        // TEMPORARY
        mapData.LoadMapFiles();

        // Grab the map we would like to reference...
        MapFile currentFile = mapData.GameMaps[mapData.GameMapIDToIndex[gameInformationTemp.startingMapID]];   // 16, 

        //
        // Spawn Tilemap
        //
        for (int i = 0; i < currentFile.tilemap.layerWidth; ++i)
        {
            for (int j = 0; j < currentFile.tilemap.layerHeight; ++j)
            {
                MapTile tile = currentFile.tilemap.tiles[i, j];

                if (tile.renderMeshID == -1)
                    continue;

                GameObject newObject = Instantiate(tileInstanceBase, tilemapRoot);
                newObject.name = $"{i:D4}:{j:D4}";
                newObject.transform.position = new Vector3(2 * i, tile.elevation, 2 * j);
            }
        }

        //
        // Spawn Map Content
        /*
        
        // Objects
        for(int i = 0; i < currentFile.ObjectInstances.Count; ++i)
        {
            // Get the object instance
            MapObjectInstance instance = currentFile.ObjectInstances[i];

            // Class IDs < 0 are invalid
            if (instance.classID < 0)
                continue;

            // Construction
            GameObject newObject = Instantiate(objectInstanceBase, objectRoot);
            newObject.name = $"Object {i:D4}";

            // Parameter application
            newObject.transform.position   = instance.position;
            newObject.transform.localScale = new Vector3(instance.uniformScale, instance.uniformScale, instance.uniformScale);
        }

        // Enemies
        for (int i = 0; i < currentFile.EnemyInstances.Count; ++i)
        {
            // Get the object instance
            MapEnemyInstance instance = currentFile.EnemyInstances[i];

            // Class IDs < 0 are invalid
            if (instance.classID < 0)
                continue;

            // Construction
            GameObject newEnemy = Instantiate(enemyInstanceBase, enemyRoot);
            newEnemy.name = $"Enemy {i:D4}";

            // Parameter application
            newEnemy.transform.position = instance.position;
            newEnemy.transform.localScale = new Vector3(instance.uniformScale, instance.uniformScale, instance.uniformScale);
        }

        // Characters
        for (int i = 0; i < currentFile.CharacterInstances.Count; ++i)
        {
            // Get the object instance
            MapCharacterInstance instance = currentFile.CharacterInstances[i];

            // Class IDs < 0 are invalid
            if (instance.classID < 0)
                continue;

            // Construction
            GameObject newCharacter = Instantiate(characterInstanceBase, characterRoot);
            newCharacter.name = $"Character {i:D4}";

            // Parameter application
            newCharacter.transform.position = instance.position;
            newCharacter.transform.localScale = new Vector3(instance.uniformScale, instance.uniformScale, instance.uniformScale);
        }

        // Items
        for (int i = 0; i < currentFile.ItemInstances.Count; ++i)
        {
            MapItemInstance instance = currentFile.ItemInstances[i];

            // Class IDs < 0 are invalid
            if (instance.classID < 0)
                continue;

            GameObject newItem = Instantiate(itemInstanceBase, itemRoot);
            newItem.name = $"Item {i:D4}";

            newItem.transform.position = instance.position;
        }

        */
    }
}
