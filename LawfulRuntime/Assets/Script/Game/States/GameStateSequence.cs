using UnityEngine.SceneManagement;
using UnityEngine;

public class GameStateSequence : BaseState<GameState>
{
    GameSequenceInfo sequence;
    GameState nextState;

    public GameStateSequence(GameSequenceInfo sequence, GameState nextState) : base()
    {
        // Our Sequence state is generalized.
        this.sequence  = sequence;
        this.nextState = nextState;
    }

    /// <summary>
    /// BaseState override.<br/>
    /// </summary>
    public override void OnEnter()
    {
        // First check if a sequence is avaliable, if not - we go right into the title...
        if (sequence.mode == SequenceMode.None)
            StateMachine.Switch(nextState);
        else
        {
            // Start loading the sequence player scene...
            AsyncOperation sceneLoadOperation = SceneManager.LoadSceneAsync("SCNSequencePlayer", LoadSceneMode.Additive);
            sceneLoadOperation.completed += OnSceneLoadComplete;
        }

        base.OnEnter();
    }

    /// <summary>
    /// Event Callback.<br/>
    /// </summary>
    void OnSceneLoadComplete(AsyncOperation sceneLoadOperation)
    {
        // Self remove from the event handler
        sceneLoadOperation.completed -= OnSceneLoadComplete;

        // Start playing our sequence
        SequenceController.Instance.SequenceComplete += OnSequenceComplete;
        SequenceController.Instance.PlaySequence(sequence);
    }

    /// <summary>
    /// Event Callback.<br/>
    /// </summary>
    void OnSceneUnloadComplete(AsyncOperation sceneUnloadOperation)
    {
        // Self remove from the event handler
        sceneUnloadOperation.completed -= OnSceneUnloadComplete;

        // Progress to the next state
        StateMachine.Switch(nextState);
    }

    /// <summary>
    /// Event Callback.<br/>
    /// When the sequence has completed clean up the sequence player
    /// </summary>
    void OnSequenceComplete()
    {
        SequenceController.Instance.SequenceComplete -= OnSequenceComplete;

        AsyncOperation sceneUnloadOperation = SceneManager.UnloadSceneAsync("SCNSequencePlayer", UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        sceneUnloadOperation.completed += OnSceneUnloadComplete;
    }
}
