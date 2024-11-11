using System;

public abstract class BaseState<T> where T : Enum
{
    /// <summary>Retrieve the parent state machine.</summary>
    public StateMachine<T> StateMachine { get; private set; }

    /// <summary>Retrieve the state name</summary>
    public virtual string Name => $"{StateKey}";

    /// <summary>This states identifying key inside the state machine</summary>
    public T StateKey { get; private set; }

    /// <summary>
    /// Executed when the state is registed with a state machine.
    /// </summary>
    /// <param name="stateMachine">The statemachine the state was registered with.</param>
    public void OnRegister(StateMachine<T> stateMachine, T stateKey)
    {
        StateMachine = stateMachine;
        StateKey     = stateKey;
    }  

    /// <summary>Executed when the state is entered.</summary>
    public virtual void OnEnter()
    {
        #if UNITY_EDITOR
        if(StateMachine.VerboseLogging)
            Logger.Info($"{Name}::OnEnter()");
        #endif
    }

    /// <summary>Executed when the state is exited.</summary>
    public virtual void OnExit()
    {
        #if UNITY_EDITOR
        if (StateMachine.VerboseLogging)
            Logger.Info($"{Name}::OnExit()");
        #endif
    }

    /// <summary>Executed when the state is ticked.</summary>
    public virtual void OnTick()
    {
        #if UNITY_EDITOR
        if (StateMachine.VerboseLogging)
            Logger.Info($"{Name}::OnTick()");
        #endif
    }
}
