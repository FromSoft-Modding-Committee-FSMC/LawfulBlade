using System;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using DG.Tweening;
using TMPro;
using System.Collections;
using Lawful.Resource;
using System.Text;

public class SequenceController : MonoBehaviour
{
    [Header("References (External)")]
    [SerializeField] GameInformation gameInformation;

    [Header("References (Internal)")]
    [SerializeField] AudioSource sequenceAudio;
    [SerializeField] VideoPlayer sequenceVideo;
    [SerializeField] RawImage sequenceImage;
    [SerializeField] TextMeshProUGUI sequenceText;
    [SerializeField] Material sequenceFontMaterial;

    [Header("Debugging")]
    [SerializeField] SequenceData sequenceData;

    [SerializeField] ulong audioResourceName;
    [SerializeField] ulong imageResourceName;
    [SerializeField] ulong fontResourceName;

    // Events
    public event Action SequenceComplete;

    // Singleton
    public static SequenceController Instance { get; private set; }

    /// <summary>
    /// MonoBehaviour Implementation<br/>
    /// Initializes Singleton.
    /// </summary>
    void Awake()
    {
        // Singleton Capturing
        if (Instance != null)
            throw new Exception("Cannot have more than one instance of SequenceContolller");

        Logger.Info("Sequence Playback Initialized...");

        Instance = this;

        // Load our sequence font
        fontResourceName = ResourceManager.Load<FontResource>(Path.Combine("FONT", "SequenceText.otf"));
    }

    /// <summary>
    /// MonoBehaviour Implementation<br/>
    /// Destroys Singleton.
    /// </summary>
    void OnDestroy()
    {
        Instance = null;
    }

    /// <summary>
    /// Play a sequence in the sequence controller
    /// </summary>
    public void PlaySequence(GameSequenceInfo gameSequenceInfo)
    {
        if (gameSequenceInfo.mode == SequenceMode.None)
            return;

        switch (gameSequenceInfo.mode)
        {
            case SequenceMode.Video:

                Logger.Info("Sequence Type: Video");

                // Setup for video playback
                sequenceAudio.enabled = true;
                sequenceVideo.enabled = true;

                // Wait for video to end
                sequenceVideo.loopPointReached += OnVideoComplete;

                // Set URL and start streaming the video file
                sequenceVideo.url = gameSequenceInfo.file;
                sequenceVideo.Play();
                break;

            case SequenceMode.Slideshow:

                Logger.Info("Sequence Type: Slideshow");

                // Setup for slideshow playback
                sequenceAudio.enabled = true;
                sequenceImage.enabled = true;
                sequenceText.enabled  = true;

                // Configure text font
                sequenceText.font = ResourceManager.Get<FontResource>(fontResourceName).Get();

                // Enable Underlay for the sequence text font
                sequenceText.fontMaterial.EnableKeyword(ShaderUtilities.Keyword_Underlay);
                sequenceText.fontMaterial.SetColor("_UnderlayColor", Color.black);
                sequenceText.fontMaterial.SetFloat("_UnderlayOffsetX",  1f);
                sequenceText.fontMaterial.SetFloat("_UnderlayOffsetY", -1f);

                LoadSlideshowData(gameSequenceInfo.file);
                StartCoroutine(SlideShowPlaybackCO());

                break;
        }
    }

    /// <summary>s
    /// Loads sequence slideshow data from a file
    /// </summary>
    /// <param name="file">The file to load fron...</param>
    void LoadSlideshowData(string file)
    {
        sequenceData = new SequenceData();

        // Start reading our sequence data
        using StreamReader sr = new(file);

        // First comes the number of slides.
        sequenceData.numberOfSlides = int.Parse(sr.ReadLine());

        // Now we must read our slide data....
        SequenceSlide[] slides = new SequenceSlide[sequenceData.numberOfSlides];

        // First comes a list of sequence images and display data information
        for (int i = 0; i < sequenceData.numberOfSlides; ++i)
        {
            slides[i] = new SequenceSlide
            {
                imageFile = Path.Combine("DATA", "PICTURE", sr.ReadLine()),
                displayTime = int.Parse(sr.ReadLine()) / 1000f
            };
        }

        // The audio file is stored afterwards.
        sequenceData.audioFile = Path.Combine("DATA", "SOUND", "BGM", sr.ReadLine());

        // Now comes the text list
        StringBuilder sb = new ();

        for (int i = 0; i < sequenceData.numberOfSlides; ++i)
        {
            // First there is an empty line for padding
            sr.ReadLine();

            // Now read the lines for text
            for (int j = 0; j < 16; ++j)
                sb.AppendLine(sr.ReadLine());

            // Load text from the string builder, reset the string builder
            slides[i].text = sb.ToString();
            sb.Clear();
        }

        // Store our loaded slides on here
        sequenceData.slides = slides;
    }

    IEnumerator SlideShowPlaybackCO()
    {
        // First, attempt to load the audio file
        if (!sequenceData.audioFile.EndsWith("NO_BGM"))
        {
            // Actual loading
            audioResourceName = ResourceManager.Load<AudioResource>(sequenceData.audioFile);
            AudioResource audioResource = ResourceManager.Get<AudioResource>(audioResourceName);

            // Apply and play our audio
            sequenceAudio.clip   = audioResource.Get();
            sequenceAudio.volume = 0f;
            sequenceAudio.Play();

            // We must fade the audio in
            sequenceAudio.DOFade(1f, 1f);
        }

        // Now play through each slide
        for (int i = 0; i < sequenceData.numberOfSlides; ++i)
        {
            // Grab the slide
            SequenceSlide slide = sequenceData.slides[i];

            // If the image is valid, we will try to load and display it
            if (!slide.imageFile.EndsWith("NO_BMP"))
            {
                imageResourceName = ResourceManager.Load<TextureResource>(slide.imageFile);
                TextureResource textureResource = ResourceManager.Get<TextureResource>(imageResourceName);

                // Apply the image
                sequenceImage.texture = textureResource.Get();
            }

            // Set the text for the slide
            sequenceText.text = slide.text;

            // Fade in the current slide
            Sequence fadeInSequence = DOTween.Sequence();
            fadeInSequence.Join(sequenceText.DOFade(1f, 1f));
            fadeInSequence.Join(sequenceImage.DOFade(1f, 1f));
            yield return fadeInSequence.WaitForCompletion();

            // Wait for the display time
            yield return new WaitForSeconds(slide.displayTime);

            // Fade out the current slide
            Sequence fadeOutSequence = DOTween.Sequence();
            fadeOutSequence.Join(sequenceText.DOFade(0f, 1f));
            fadeOutSequence.Join(sequenceImage.DOFade(0f, 1f));
            yield return fadeOutSequence.WaitForCompletion();

            // Wait two seconds before playing the next slide
            yield return new WaitForSeconds(2f);

            // Here is where we should be freeing our texture
            Logger.Warn("TEXTURE LOST! MUST IMPLEMENT FREEING LOGIC!!!!");
        }


        // If we have sequence audio, we should fade it out now - and then free the asset
        if (!sequenceData.audioFile.EndsWith("NO_BGM"))
        {
            sequenceAudio.DOFade(0f, 1f)
                .OnComplete(() =>
                {
                    OnSlideshowComplete();

                    sequenceAudio.Stop();
                    sequenceAudio.clip = null;

                    Logger.Warn("AUDIO LOST! MUST IMPLEMENT FREEING LOGIC!!!!");
                });
        }
    }

    /// <summary>
    /// Event Callback.<br/>
    /// Called when the video has completed playback, triggering Sequence Complete!
    /// </summary>
    void OnVideoComplete(VideoPlayer videoPlayer)
    {
        sequenceVideo.loopPointReached -= OnVideoComplete;
        sequenceVideo.enabled = false;
        sequenceAudio.enabled = false;

        SequenceComplete?.Invoke();
    }

    /// <summary>
    /// Event Callback.<br/>
    /// Called when a slideshow has completed playback.
    /// </summary>
    void OnSlideshowComplete()
    {
        sequenceAudio.enabled = false;
        sequenceImage.enabled = false;
        sequenceText.enabled = false;

        SequenceComplete?.Invoke();
    }
}

[Serializable]
public struct SequenceData
{
    public int numberOfSlides;
    public string audioFile;
    public SequenceSlide[] slides;
}

[Serializable]
public struct SequenceSlide
{
    public float  displayTime;
    public string imageFile;
    public string text;
}
