using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public partial class SkillRegisterManager : MonoBehaviour
{
    public enum NavDir { Up = 0, Left = 1, Down = 2, Right = 3 }
    private GameObject _focusedIcon;
    private int _focusedIndex = -1;
    [SerializeField] Text skillNameText;
    [SerializeField] Text descriptionText;

    private void Update()
    {
        if (!UIFocusController.Instance.IsFocused(UIFocusController.UIFocusTarget.SkillRegister))
            return;

        if (spawnedIcons == null || spawnedIcons.Count == 0)
            return;

        // 최초 포커스
        if (_focusedIcon == null)
        {
            skillNameText.text = "";
            descriptionText.text = "";
            if (Input.anyKeyDown)
                FocusIcon(0);
            return;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
            MoveFocus(NavDir.Up);
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            MoveFocus(NavDir.Left);
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            MoveFocus(NavDir.Down);
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            MoveFocus(NavDir.Right);
    }

    private void FocusIcon(int index)
    {
        if (index < 0 || index >= spawnedIcons.Count)
            return;

        _focusedIcon = spawnedIcons[index];
        _focusedIndex = index;

        // UI 시스템에 반영
        EventSystem.current?.SetSelectedGameObject(_focusedIcon);
        descriptionText.text = IconData[index].description;
        skillNameText.text = IconData[index].skillName;

        // 포커스 색상 반영
        RefreshFocusVisual();
    }

    private void RefreshFocusVisual()
    {
        for (int i = 0; i < spawnedIcons.Count; i++)
        {
            var img = spawnedIcons[i].GetComponent<Image>();
            if (img == null) continue;
            img.color = (i == _focusedIndex) ? Color.yellow : Color.white;
        }
    }

    private void MoveFocus(NavDir dir)
    {
        if (_focusedIndex < 0) return;

        int cols = 6; // 한 줄에 아이콘 6개라고 가정 (GridLayoutGroup에 맞게 조정)
        int rows = Mathf.CeilToInt(spawnedIcons.Count / (float)cols);
        int targetIndex = _focusedIndex;

        switch (dir)
        {
            case NavDir.Left:
                targetIndex = Mathf.Max(0, _focusedIndex - 1);
                break;
            case NavDir.Right:
                targetIndex = Mathf.Min(spawnedIcons.Count - 1, _focusedIndex + 1);
                break;
            case NavDir.Up:
                targetIndex = Mathf.Max(0, _focusedIndex - cols);
                break;
            case NavDir.Down:
                targetIndex = Mathf.Min(spawnedIcons.Count - 1, _focusedIndex + cols);
                break;
        }

        if (targetIndex != _focusedIndex)
            FocusIcon(targetIndex);
    }
}
