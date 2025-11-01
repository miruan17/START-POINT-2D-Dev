using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Utilities;

public class SkillHotkeyManager : MonoBehaviour
{
    private SkillSlot[] slots;

    private void Awake()
    {
        // Scene 내의 모든 SkillSlot 자동 등록
        slots = FindObjectsOfType<SkillSlot>(true);
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
                AssignSkillToSlot(focusedSkill, slot);
                Debug.Log($"[SkillHotkeyManager] {slot.Hotkey} pressed!");
                break;
            }
        }
    }


    private void AssignSkillToSlot(SkillNodeDef skill, SkillSlot targetSlot)
    {
        // 중복 방지: 이미 다른 슬롯에 존재하면 제거
        foreach (var slot in slots)
        {
            if (slot.IsAssigned(skill))
                slot.ClearSlot();
        }

        // 새로 지정
        targetSlot.AssignSkill(skill);
    }
}
