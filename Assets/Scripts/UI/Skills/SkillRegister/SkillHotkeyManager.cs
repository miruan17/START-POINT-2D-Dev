using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Utilities;

public class SkillHotkeyManager : MonoBehaviour
{
    private SkillSlot[] slots;
    public PlayerSkill playerSkill;

    private void Awake()
    {
        // Scene 내의 모든 SkillSlot 자동 등록
        slots = FindObjectsOfType<SkillSlot>(true)
            .OrderBy(s => s.transform.GetSiblingIndex())
            .ToArray();
        Debug.Log($"[SkillHotkeyManager] {slots.Length} hotkey slots found.");
    }

    private void Update()
    {
        // ① 활성화 여부
        if (!isActiveAndEnabled)
            return;

        // ② 현재 포커스 확인
        if (UIFocusController.Instance?.currentFocus != UIFocusController.UIFocusTarget.SkillRegister)
            return;

        // ④ 실제 입력 감지
        SkillNodeDef focusedSkill = SkillRegisterManager.CurrentFocusedSkill;
        if (focusedSkill == null)
            return;
        foreach (SkillSlot slot in slots)
        {
            if (Input.GetKeyDown(slot.Hotkey))
            {
                if (slot.assignedSkill != null && slot.assignedSkill.skillName.Equals(focusedSkill.skillName)) continue;
                AssignSkillToSlot(focusedSkill, slot);
                Debug.Log($"[SkillHotkeyManager] {slot.Hotkey} pressed!");
                break;
            }
        }
    }


    private void AssignSkillToSlot(SkillNodeDef skill, SkillSlot targetSlot)
    {
        // 중복 방지: 이미 다른 슬롯에 존재하면 제거
        for (int i = 0; i < 4; i++)
        {
            var slot = slots[i];
            Debug.Log(slot.Hotkey);
            if (slot.IsAssigned(skill))
            {
                playerSkill.UpdateActiveSkill(i, null);
                slot.ClearSlot();
            }
            if (slot.Equals(targetSlot))
            {
                // 새로 지정
                targetSlot.AssignSkill(skill);
                playerSkill.UpdateActiveSkill(i, targetSlot.assignedSkill);

            }
        }

    }
}
