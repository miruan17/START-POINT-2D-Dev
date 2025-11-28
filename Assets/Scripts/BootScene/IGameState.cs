using UnityEngine.SceneManagement;

public interface IGameState
{
    void Enter();
    void Exit();
    string GetName();
}

#region Title State
public class TitleState : IGameState
{
    private string stateName = "TitleState";
    private string sceneName = "TitleScene";

    public void Enter()
    {
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }

    public void Exit()
    {
        SceneManager.UnloadSceneAsync(sceneName);
    }

    public string GetName() => stateName;
}
#endregion


#region Village State
public class VillageState : IGameState
{
    private string stateName = "VillageState";
    private string sceneName = "VillageScene";

    public void Enter()
    {
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }

    public void Exit()
    {
        SceneManager.UnloadSceneAsync(sceneName);
    }

    public string GetName() => stateName;
}
#endregion


#region State State
public class StageState : IGameState
{
    private string stateName = "StageState";

    private string stageId;
    private string startRoomId;
    private string startRoomSceneName;

    public StageState(string stageId, string startRoomId, string startRoomSceneName)
    {
        this.stageId = stageId;
        this.startRoomId = startRoomId;
        this.startRoomSceneName = startRoomSceneName;
    }

    public void Enter()
    {
        GameManager.Instance.StartStage(stageId, startRoomId, startRoomSceneName);
    }

    public void Exit()
    {
        if (GameManager.Instance.RoomSceneManager != null)
        {
            GameManager.Instance.RoomSceneManager.UnloadCurrentRoom();
        }

        GameManager.Instance.ClearStage();
    }

    public string GetName() => stateName;
}
#endregion