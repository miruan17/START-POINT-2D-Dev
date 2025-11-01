using UnityEngine;
using UnityEngine.UI;

public class SkillRegisterToggleBtn : MonoBehaviour
{
    [SerializeField] private GameObject skillRegisterPanel;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(TogglePanel);
    }

    private void TogglePanel()
    {
        bool isActive = skillRegisterPanel.activeSelf;
        skillRegisterPanel.SetActive(!isActive);

        if (isActive)
            UIFocusController.Instance.SetFocus(UIFocusController.UIFocusTarget.SkillTreeUI);
        else
            UIFocusController.Instance.SetFocus(UIFocusController.UIFocusTarget.SkillRegister);
    }
}
