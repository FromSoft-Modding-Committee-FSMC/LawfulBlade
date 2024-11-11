public class GameStateInitialize : BaseState<GameState>
{
    public override void OnEnter()
    {
        // For now immediately switch state to attract sequence
        StateMachine.Switch(GameState.AttractSequence);

        base.OnEnter();
    }
}
