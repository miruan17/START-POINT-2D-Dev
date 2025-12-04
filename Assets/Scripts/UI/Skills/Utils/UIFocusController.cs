using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIFocusController : MonoBehaviour
{
    public static UIFocusController Instance { get; private set; }
    public bool isEnter = false;

    public enum UIFocusTarget
    {
        SkillTreeUI,
        SkillRegister,
        SkillPanel
    }

    public UIFocusTarget currentFocus = UIFocusTarget.SkillPanel;
    public UIFocusTarget nextFocus = UIFocusTarget.SkillPanel;
    private GameObject lastSkillTreeFocused;  // SkillTreeUI에서 마지막으로 선택된 노드 저장
    private GameObject lastRegisterFocused;   // RegisterPanel 내부 마지막 포커스 저장

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }
    private void OnEnable()
    {
        nextFocus = UIFocusTarget.SkillPanel;
        SetFocus(nextFocus);
        EventSystem.current.SetSelectedGameObject(null);
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
        if (!isEnter)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                nextFocus = UIFocusTarget.SkillTreeUI;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                nextFocus = UIFocusTarget.SkillRegister;
            }
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                isEnter = true;
                SetFocus(nextFocus);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                nextFocus = UIFocusTarget.SkillPanel;
                EventSystem.current?.SetSelectedGameObject(null);
                SetFocus(nextFocus);
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
        if (currentFocus == UIFocusTarget.SkillPanel) { isEnter = false; return; }
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
    public void OnClicked(int now)
    {
        if (now == 1) // SkillTreeUI
        {
            nextFocus = UIFocusTarget.SkillTreeUI;
            SetFocus(nextFocus);
            isEnter = true;
        }
        if (now == 2) // SkillRegisterUI
        {
            nextFocus = UIFocusTarget.SkillRegister;
            SetFocus(nextFocus);
            isEnter = true;
        }
    }
}
