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

    private int stageLevel;
    private RoomSession currentRoomSession;
    public List<RoomSession> roomDatas = new List<RoomSession>();

    public StageState(int stageLevel)
    {
        this.stageLevel = stageLevel;

        // RoomGenerate
        roomDatas.Add(new RoomSession(0, stageLevel, Resources.Load<StageDefaultSettings>("Level/RoomSettings1")));
        roomDatas.Add(new RoomSession(1, stageLevel, Resources.Load<StageDefaultSettings>("Level/RoomSettings2")));
        roomDatas.Add(new RoomSession(2, stageLevel, Resources.Load<StageDefaultSettings>("Level/RoomSettings3")));

        currentRoomSession = roomDatas[0];
    }



    public void Enter()
    {
        SceneManager.LoadSceneAsync(currentRoomSession.roomSceneName, LoadSceneMode.Additive);
        foreach (var es in currentRoomSession.enemySessions)
        {
            if (es.isDead) continue;
            if (es.settings == null) continue;
            if (es.settings.prefab == null) continue;

            Vector2 pos = es.settings.spawnPoint;
            GameManager.Instance.CharacterInstantiate(es.settings.prefab, pos, es.enemyId);

        }
    }

    public void Exit()
    {
        Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>(true);
        foreach (var enemy in enemies)
        {
            if (!enemy.gameObject.activeInHierarchy)
                currentRoomSession.RemoveEnemy(GameManager.Instance.enemies[enemy.gameObject]);
            GameObject.Destroy(enemy.gameObject);
        }
        SceneManager.UnloadSceneAsync(currentRoomSession.roomSceneName);
    }

    public void Move(Navigate nav)
    {
        int targetId = currentRoomSession.roomId;

        switch (nav)
        {
            case Navigate.left:
                targetId -= 1;
                nav = Navigate.right;
                break;

            case Navigate.right:
                targetId += 1;
                nav = Navigate.left;
                break;

            // 위/아래 이동도 나중에 쓰고 싶으면 여기서 처리하면 됨
            default:
                return;
        }

        if (targetId < 0 || targetId >= roomDatas.Count)
        {
            return;
        }


        // RoomUpdate
        Exit();
        currentRoomSession = roomDatas[targetId];
        Enter();
        GameManager.Instance.setPlayer(currentRoomSession.getNavigate(nav));
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