using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeManager : MonoBehaviour, ISkillPointProvider
{
    [SerializeField] private List<SkillTree> trees = new();
    [SerializeField] private int startingPoints = 10; // 시작 Skill Point
    [SerializeField] private Text skillPointText;
    [SerializeField] private SkillRegisterManager skillRegisterManager;
    private List<SkillNodeBase> unlockedSkillList = new();
    private List<SkillNodeBase> unlockedPassiveList = new();
    public int useableSkillPoints { get; private set; }
    void Awake()
    {

        if (trees == null || trees.Count == 0)
            trees = GetComponentsInChildren<SkillTree>(includeInactive: true).ToList();

        useableSkillPoints = startingPoints;
        skillPointText.text = "스킬 포인트: " + useableSkillPoints;

        foreach (var tree in trees)
        {
            if (tree == null) continue;
            tree.BindSkillTreeManager(this); // 스킬트리 - 매니저 맵핑 작업
            tree.BindPointProvider(this);
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
            //Debug.Log($"Not enough points. Need {cost - useableSkillPoints} more.");
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

    // Unlocked Skill을 list에 추가
    public void addUnlockedSkilltoList(SkillNodeBase unlocked)
    {
        if (!unlocked.Definition.isMainNode)
        {
            Player player = FindObjectOfType<Player>();
            Effect getter = player.getEffectLib().getEffectbyID(unlocked.Definition.tag);
            if (getter != null) // is upgrade node
            {
                getter.upgrade();
            }
            else // normal subnode (ex. hp up, speed up ...)
            {

            }
            return;
        }
        if (unlocked.Definition.isPassive)
        {
            unlockedPassiveList.Add(unlocked);
            Player player = FindObjectOfType<Player>();
            player.setPassiveSkillList(unlockedPassiveList);
            return;
        }
        if (unlockedSkillList.Contains(unlocked)) // 이미 리스트에 들어가 있는 경우는 추가 대신 갱신 (ex 스킬 레벨업)
        {
            var exist = unlockedSkillList.First(n => n == unlocked);
            exist = unlocked;
        }
        else unlockedSkillList.Add(unlocked);
        skillRegisterManager?.updateUnlockedSkillList(unlockedSkillList);
    }

    // 각 tree를 Refresh
    public void RefreshAll()
    {
        foreach (var t in trees) t?.RefreshAll();
        skillPointText.text = "스킬 포인트: " + useableSkillPoints;
    }
}
