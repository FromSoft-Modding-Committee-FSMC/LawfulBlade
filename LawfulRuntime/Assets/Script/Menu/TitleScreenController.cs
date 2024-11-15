using DG.Tweening;
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreenController : MonoBehaviour
{
    // Inspector
    [Header("References (External)")]
    [SerializeField] GameInformation gameInformation;

    [Header("References (Internal)")]
    [SerializeField] RawImage titleImage;
    [SerializeField] GameObject screenPressAny;
    [SerializeField] GameObject screenOptions;

    [Header("Configuration")]
    [SerializeField] float timeoutTime = 20f;

    // Events
    public event Action TitleTimeout;
    public event Action TitleNewGame;

    // Properties
    public static TitleScreenController Instance { get; private set; }

    // Data
    TextureFactory.TextureReference titleTexture = null;
    float timeoutTimer = 0f;
    bool  timeoutActivatedOrDisabled = false;

    /// <summary>
    /// MonoBehaviour Implementation.<br/>
    /// Initializes Singleton.
    /// </summary>
    void Awake()
    {
        // Construct our instance
        if (Instance != null)
            throw new Exception("Cannot have more than one instance of 'TitleScreenController'!");

        Instance = this;

        // Load the title screen image
        if (ResourceManager.OverrideExists(Path.GetFileNameWithoutExtension(gameInformation.titleImage)))
        {
            titleImage.texture = ResourceManager.OverrideLoad<Texture2D>(Path.GetFileNameWithoutExtension(gameInformation.titleImage));
            titleImage.enabled = true;

            // Fade in the title image
            titleImage.DOFade(1f, 1f)
                .OnComplete(() =>
                {
                // Activate our "Press Any Key" text
                screenPressAny.SetActive(true);
                });

            return;
        }

        try
        {
            titleTexture = TextureFactory.LoadTextureFromFile(Path.Combine(ResourceManager.GameDataPath, "PICTURE", gameInformation.titleImage));
        } 
        catch (Exception ex)
        {
            Logger.Error($"Failed to load titlescreen image: {ex.Message}");
        }

        // Apply our title screen image
        if (titleTexture != null)
            StartCoroutine(LoadTitleScreenCO());
    }

    /// <summary>
    /// MonoBehaviour Implementation.<br/>
    /// Destroys Singleton.
    /// </summary>
    void OnDestroy()
    {
        Instance = null;
    }
    
    /// <summary>
    /// MonoBehaviour Implementation.<br/>
    /// </summary>
    void FixedUpdate()
    {
        if (timeoutActivatedOrDisabled)
            return;

        // If any key is pressed, reset the time out timer...
        if (Input.anyKey)
            timeoutTimer = 0f;

        timeoutTimer += Time.fixedDeltaTime;
        if (timeoutTimer >= timeoutTime)
        {
            titleImage.DOFade(0f, 1f)
                .OnStart(() =>
                {
                    // Disable our texts
                    screenPressAny.SetActive(false);
                    screenOptions.SetActive(false);
                })
                .OnComplete(() =>
                {
                    // Invoke the title timeout event after the fade is completed...
                    TitleTimeout?.Invoke();
                });

            timeoutActivatedOrDisabled = true;
        }
    }

    IEnumerator LoadTitleScreenCO()
    {
        while (!titleTexture.isReady)
            yield return new WaitForEndOfFrame();

        // Set our texture to the one we just loaded
        titleImage.texture = titleTexture.unityTexture;
        titleImage.enabled = true;

        // Fade in the title image
        titleImage.DOFade(1f, 1f)
            .OnComplete(() =>
            {
                // Activate our "Press Any Key" text
                screenPressAny.SetActive(true);
            });
    }

    /// <summary>
    /// Inspector Binding Callback.<br/>
    /// </summary>
    public void OnPressAnyKey()
    {
        // Timeout is disabled from here on out
        timeoutActivatedOrDisabled = true;

        // Disable our first text
        screenPressAny.SetActive(false);
        screenOptions.SetActive(true);
    }

    public void OnPressNewGame()
    {
        screenOptions.SetActive(false);

        titleImage.DOFade(0f, 1f)
        .OnComplete(() =>
        {
            // Invoke the title timeout event after the fade is completed...
            TitleNewGame?.Invoke();
        });
    }
}
