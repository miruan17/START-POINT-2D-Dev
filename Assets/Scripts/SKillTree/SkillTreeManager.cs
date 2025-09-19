using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class SkillTreeManager : MonoBehaviour, ISkillPointProvider
{
    [SerializeField] private List<SkillTree> trees = new(); // optional manual wiring
    [SerializeField] private int startingPoints = 10;
    private int useableSkillPoints;
    void Awake()
    {
        // Auto-discover children if the list is empty or incomplete.
        if (trees == null || trees.Count == 0)
            trees = GetComponentsInChildren<SkillTree>(includeInactive: true).ToList();

        useableSkillPoints = startingPoints;

        // Initialize each tree once.
        foreach (var tree in trees)
        {
            if (tree == null) continue;
            tree.BindPointProvider(this);
            tree.RefreshAll(); // your method to build graph, bind UI, etc.
        }
    }

    public int GetAvailable() => useableSkillPoints;
    public bool TrySpend(int cost)
    {
        if (cost <= 0) return true;
        if (useableSkillPoints < cost)
        {
            Debug.Log($"Not enough points. Need {cost - useableSkillPoints} more.");
            return false;
        }
        useableSkillPoints -= cost;
        RefreshAll();
        return true;
    }
    public void AddPoints(int amount)
    {
        if (amount <= 0) return;
        useableSkillPoints += amount;
        RefreshAll();
    }

    public IEnumerable<SkillTree> Trees => trees;

    public SkillTree GetTreeById(string id) =>
        trees.FirstOrDefault(t => t.getTreeId() == id);

    public void RefreshAll()
    {
        foreach (var t in trees) t?.RefreshAll();
    }
}
