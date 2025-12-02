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

// 세션 데이터
public class RoomSession
{
    public int RoomId;
    public string RoomName;
    // 주의: 새로운 Room 기준이 아니라 기존의 Room 기준
    private Dictionary<Transform, Navigate> passage;

    public RoomSession(int roomId, string roomName)
    {
        RoomId = roomId;
        RoomName = roomName;
    }
}
public class StageState : IGameState
{
    private string stateName = "StageState";

    // Room Scene Name Library
    private string[] roomSeed = { "stage1", "stage2", "stage3" };

    // Runtime Session
    private List<RoomSession> sessionData;
    private RoomSession currentSession;



    // 추후에 시간 남으면 랜덤생성기능 설정
    public StageState()
    {
        sessionData = new List<RoomSession>();
        sessionData.Add(new RoomSession(0, roomSeed[0]));
        sessionData.Add(new RoomSession(1, roomSeed[1]));
        sessionData.Add(new RoomSession(2, roomSeed[2]));

        currentSession = sessionData[0];

    }

    public void Enter()
    {
        SceneManager.LoadSceneAsync(currentSession.RoomName,LoadSceneMode.Additive);
    }

    public void Exit()
    {
        SceneManager.UnloadSceneAsync(currentSession.RoomName);
    }
    public void Move(Navigate nav)
    {
    int targetId = currentSession.RoomId;

        switch (nav)
            {
                case Navigate.left:
                    targetId -= 1;
                    break;

                case Navigate.right:
                    targetId += 1;
                    break;

                default:
                    return;
            }
            
        if (targetId < 0 || targetId >= sessionData.Count)
        {
            return;
        }
        SceneManager.UnloadSceneAsync(currentSession.RoomName);

        // Session Update
        currentSession = sessionData[targetId];
        SceneManager.LoadSceneAsync(currentSession.RoomName, LoadSceneMode.Additive);
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