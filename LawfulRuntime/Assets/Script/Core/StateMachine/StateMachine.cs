using System;
using System.Collections.Generic;

public class StateMachine<T> where T : Enum
{
    // The statemap stores all possible states the player can be in
    readonly Dictionary<T, BaseState<T>> stateMap;

    /// <summary>The previous state the machine was in</summary>
    public BaseState<T> PreviousState { get; private set; } = null;

    /// <summary>The currently active state</summary>
    public BaseState<T> CurrentState  { get; private set; } = null;

    /// <summary>The target next state</summary>
    public BaseState<T> TargetState   { get; private set; } = null;

    /// <summary>Set when verbose (ott) logging is enabled for the state machine</summary>
    public bool VerboseLogging { get; set; } = false;

    /// <summary>
    /// Default Constructor.
    /// </summary>
    public StateMachine(bool verboseLogging)
    {
        // Create state map.
        stateMap = new Dictionary<T, BaseState<T>>();

        VerboseLogging = verboseLogging;
    }

    /// <summary>
    /// Updates the statemachine, changes state if requested...
    /// </summary>
    public void Tick()
    {
        // If the current state is null, we might be setting the first state...
        if (CurrentState == null)
        {
            // We must be setting the initial state.
            CurrentState = TargetState ?? throw new Exception("State Machine has no current or next state!");
            TargetState = null;

            // Run on enter for the initial state.
            CurrentState.OnEnter();
        }

        // Tick the current state, assuming it exits...
        CurrentState.OnTick();

        // Was a new state requested?
        if (TargetState != null)
        {
            // Switch states
            PreviousState = CurrentState;
            CurrentState = null;

            // Run exit for the last state, if it was valid
            PreviousState.OnExit();
        }
    }

    /// <summary>
    /// Registers a state with the state machine
    /// </summary>
    /// <param name="key">The key to register the state as</param>
    /// <param name="state">The state object</param>
    /// <param name="makeCurrent">If the state should be forced as current.</param>
    public void Register(T key, BaseState<T> state)
    {
        if (stateMap.ContainsKey(key))
            throw new Exception($"State Map already contains a definition for {key}!");

        stateMap[key] = state ?? throw new Exception($"Cannot register null state for {key}");

        state.OnRegister(this, key);
    }

    /// <summary>
    /// Prepares to switch to the next state, but will not perform it until the current tick has completed.
    /// </summary>
    /// <param name="to">The key of the state to switch in to</param>
    public void Switch(T key) =>
        TargetState = stateMap[key];

    /// <summary>
    /// Checks to see if a state with the given key exists.
    /// </summary>
    /// <param name="key">The key of the state we're checking for</param>
    /// <returns>True if the state exists, False otherwise</inheritdoc>/></returns>
    public bool Exists(T key) =>
        stateMap.ContainsKey(key);

}
