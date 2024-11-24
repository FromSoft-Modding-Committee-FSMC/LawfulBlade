using System.IO;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Lawful.Resource;

public class TextureTestController : MonoBehaviour
{
    [Header("References (External)")]
    [SerializeField] TextMeshProUGUI textureCountField;
    [SerializeField] TextMeshProUGUI textureCurrentField;
    [SerializeField] RawImage textureDisplayImage;
    [SerializeField] AspectRatioFitter textureDisplayFitter;

    static string gameFileRoot = ResourceManager.GamePath;
    static string pictureFileRoot = Path.Combine(gameFileRoot, "DATA", "PICTURE");
    static string sfxFileRoot = Path.Combine(gameFileRoot, "DATA", "SFX", "MODEL");
    static string objFileRoot = Path.Combine(gameFileRoot, "DATA", "OBJ", "MODEL");
    static string menuFileRoot = Path.Combine(gameFileRoot, "DATA", "MENU");

    string[] pictureFiles, sfxTextureFiles, objTextureFiles, menuTextureFiles;

    List<string> combinedTextureFiles;

    int currentTextureIndex;
    ulong currentTextureName;
    TextureResource currentResource;

    void Awake()
    {
        Logger.Info("Scanning for pictures...");
        pictureFiles    = Directory.GetFiles(pictureFileRoot, "*.bmp");
        sfxTextureFiles = Directory.GetFiles(sfxFileRoot, "*.txr");
        objTextureFiles = Directory.GetFiles(objFileRoot, "*.txr");
        menuTextureFiles = Directory.GetFiles(menuFileRoot, "*.bmp");

        textureCountField.text = $"Number of Textures [picture = {pictureFiles.Length}, menu = {menuTextureFiles.Length}]";

        combinedTextureFiles = new List<string>();
        combinedTextureFiles.AddRange(pictureFiles);
        combinedTextureFiles.AddRange(menuTextureFiles);

        currentTextureIndex = 0;
        textureCurrentField.text = Path.GetFileName(combinedTextureFiles[currentTextureIndex]);
    }

    public void OnPreviousTexture()
    {
        currentTextureIndex--;
        if (currentTextureIndex < 0)
            currentTextureIndex = combinedTextureFiles.Count - 1;

        textureCurrentField.text = Path.GetFileName(combinedTextureFiles[currentTextureIndex]);
    }

    public void OnNextTexture()
    {
        currentTextureIndex = (++currentTextureIndex) % combinedTextureFiles.Count;
        textureCurrentField.text = Path.GetFileName(combinedTextureFiles[currentTextureIndex]);
    }

    public void OnLoadTexture()
    {
        ResourceManager.LoadAsync<TextureResource>(Path.GetRelativePath(gameFileRoot, combinedTextureFiles[currentTextureIndex]), OnTextureLoaded);
    }
        

    void OnTextureLoaded(ulong name)
    {
        currentTextureName = name;
        currentResource = ResourceManager.Get<TextureResource>(currentTextureName);

        textureDisplayFitter.aspectRatio = currentResource.width / (float)currentResource.height;
        textureDisplayImage.texture = currentResource.Get();
    }
}
