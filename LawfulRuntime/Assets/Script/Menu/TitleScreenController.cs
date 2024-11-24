using DG.Tweening;
using Lawful.Resource;
using System;
using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreenController : MonoBehaviour
{
    // Inspector
    [Header("References (External)")]
    [SerializeField] GameInformation gameInformation;

    [Header("References (Internal)")]
    [SerializeField] RawImage titleImage;

    // Main Screen
    [SerializeField] GameObject screenPressAny;
    [SerializeField] TextMeshProUGUI pressAnyKeyField;

    // Main Screen, Options Lists
    [SerializeField] GameObject screenOptions;

    [Header("Configuration")]
    [SerializeField] float timeoutTime = 20f;

    // Events
    public event Action TitleTimeout;
    public event Action TitleNewGame;

    // Properties
    public static TitleScreenController Instance { get; private set; }

    // Data
    ulong fontResourceName;

    float timeoutTimer = 0f;
    bool  timeoutActivatedOrDisabled = true;

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

        fontResourceName = ResourceManager.Load<FontResource>(Path.Combine("FONT", "TitleScreen.otf"));
        
        ResourceManager.LoadAsync<TextureResource>(Path.Combine("DATA", "PICTURE", gameInformation.titleImage), OnTitleLoadComplete);
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
    /// Event Callback.<br/>
    /// Called when the title image finishes loading
    /// </summary>
    void OnTitleLoadComplete(ulong textureName)
    {
        titleImage.texture = ResourceManager.Get<TextureResource>(textureName).Get();

        pressAnyKeyField.font = ResourceManager.Get<FontResource>(fontResourceName).Get();

        titleImage.DOFade(1f, 1f)
        .OnComplete(() =>
        {
            screenPressAny.SetActive(true);

            timeoutActivatedOrDisabled = false;
        });
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
