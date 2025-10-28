using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillTreeManager : MonoBehaviour, ISkillPointProvider
{
    [SerializeField] private List<SkillTree> trees = new();
    [SerializeField] private int startingPoints = 10; // 시작 Skill Point
    public UseableSkillPointTextUI skillPointText;
    public int useableSkillPoints { get; private set; }
    void Awake()
    {

        if (trees == null || trees.Count == 0)
            trees = GetComponentsInChildren<SkillTree>(includeInactive: true).ToList();

        useableSkillPoints = startingPoints;

        foreach (var tree in trees)
        {
            if (tree == null) continue;
            tree.BindPointProvider(this); // 스킬트리 - 매니저 맵핑 작업
            tree.RefreshAll();
        }
    }

    // 현재 사용가능 Skill Point의 getter 함수
    public int GetAvailable() => useableSkillPoints;

    // 주어진 cost에 대해 Useable Skill Point를 사용
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
    // Useable Skill Point를 추가 (ex. 레벨업)
    public void AddPoints(int amount)
    {
        if (amount <= 0) return;
        useableSkillPoints += amount;
        RefreshAll();
    }

    public IEnumerable<SkillTree> Trees => trees;

    // id로 Tree를 Search
    public SkillTree GetTreeById(string id) =>
        trees.FirstOrDefault(t => t.getTreeId() == id);

    // 각 tree를 Refresh
    public void RefreshAll()
    {
        foreach (var t in trees) t?.RefreshAll();
        skillPointText.UpdateText();
    }
}
