using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public abstract class SkillNodeBase : MonoBehaviour
{
    [SerializeField] protected SkillNodeDef definition;
    [SerializeField] protected Image iconImage;
    [SerializeField] protected Button button;

    public string Id => definition?.id;
    public bool IsUnlocked { get; private set; }
    public SkillTree Owner { get; private set; }
    public SkillNodeDef Definition => definition;

    public void Bind(SkillTree owner)
    {
        Owner = owner;
        if (iconImage && definition?.icon) iconImage.sprite = definition.icon;
        button.onClick.AddListener(OnClicked);
        RefreshVisual();
    }

    protected virtual void OnDestroy()
    {
        if (button) button.onClick.RemoveListener(OnClicked);
    }

    void OnClicked()
    {
        if (Owner == null || definition == null) return;
        if (!CanUnlock()) return;

        if (Owner.TrySpendPoints(definition.cost))
        {
            IsUnlocked = true;
            Owner.NotifyUnlocked(this);
            RefreshVisual();
            Owner.RefreshDependents(this);
        }
    }

    public bool CanUnlock()
    {
        if (IsUnlocked || definition == null) return false;
        if (Owner.AvailablePoints < definition.cost) return false;
        return definition.prerequisiteIds.All(pid => Owner.IsNodeUnlocked(pid));
    }

    public void RefreshVisual()
    {
        bool interactable = !IsUnlocked && CanUnlock();
        if (button) button.interactable = interactable;
        if (iconImage)
            iconImage.color = IsUnlocked ? new Color(1, 1, 1, 1) : (interactable ? new Color(1, 1, 1, 0.9f) : new Color(1, 1, 1, 0.35f));
    }
}
