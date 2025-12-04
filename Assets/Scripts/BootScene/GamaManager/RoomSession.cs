using System.Collections.Generic;
using UnityEngine;

public class RoomSession
{
    public readonly int roomId;
    public readonly int roomLevel;
    public readonly string roomSceneName;

    // Session Field
    private Dictionary<int, GameObject> EnemySession = new Dictionary<int, GameObject>();


    // Navigate Field
    private Dictionary<Navigate, Vector2> navigateSession = new Dictionary<Navigate, Vector2>();


    public RoomSession(int roomId, int roomLevel, StageDefaultSettings sds)
    {

        this.roomId = roomId;
        this.roomLevel = roomLevel;
        this.roomSceneName = sds.sceneName;

        navigateSession.Add(Navigate.left,sds.left);
        navigateSession.Add(Navigate.right, sds.right);
        navigateSession.Add(Navigate.up,sds.up);
        navigateSession.Add(Navigate.down,sds.down);
        navigateSession.Add(Navigate.spawn, sds.spawn);

    }

    public Vector2 getNavigate(Navigate navigate) => navigateSession[navigate];
}