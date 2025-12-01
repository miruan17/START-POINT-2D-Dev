using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillRegisterIcon : MonoBehaviour, IPointerClickHandler, ISubmitHandler
{
    public SkillNodeDef Definition;
    public SkillRegisterManager parent;
    public int index;
    [SerializeField] public Image icon;
    [SerializeField] public Button interactBtn;
    public void OnPointerClick(PointerEventData eventData)
    {
        HandleClick(isKeyboard: false);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        HandleClick(isKeyboard: true);
    }
    private void HandleClick(bool isKeyboard)
    {
        if (!isKeyboard)
            if (parent != null) parent.FocusIcon(index);
    }
}