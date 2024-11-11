using Mochineko.StbImageSharpForUnity;
using StbImageSharp;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class TextureFactory
{
    // Class is used to store a single texture
    public class TextureReference
    {
        public Texture2D unityTexture;
        public uint referenceCount;
        public string textureName;
        public bool isReady;
    }

    // We store all loaded textures here...
    static ConcurrentDictionary<string, TextureReference> TextureList = new();

    public static TextureReference LoadTextureFromFile(string filepath)
    {
        // If the file does not exist, throw an exception.
        if (!File.Exists(filepath))
            throw new Exception($"File does not exist '{filepath}'!");

        // If we've already loaded this asset
        if (TextureList.ContainsKey(filepath))
        {
            TextureList[filepath].referenceCount++;
            return TextureList[filepath];
        }
            
        // Create our texture reference class...
        TextureReference newTexture = new TextureReference
        {
            unityTexture   = Texture2D.blackTexture,
            referenceCount = 1,
            textureName    = filepath
        };

        // Place it within the cocurrent dictionary
        TextureList[filepath] = newTexture;

        // Load the data on a task, when it is complete we can create
        Task<ImageResult> decodeTask = new(() =>
        {
            // Load the file
            byte[] bytes = File.ReadAllBytes(filepath);
            return ImageDecoder.DecodeImage(bytes, ColorComponents.RedGreenBlueAlpha);
        });
        decodeTask.ContinueWith((PreviousTask) =>
        {
            newTexture.unityTexture = PreviousTask.Result.ToTexture2D();
            newTexture.isReady      = true;
        }, TaskScheduler.FromCurrentSynchronizationContext());

        decodeTask.Start();

        return newTexture;
    }

    public static void FreeTexture(TextureReference textureReference)
    {
        textureReference.referenceCount--;

        if (textureReference.referenceCount == 0)
        {
            // I'm not sure what's going on here, it seems unity has no way to kill a texture - so we'll just null it?
            textureReference.unityTexture = null;

            TextureList.TryRemove(textureReference.textureName, out _);
            textureReference.textureName = null;
        }
    }
}
