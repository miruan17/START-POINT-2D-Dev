using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class PlatformID : MonoBehaviour
{
    public int platformGroupID = -1;
    public bool leftBlocked = false;
    public bool rightBlocked = false;
}

public class FallPoint : MonoBehaviour
{
    public int platformGroupID;
    public Vector2 worldPos;
}

public class PlatformAutoBuilder : EditorWindow
{
    [MenuItem("Tools/Build Platform Groups + Fall Points")]
    public static void BuildPlatformGroups()
    {
        // ============================
        // 1) Gather Platform colliders
        // ============================
        PolygonCollider2D[] cols = GameObject.FindObjectsByType<PolygonCollider2D>(
            FindObjectsInactive.Exclude,
            FindObjectsSortMode.None
        );

        List<PolygonCollider2D> platforms = new List<PolygonCollider2D>();
        List<PolygonCollider2D> walls = new List<PolygonCollider2D>();   // ★ NEW ★

        foreach (var col in cols)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Platform"))
                platforms.Add(col);

            else if (col.gameObject.layer == LayerMask.NameToLayer("Wall")) // ★ NEW ★
                walls.Add(col);
        }

        if (platforms.Count == 0)
        {
            Debug.LogWarning("[PlatformAutoBuilder] No platform colliders found.");
            return;
        }

        int n = platforms.Count;
        int[] parent = new int[n];

        for (int i = 0; i < n; i++)
            parent[i] = i;

        // ============================
        // 2) Union-Find
        // ============================
        int Find(int x)
        {
            if (parent[x] == x) return x;
            return parent[x] = Find(parent[x]);
        }

        void Union(int a, int b)
        {
            a = Find(a);
            b = Find(b);
            if (a != b) parent[b] = a;
        }

        // ==========
        // 3) Grouping by intersection
        // ==========
        for (int i = 0; i < n; i++)
        {
            for (int j = i + 1; j < n; j++)
            {
                if (platforms[i].bounds.Intersects(platforms[j].bounds))
                    Union(i, j);
            }
        }

        // ======================================================
        // 4) Sort group roots so that the lowest Y platform = ID 0
        // ======================================================
        HashSet<int> uniqueRoots = new HashSet<int>();
        Dictionary<int, float> rootMinY = new Dictionary<int, float>();

        for (int i = 0; i < n; i++)
        {
            int root = Find(i);
            uniqueRoots.Add(root);

            float y = platforms[i].bounds.min.y;

            if (!rootMinY.ContainsKey(root))
                rootMinY[root] = y;
            else
                rootMinY[root] = Mathf.Min(rootMinY[root], y);
        }

        List<int> sortedRoots = new List<int>(uniqueRoots);
        sortedRoots.Sort((a, b) => rootMinY[a].CompareTo(rootMinY[b]));

        Dictionary<int, int> groupMap = new Dictionary<int, int>();

        for (int i = 0; i < sortedRoots.Count; i++)
            groupMap[sortedRoots[i]] = i; // lowest = 0

        // ======================================================
        // 5) Assign PlatformID
        // ======================================================
        for (int i = 0; i < n; i++)
        {
            int root = Find(i);

            PlatformID id = platforms[i].GetComponent<PlatformID>();
            if (id == null)
                id = platforms[i].gameObject.AddComponent<PlatformID>();

            id.platformGroupID = groupMap[root];
            EditorUtility.SetDirty(id);
        }

        // ============================
        // 7) Fall Point Generation
        // ============================
        int platformMask = LayerMask.GetMask("Platform");

        foreach (var col in platforms)
        {
            PlatformID pid = col.GetComponent<PlatformID>();
            int groupID = pid.platformGroupID;

            // ★ NEW RULE #1: Lowest platform (ID 0) → DO NOT generate fall points
            Bounds b = col.bounds;
            float left = b.min.x;
            float right = b.max.x;
            float top = b.max.y;

            foreach (var w in walls)
            {
                if (w.bounds.Intersects(new Bounds(new Vector3(left - 0.1f, top), new Vector3(0.2f, b.size.y))))
                    pid.leftBlocked = true;

                if (w.bounds.Intersects(new Bounds(new Vector3(right + 0.1f, top), new Vector3(0.2f, b.size.y))))
                    pid.rightBlocked = true;
            }
            foreach (var c in platforms)
            {
                PlatformID pid1 = c.GetComponent<PlatformID>();
                if (pid1.platformGroupID == groupID)
                {
                    pid.leftBlocked = (pid.leftBlocked || pid1.leftBlocked) ? true : false;
                    pid.rightBlocked = (pid.rightBlocked || pid1.rightBlocked) ? true : false;
                }
            }
        }

        Debug.Log($"[PlatformAutoBuilder] DONE! Groups = {groupMap.Count} (Lowest = ID 0), FallPoints generated.");
    }
}
