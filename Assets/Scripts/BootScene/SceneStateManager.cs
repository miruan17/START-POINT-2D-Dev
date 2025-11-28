public class GameStateManager
{
    private IGameState currentState;

    public void ChangeState(IGameState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;

        if (currentState != null)
        {
            currentState.Enter();
        }
    }

    public IGameState GetCurrentState() => currentState;
}
