using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class RoomSession
{
    public readonly int roomId;
    public readonly int roomLevel;
    public readonly string roomSceneName;

    // Session Field
    
    public readonly List<EnemySession> enemySessions = new List<EnemySession>();

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

        int id = 0;
        if (sds.enemyList != null)
        {
            foreach (var enemyDef in sds.enemyList)
            {
                if (enemyDef == null) continue;
                var session = new EnemySession(id++, enemyDef);
                enemySessions.Add(session);
            }
        }
        
    }

    public Vector2 getNavigate(Navigate navigate) => navigateSession[navigate];

    public void RemoveEnemy(int enemyId)
    {
        enemySessions.RemoveAll(e => e.enemyId == enemyId);
    }
}

public class EnemySession
{
    public readonly int enemyId;
    public readonly EnemyDefaultSettings settings;
    public bool isDead;

    public EnemySession(int enemyId, EnemyDefaultSettings settings)
    {
        this.enemyId = enemyId;
        this.settings = settings;
        isDead = false;
    }
}
