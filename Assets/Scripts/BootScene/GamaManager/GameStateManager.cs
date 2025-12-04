public class GameStateManager
{
    private IGameState currentState;
    public GameManager parent;

    public void ChangeState(IGameState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }
        currentState = newState;
        GameManager.Instance.clearEnemies();
        if (currentState != null)
        {
            currentState.Enter();
        }
    }

    public IGameState GetCurrentState() => currentState;
}
