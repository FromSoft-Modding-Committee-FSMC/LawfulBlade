using System;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using DG.Tweening;
using TMPro;
using System.Collections;

public class SequenceController : MonoBehaviour
{
    [Header("References (External)")]
    [SerializeField] GameInformation gameInformation;

    [Header("References (Internal)")]
    [SerializeField] AudioSource sequenceAudio;
    [SerializeField] VideoPlayer sequenceVideo;
    [SerializeField] RawImage sequenceImage;
    [SerializeField] TextMeshProUGUI sequenceText;

    [Header("Debugging")]
    [SerializeField] SequenceData sequenceData;

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

        Instance = this;
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

                // Setup for slideshow playback
                sequenceAudio.enabled = true;
                sequenceImage.enabled = true;
                sequenceText.enabled  = true;

                LoadSlideshowData(gameSequenceInfo.file);
                StartCoroutine(SlideShowPlaybackCO());

                break;
        }
    }

    void LoadSlideshowData(string file)
    {
        sequenceData = new SequenceData();

        // Gather slides...
        List<SequenceSlide> slides = new();

        using (StreamReader sr = new (file))
        {
            // Number of slides is first...
            sequenceData.numberOfSlides = int.Parse(sr.ReadLine());

            for (int i = 0; i < sequenceData.numberOfSlides; ++i)
            {
                slides.Add(new SequenceSlide
                {
                    imageFile = Path.Combine(Path.GetFullPath(Application.streamingAssetsPath), "Game", "DATA", "PICTURE", sr.ReadLine()),
                    displayTime = int.Parse(sr.ReadLine()) / 1000f
                });
            }

            sequenceData.audioFile = Path.Combine(Path.GetFullPath(Application.streamingAssetsPath), "Game", "DATA", "SOUND", "BGM", sr.ReadLine());

            for (int i = 0; i < sequenceData.numberOfSlides; ++i)
            {
                // Alignment
                sr.ReadLine();

                SequenceSlide copy = slides[i];

                copy.text = sr.ReadLine();
                for (int j = 0; j < 15; ++j)
                    copy.text += ("\n" + sr.ReadLine());

                slides[i] = copy;
            }
        }

        sequenceData.slides = slides.ToArray();
    }

    IEnumerator SlideShowPlaybackCO()
    {
        // Load up the audio
        AudioFactory.AudioReference slideshowAudio = null;

        if (!sequenceData.audioFile.Equals("NO_BGM"))
        {
            string audioPath = Path.Combine(Path.GetFullPath(Application.streamingAssetsPath), "Game", "DATA", "SOUND", "BGM", sequenceData.audioFile);

            // Attempt to load audio
            slideshowAudio = AudioFactory.LoadWavFromFile(audioPath);

            // Wait for audio to be loaded...
            while (!slideshowAudio.isReady)
                yield return new WaitForEndOfFrame();

            // Assign audio clip
            sequenceAudio.clip = slideshowAudio.unityClip;
            sequenceAudio.volume = 0f;
            sequenceAudio.Play();

            sequenceAudio.DOFade(1f, 1f);
        }

        for(int i = 0; i < sequenceData.numberOfSlides; ++i)
        {
            // Load up the image and text for this slide...
            TextureFactory.TextureReference slideshowImage = null;
            if (!sequenceData.slides[i].imageFile.Equals("NO_BMP"))
            {
                slideshowImage = TextureFactory.LoadTextureFromFile(Path.Combine(Path.GetFullPath(Application.streamingAssetsPath), "Game", "DATA", "PICTURE", sequenceData.slides[i].imageFile));

                // Wait for our image to be ready...
                while (!slideshowImage.isReady)
                    yield return new WaitForEndOfFrame();

                // Assign texture from image
                sequenceImage.texture = slideshowImage.unityTexture;
            }
            
            sequenceText.text     = sequenceData.slides[i].text;
            
            // Fade in for one second
            sequenceText.DOFade(1f, 1f);
            sequenceImage.DOFade(1f, 1f);
            yield return new WaitForSeconds(1f);

            // Display for an amount of time
            yield return new WaitForSeconds(sequenceData.slides[i].displayTime);

            // Fade out for one second
            sequenceText.DOFade(0f, 1f);
            sequenceImage.DOFade(0f, 1f);
            yield return new WaitForSeconds(1f);

            // Wait two seconds before displaying the next slide
            yield return new WaitForSeconds(2f);

            // Free our texture!
            sequenceImage.texture = null;
            TextureFactory.FreeTexture(slideshowImage);
        }

        // Load up the audio
        if (!sequenceData.audioFile.Equals("NO_BGM"))
        {
            sequenceAudio.DOFade(0f, 1f)
                .OnComplete(() =>
                {
                    OnSlideshowComplete();
                    
                    // Clear up our sequence audio
                    sequenceAudio.Stop();
                    sequenceAudio.clip = null;

                    // Free the audio
                    AudioFactory.FreeAudio(slideshowAudio);
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
