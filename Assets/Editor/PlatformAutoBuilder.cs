using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class PlatformID : MonoBehaviour
{
    public int platformGroupID = -1;
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
        // 1) Gather all active PolygonCollider2D in the scene
        PolygonCollider2D[] cols = GameObject.FindObjectsByType<PolygonCollider2D>(
            FindObjectsInactive.Exclude,
            FindObjectsSortMode.None
        );

        List<PolygonCollider2D> platforms = new List<PolygonCollider2D>();

        foreach (var col in cols)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Platform"))
                platforms.Add(col);
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

        // Union-Find
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

        // 2) Group by bounding-box intersection
        for (int i = 0; i < n; i++)
        {
            for (int j = i + 1; j < n; j++)
            {
                if (platforms[i].bounds.Intersects(platforms[j].bounds))
                    Union(i, j);
            }
        }

        // ---------- NEW GROUP MAP BUILDING (lowest Y group = 0) ----------
        HashSet<int> uniqueRoots = new HashSet<int>();
        Dictionary<int, float> rootMinY = new Dictionary<int, float>();

        // 1) collect roots and track lowest Y per group
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

        // 2) sort roots by minY (ascending)
        List<int> sortedRoots = new List<int>(uniqueRoots);
        sortedRoots.Sort((a, b) => rootMinY[a].CompareTo(rootMinY[b]));

        // 3) build groupMap: lowest Y root becomes groupID = 0
        Dictionary<int, int> groupMap = new Dictionary<int, int>();

        for (int i = 0; i < sortedRoots.Count; i++)
        {
            groupMap[sortedRoots[i]] = i; // i starts from 0
        }

        // 4) assign PlatformID based on reordered groups
        for (int i = 0; i < n; i++)
        {
            int root = Find(i);

            PlatformID id = platforms[i].GetComponent<PlatformID>();
            if (id == null)
                id = platforms[i].gameObject.AddComponent<PlatformID>();

            id.platformGroupID = groupMap[root];
            EditorUtility.SetDirty(id);
        }


        // Delete old fall points
        foreach (var fp in GameObject.FindObjectsByType<FallPoint>(
            FindObjectsInactive.Exclude,
            FindObjectsSortMode.None))
        {
            GameObject.DestroyImmediate(fp.gameObject);
        }

        // 4) Generate Fall Points
        float sampleStep = 0.3f;
        float forwardOffset = 0.2f;
        float rayDownDist = 1.0f;

        for (int i = 0; i < n; i++)
        {
            PolygonCollider2D col = platforms[i];
            PlatformID pid = col.GetComponent<PlatformID>();
            int groupID = pid.platformGroupID;

            Bounds b = col.bounds;
            float left = b.min.x;
            float right = b.max.x;
            float top = b.max.y;

            for (float x = left; x <= right; x += sampleStep)
            {
                // Position directly above platform
                Vector2 foot = new Vector2(x, top + 0.05f);

                // Ray 1: current foot must detect ground
                bool groundBelow =
                    Physics2D.Raycast(foot, Vector2.down, rayDownDist, 1 << LayerMask.NameToLayer("Platform"));

                if (!groundBelow) continue;

                // Ray 2: foot slightly forward must NOT detect ground
                Vector2 forwardPos = new Vector2(x + forwardOffset, top + 0.05f);
                bool forwardGround =
                    Physics2D.Raycast(forwardPos, Vector2.down, rayDownDist, 1 << LayerMask.NameToLayer("Platform"));

                if (forwardGround) continue;

                // Fall point confirmed
                GameObject fp = new GameObject($"FallPoint_{groupID}_{x:F2}");
                fp.transform.position = foot;

                FallPoint fall = fp.AddComponent<FallPoint>();
                fall.platformGroupID = groupID;
                fall.worldPos = foot;

                EditorUtility.SetDirty(fall);
            }
        }

        Debug.Log($"[PlatformAutoBuilder] Done! Platform Groups = {groupMap.Count}, FallPoints generated.");
    }
}
