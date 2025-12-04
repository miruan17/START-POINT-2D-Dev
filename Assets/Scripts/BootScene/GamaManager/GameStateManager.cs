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
        if (currentState.GetName().Equals("VillageState")) { BGMManager.Instance.PlaySFX(parent.VillageBGM); }
        else if (currentState.GetName().Equals("StageState")) { BGMManager.Instance.PlaySFX(parent.StageBGM); }
        GameManager.Instance.clearEnemies();
        if (currentState != null)
        {
            currentState.Enter();
        }
    }

    public IGameState GetCurrentState() => currentState;
}
