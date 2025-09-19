using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class SkillTreeManager : MonoBehaviour, ISkillPointProvider
{
    [SerializeField] private List<SkillTree> trees = new();
    [SerializeField] private int startingPoints = 10;
    private int useableSkillPoints;
    void Awake()
    {

        if (trees == null || trees.Count == 0)
            trees = GetComponentsInChildren<SkillTree>(includeInactive: true).ToList();

        useableSkillPoints = startingPoints;


        foreach (var tree in trees)
        {
            if (tree == null) continue;
            tree.BindPointProvider(this);
            tree.RefreshAll();
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
