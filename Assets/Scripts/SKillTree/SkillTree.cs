using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class SkillTree : MonoBehaviour
{
    [SerializeField] private string treeId;
    private ISkillPointProvider points;
    private readonly Dictionary<string, SkillNodeBase> nodes = new();
    public void BindPointProvider(ISkillPointProvider provider) => points = provider;
    void Awake()
    {
        foreach (var node in GetComponentsInChildren<SkillNodeBase>(true))
        {
            if (node == null || string.IsNullOrEmpty(node.Id)) continue;
            nodes[node.Id] = node;
            node.Bind(this);
        }
        Debug.Log(nodes.Count);
        RefreshAll();
    }
    public int AvailablePoints => points?.GetAvailable() ?? 0;
    public bool TrySpendPoints(int cost) => points != null && points.TrySpend(cost);

    public bool IsNodeUnlocked(string id)
        => nodes.TryGetValue(id, out var n) && n != null && n.IsUnlocked;


    public void NotifyUnlocked(SkillNodeBase node)
    {
        Debug.Log(node.Id + "is unlocked");
    }

    public void RefreshDependents(SkillNodeBase changed)
    {
        RefreshAll();
    }

    public void RefreshAll()
    {
        foreach (var n in nodes.Values) n.RefreshVisual();
    }
    public string getTreeId()
    {
        return treeId;
    }
}
