using System.IO;
using System.Collections.Generic;

using UnityEngine;
using TMPro;

using Lawful.Resource;

public class AudioTestController : MonoBehaviour
{
    [Header("References (External)")]
    [SerializeField] TextMeshProUGUI soundCountField;
    [SerializeField] TextMeshProUGUI currentSoundField;
    [SerializeField] AudioSource soundSource;

    static string gameFileRoot  = ResourceManager.GamePath;
    static string soundFileRoot = Path.Combine(gameFileRoot, "DATA", "SOUND", "SE");
    static string musicFileRoot = Path.Combine(gameFileRoot, "DATA", "SOUND", "BGM");

    string[] soundFiles;
    string[] musicFiles;
    List<string> combinedSoundFiles;
   
    AudioResource currentResource;
    int currentSoundIndex;
    ulong currentSoundName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Logger.Info("Scanning for sound files...");
        soundFiles = Directory.GetFiles(soundFileRoot, "*.snd");

        Logger.Info("Scanning for music files...");
        musicFiles = Directory.GetFiles(musicFileRoot, "*.wav");

        soundCountField.text = $"Number of sounds: [se = {soundFiles.Length}, bgm = {musicFiles.Length}]";

        // Combine our arrays into a list
        combinedSoundFiles = new List<string>(soundFiles.Length + musicFiles.Length);
        combinedSoundFiles.AddRange(musicFiles);
        combinedSoundFiles.AddRange(soundFiles);

        // Set our current sound to the first...
        currentSoundIndex = 0;
        currentSoundField.text = Path.GetFileName(combinedSoundFiles[currentSoundIndex]);
    }

    public void OnPreviousSound()
    {
        currentSoundIndex--;
        if (currentSoundIndex < 0)
            currentSoundIndex = combinedSoundFiles.Count - 1;

        currentSoundField.text = Path.GetFileName(combinedSoundFiles[currentSoundIndex]);
    }

    public void OnNextSound()
    {
        currentSoundIndex = (++currentSoundIndex) % combinedSoundFiles.Count;

        currentSoundField.text = Path.GetFileName(combinedSoundFiles[currentSoundIndex]);
    }

    public void OnPlaySound()
    {
        // Load the sound using our relative path
        string soundPath = Path.GetRelativePath(gameFileRoot, combinedSoundFiles[currentSoundIndex]);

        // Sync test
        //currentSoundName = ResourceManager.Load<AudioResource>(soundPath);

        // We can now grab it and place it on our source...
        //soundSource.clip = ResourceManager.Get<AudioResource>(currentSoundName).Get();
        //soundSource.Play();

        // ASync test
        ResourceManager.LoadAsync<AudioResource>(soundPath, OnSoundLoaded);
    }

    public void OnSoundLoaded(ulong resourceName)
    {
        currentSoundName = resourceName;
        currentResource  = ResourceManager.Get<AudioResource>(currentSoundName);
        soundSource.clip = currentResource.Get();
        soundSource.Play();
    }
}
