using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[DisallowMultipleComponent]
public class SkillSlot : MonoBehaviour
{
    [Header("Slot Settings")]
    [SerializeField] private HotkeyType hotkey;
    [SerializeField] private Image iconImage;
    [SerializeField] Text keyText;

    [HideInInspector] public SkillNodeDef assignedSkill;

    /// 현재 슬롯의 키 반환.
    public KeyCode Hotkey => (KeyCode)hotkey.ToKeyCode();
    private void Start()
    {
        ClearSlot();
    }
    /// 스킬을 슬롯에 등록
    public void AssignSkill(SkillNodeDef skill)
    {
        assignedSkill = skill;
        if (iconImage != null)
        {
            iconImage.sprite = skill.icon;
            iconImage.enabled = true;
        }
        Debug.Log($"[SkillSlot] {hotkey} assigned to {skill.skillName}");
    }

    /// 슬롯에 등록된 스킬을 제거
    public void ClearSlot()
    {
        assignedSkill = null;
        if (iconImage != null)
        {
            iconImage.sprite = null;
            iconImage.enabled = false;
        }
    }

    /// 특정 스킬이 이 슬롯에 이미 등록되어 있는지 확인
    public bool IsAssigned(SkillNodeDef skill) => assignedSkill?.skillName == skill.skillName;

    public bool HasSkill => assignedSkill != null;

#if UNITY_EDITOR // 이름 자동갱신
    private void OnValidate()
    {
        keyText.text = $"{hotkey}";
        gameObject.name = $"SkillSlot_{hotkey}";
    }
#endif
}
