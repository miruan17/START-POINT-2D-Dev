using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class SkillRegisterManager : MonoBehaviour
{
    [Header("Skill UI Settings")]
    [SerializeField] private Transform contentParent;      // ScrollView의 Content
    [SerializeField] private SkillRegisterIcon skillIconPrefab;   // Skill 아이콘 프리팹

    [Header("Runtime Data")]
    public List<SkillNodeBase> unlockedSkillList = new();  // 해금된 스킬 리스트
    private readonly List<SkillRegisterIcon> spawnedIcons = new(); // 생성된 UI 오브젝트 캐시


    public void updateUnlockedSkillList(List<SkillNodeBase> unlockedSkillList)
    {
        this.unlockedSkillList = unlockedSkillList;
        RefreshUI();
    }
    private void RefreshUI()
    {
        var rect = (RectTransform)contentParent;
        rect.sizeDelta = new Vector2(0, rect.sizeDelta.y);

        if (contentParent == null || skillIconPrefab == null)
        {
            Debug.LogWarning("[SkillRegisterManager] ContentParent or Prefab not assigned!");
            return;
        }

        // 기존 아이콘 제거
        foreach (var icon in spawnedIcons)
        {
            if (icon != null)
                Destroy(icon.gameObject);
        }
        spawnedIcons.Clear();
        int cnt = 0;

        // 새 아이콘 생성
        foreach (SkillNodeBase node in unlockedSkillList)
        {
            if (node == null || node.Definition == null)
                continue;

            SkillRegisterIcon icon = Instantiate(skillIconPrefab, contentParent);
            icon.name = $"Skill_{node.Definition.skillName}";
            icon.index = cnt++;
            icon.Definition = node.Definition.Clone();
            icon.parent = this;
            if (icon.Definition.icon != null && icon.icon != null)
            {
                icon.icon.sprite = icon.Definition.icon;
            }

            spawnedIcons.Add(icon);
        }
    }
}
