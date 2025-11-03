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
    private GameObject lastSkillTreeFocused;  // SkillTreeUI에서 마지막으로 선택된 노드 저장
    private GameObject lastRegisterFocused;   // RegisterPanel 내부 마지막 포커스 저장

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
        // 현재 포커스 대상에 따라 이전 선택 객체 저장
        if (currentFocus == UIFocusTarget.SkillTreeUI)
            lastSkillTreeFocused = EventSystem.current?.currentSelectedGameObject;
        else if (currentFocus == UIFocusTarget.SkillRegister)
            lastRegisterFocused = EventSystem.current?.currentSelectedGameObject;

        currentFocus = target;
        Debug.Log($"[UIFocusController] Focus switched to: {target}");

        // EventSystem에 포커스 복원
        if (target == UIFocusTarget.SkillTreeUI)
        {
            if (lastSkillTreeFocused != null)
                EventSystem.current?.SetSelectedGameObject(lastSkillTreeFocused);
        }
        else if (target == UIFocusTarget.SkillRegister)
        {
            if (lastRegisterFocused != null)
                EventSystem.current?.SetSelectedGameObject(lastRegisterFocused);
            else
                EventSystem.current?.SetSelectedGameObject(null);
        }
    }

    public bool IsFocused(UIFocusTarget target)
    {
        return currentFocus == target;
    }
}
