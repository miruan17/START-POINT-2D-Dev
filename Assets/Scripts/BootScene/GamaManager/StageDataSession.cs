using System.Collections.Generic;
using UnityEngine;

public class StageDataSession
{
    public List<RoomSession> roomDatas = new List<RoomSession>();

    public StageDataSession()
    {
        roomDatas.Add(new RoomSession(0, "stage1", Resources.Load<RoomPassage>("Level/RoomPassageCase1"), 1));
        roomDatas.Add(new RoomSession(1, "stage2", Resources.Load<RoomPassage>("Level/RoomPassageCase2"), 1));
        roomDatas.Add(new RoomSession(2, "stage3", Resources.Load<RoomPassage>("Level/RoomPassageCase3"), 1));
    }
}

public class RoomSession
{
    public readonly int roomId;
    public readonly string roomSceneName;
    public readonly Dictionary<Navigate, Vector2> passage;
    public readonly int difficulty;

    public RoomSession(int roomId, string roomSceneName, RoomPassage roompassage, int difficulty)
    {
        this.roomId = roomId;
        this.roomSceneName = roomSceneName;

        passage = new Dictionary<Navigate, Vector2>();

        if (roompassage == null)
        {
            Debug.LogError($"[RoomSession] RoomPassage is NULL for roomSceneName = {roomSceneName}. Resources 경로 확인 필요.");
            return;
        }

        passage.Add(Navigate.left,  roompassage.left);
        passage.Add(Navigate.right, roompassage.right);
        passage.Add(Navigate.up,    roompassage.up);
        passage.Add(Navigate.down,  roompassage.down);
        passage.Add(Navigate.spawn, roompassage.spawn);

        this.difficulty = difficulty;
    }

    public Vector2 GetPassage(Navigate navigate) => passage[navigate];
}
