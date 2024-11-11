using UnityEngine.SceneManagement;
using UnityEngine;

public class GameStateTitle : BaseState<GameState>
{
    GameState nextState;

    public override void OnEnter()
    {
        // Load the title scene
        AsyncOperation sceneLoadOperation = SceneManager.LoadSceneAsync("SCNTitle", LoadSceneMode.Additive);
        sceneLoadOperation.completed += OnSceneLoadComplete;

        base.OnEnter();
    }

    /// <summary>
    /// Event Callback.<br/>
    /// </summary>
    void OnSceneLoadComplete(AsyncOperation sceneLoadOperation)
    {
        // Self remove from the event handler
        sceneLoadOperation.completed -= OnSceneLoadComplete;

        // Bind to our events...
        TitleScreenController.Instance.TitleTimeout += OnTitleTimeout;
        TitleScreenController.Instance.TitleNewGame += OnTitleNewGame;
    }

    void OnTitleNewGame()
    {
        // Self unbind
        TitleScreenController.Instance.TitleTimeout -= OnTitleTimeout;
        TitleScreenController.Instance.TitleNewGame -= OnTitleNewGame;

        // Configure the next state
        nextState = GameState.OpeningSequence;

        // Unload the title scene
        AsyncOperation sceneUnloadOperation = SceneManager.UnloadSceneAsync("SCNTitle", UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        sceneUnloadOperation.completed += OnSceneUnloadComplete;
    }

    void OnTitleTimeout()
    {
        // Self unbind
        TitleScreenController.Instance.TitleTimeout -= OnTitleTimeout;
        TitleScreenController.Instance.TitleNewGame -= OnTitleNewGame;

        // Configure the next state
        nextState = GameState.AttractSequence;

        // Unload the title scene
        AsyncOperation sceneUnloadOperation = SceneManager.UnloadSceneAsync("SCNTitle", UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        sceneUnloadOperation.completed += OnSceneUnloadComplete;
    }

    /// <summary>
    /// Event Callback.<br/>
    /// </summary>
    void OnSceneUnloadComplete(AsyncOperation sceneUnloadOperation)
    {
        // Self remove from the event handler
        sceneUnloadOperation.completed -= OnSceneUnloadComplete;

        StateMachine.Switch(nextState);
    }
}
