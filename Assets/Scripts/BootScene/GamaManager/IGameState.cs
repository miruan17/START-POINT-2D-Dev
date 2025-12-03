using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Navigate
{
    spawn,
    left,
    right,
    up,
    down
}
public interface IGameState
{
    void Enter();
    void Exit();
    void Move(Navigate navigate);
    string GetName();
}


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

    public void Move(Navigate navigate)
    {
        return;
    }
    public string GetName() => stateName;
}
#endregion


#region Stage State
public class StageState : IGameState
{
    private string stateName = "StageState";

    // Runtime Session
    private StageDataSession sessionData;
    private RoomSession currentSession;

    // 추후에 시간 남으면 랜덤 생성 기능 설정
    public StageState()
    {
        sessionData = new StageDataSession();

        // 시작 방 설정 (0번 방 기준)
        currentSession = sessionData.roomDatas[0];
    }

    public void Enter()
    {
        SceneManager.LoadSceneAsync(currentSession.roomSceneName, LoadSceneMode.Additive);
    }

    public void Exit()
    {
        SceneManager.UnloadSceneAsync(currentSession.roomSceneName);
    }

    public void Move(Navigate nav)
    {
        int targetId = currentSession.roomId;

        switch (nav)
        {
            case Navigate.left:
                targetId -= 1;
                break;

            case Navigate.right:
                targetId += 1;
                break;

            // 위/아래 이동도 나중에 쓰고 싶으면 여기서 처리하면 됨
            default:
                return;
        }

        if (targetId < 0 || targetId >= sessionData.roomDatas.Count)
        {
            return;
        }


        // RoomUpdate
        SceneManager.UnloadSceneAsync(currentSession.roomSceneName);
        currentSession = sessionData.roomDatas[targetId];
        SceneManager.LoadSceneAsync(currentSession.roomSceneName, LoadSceneMode.Additive);
    }

    public string GetName() => stateName;
}
#endregion


#region RewardState
public class RewardState : IGameState
{
    private string stateName = "RewardState";
    public void Enter()
    {
        
    }
    public void Exit()
    {
        
    }

    public void Move(Navigate navigate)
    {
        return;
    }
    public string GetName() => stateName;
}
#endregion