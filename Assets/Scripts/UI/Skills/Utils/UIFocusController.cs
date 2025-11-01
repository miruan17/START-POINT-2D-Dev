using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIFocusController : MonoBehaviour
{
    public static UIFocusController Instance { get; private set; }
    [SerializeField] private Button registerToggleButton;

    public enum UIFocusTarget
    {
        SkillTreeUI,
        SkillRegister
    }

    public UIFocusTarget currentFocus = UIFocusTarget.SkillTreeUI;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }
    private void Update()
    {
        if (currentFocus == UIFocusTarget.SkillRegister)
        {
            // 방향키 입력이 ScrollRect 등 다른 UI로 전달되지 않도록 막기
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) ||
                Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }
        // Tab 키 입력 감지
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // 실제로 버튼을 "클릭"하도록 호출
            if (registerToggleButton != null)
            {
                Debug.Log("[UIFocusController] Tab pressed → toggling SkillRegisterPanel");
                registerToggleButton.onClick.Invoke();
            }
        }
    }
    public void SetFocus(UIFocusTarget target)
    {
        currentFocus = target;
        Debug.Log($"[UIFocusController] Focus switched to: {target}");
    }

    public bool IsFocused(UIFocusTarget target)
    {
        return currentFocus == target;
    }
}
