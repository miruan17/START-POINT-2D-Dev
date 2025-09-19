using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillTree : MonoBehaviour
{
    [SerializeField] private string treeId;
    private ISkillPointProvider points;
    private readonly Dictionary<string, SkillNodeBase> _nodes = new();
    public void BindPointProvider(ISkillPointProvider provider) => _points = provider;

    void Awake()
    {
        // Discover nodes under this tree
        foreach (var node in GetComponentsInChildren<SkillNodeBase>(true))
        {
            if (node == null || string.IsNullOrEmpty(node.Id)) continue;
            _nodes[node.Id] = node;
            node.Bind(this);
        }
        RefreshAll();
    }
    public int AvailablePoints => points?.GetAvailable() ?? 0;
    public bool TrySpendPoints(int cost) => points != null && points.TrySpend(cost);

    public bool IsNodeUnlocked(string id)
        => _nodes.TryGetValue(id, out var n) && n != null && n.IsUnlocked;


    public void NotifyUnlocked(SkillNodeBase node)
    {
        // Save progress, raise events, SFX/VFX, etc.
        Debug.Log(node.Id + "is unlocked");
    }

    public void RefreshDependents(SkillNodeBase changed)
    {
        // Simple: re-evaluate all nodes’ interactable state
        RefreshAll();
    }

    public void RefreshAll()
    {
        foreach (var n in _nodes.Values) n.RefreshVisual();
    }
    public string getTreeId()
    {
        return treeId;
    }
}
