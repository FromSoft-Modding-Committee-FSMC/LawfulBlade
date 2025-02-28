using Lawful.Resource;
using System;
using System.IO;

using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Inspector
    [Header("References (External)")]
    [SerializeField] GameInformation gameInformation;
    [SerializeField] MapData gameMapData;

    // Properties
    public GameStateMachine StateMachine   { get; private set; } = null;

    // Accessor style properties
    public GameInformation GameInfo => gameInformation;

    // Singleton Implementation
    public static GameManager Instance { get; private set; }

    /// <summary>
    /// MonoBehaviour Implementation.<br/>
    /// The root of all evil.
    /// </summary>
    void Awake()
    {
        // Singleton Setup
        if (Instance != null)
            throw new Exception("Only one instance of 'GameManager' may exist at a time!");

        Instance = this;

        // Constant data loading...
        Logger.Info("Loading Game Information...");
        gameInformation.ImportFromLegacyFile(Path.Combine(ResourceManager.GamePath, "PARAM", "SYS.DAT"));

        // Logger.Info("Loading Map Data...");
        // gameMapData.LoadMapFiles();

        // Apply configuration options
        SetRuntimeConfiguration();

        // Object initialization
        StateMachine    = new GameStateMachine();
    }

    /// <summary>
    /// MonoBehaviour Implementation.<br/>
    /// Responsible for ticking our state machine.
    /// </summary>
    void FixedUpdate()
    {
        // Tick our state machine
        StateMachine.Tick();
    }

    /// <summary>
    /// Applies default expected configuration to the Unity runtime.
    /// </summary>
    void SetRuntimeConfiguration()
    {
        // Time Settings
        Time.fixedDeltaTime = 1f / 64f;
    }
}
