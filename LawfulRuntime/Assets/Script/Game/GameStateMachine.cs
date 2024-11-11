using UnityEngine;

public enum GameState : uint
{
    // Primary
    Initialize,

    // Menus
    TitleScreen,

    // Sequences
    AttractSequence,
    OpeningSequence
}

public class GameStateMachine : StateMachine<GameState>
{
    public GameStateMachine() : base(false)
    {
        // Register our states
        Register(GameState.Initialize,      new GameStateInitialize());
        Register(GameState.AttractSequence, new GameStateSequence(GameManager.Instance.GameInfo.attractSequence, GameState.TitleScreen));
        Register(GameState.TitleScreen,     new GameStateTitle());
        Register(GameState.OpeningSequence, new GameStateSequence(GameManager.Instance.GameInfo.openingSequence, GameState.TitleScreen));

        // Calling switch now sets the initial state
        Switch(GameState.Initialize);
    }
}