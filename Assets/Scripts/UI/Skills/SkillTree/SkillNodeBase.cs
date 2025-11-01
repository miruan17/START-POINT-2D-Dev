using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class SkillNodeBase : MonoBehaviour, IPointerClickHandler, ISubmitHandler
{
    [SerializeField] protected SkillNodeDef definition;
    [SerializeField] protected Image iconImage;
    [SerializeField] protected Button button;

    public string Id => definition?.id;
    public bool IsUnlocked { get; private set; }
    public SkillTree Owner { get; private set; }
    public SkillNodeDef Definition => definition;

    // SkillTree에서 호출하는 Bind 함수
    public void Bind(SkillTree owner)
    {
        Owner = owner;
        if (iconImage && definition?.icon) iconImage.sprite = definition.icon;
        if (definition != null && definition.id == "0")
            IsUnlocked = true;
        button.onClick.AddListener(OnClicked);
        RefreshVisual();
    }

    protected virtual void OnDestroy()
    {
        if (button) button.onClick.RemoveListener(OnClicked);
    }

    // 스킬 노드 버튼이 눌렸을 때 동작할 함수
    void OnClicked()
    {
    }

    public bool CanUnlock()
    {
        if (IsUnlocked || definition == null) return false;
        if (Owner.AvailablePoints < definition.cost) return false;
        if (definition.prerequisiteSkills.All(p => p == null))
            return true;
        return definition.prerequisiteSkills.Any(p => p != null && Owner.IsNodeUnlocked(p.id));
    }

    public void RefreshVisual()
    {
        bool interactable = !IsUnlocked && CanUnlock();

        if (button)
            button.interactable = interactable;

        if (iconImage)
        {
            // 기본 색상
            Color targetColor;

            if (IsUnlocked) targetColor = Color.white;
            else if (interactable) targetColor = new Color(1f, 1f, 1f, 0.9f);
            else targetColor = new Color(1f, 1f, 1f, 0.35f);

            // 포커스된 상태일 경우 강조 (예: 노란색 테두리 느낌)
            if (Owner != null && Owner.FocusedNode == this)
                targetColor = Color.yellow; // 혹은 Outline/Scale 조정으로 대체 가능

            iconImage.color = targetColor;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log($"[MouseClick] {definition.skillName}");
        HandleClick(isKeyboard: false);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        //Debug.Log($"[KeyboardEnter] {definition.skillName}");
        HandleClick(isKeyboard: true);
    }
    private void HandleClick(bool isKeyboard)
    {
        if (isKeyboard)
        {
            usePoint();
        }
        else
        {
            if (Owner.FocusedNode != this)
                Owner.FocusNode(this);
        }
    }
    public void usePoint()
    {
        if (Owner == null || definition == null) return;
        if (!CanUnlock()) return;

        if (Owner.TrySpendPoints(definition.cost))
        {
            IsUnlocked = true;
            Owner.NotifyUnlocked(this);

            if (definition.isMainNode)
                Owner.addUnlockedSkilltoList(this);

            Owner.RefreshAll();
        }
    }
}
