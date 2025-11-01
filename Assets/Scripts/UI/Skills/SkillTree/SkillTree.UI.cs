// SkillTree.UI.cs  (UI-only partial for keyboard focus)
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public partial class SkillTree : MonoBehaviour
{
    public enum NavDir { Up = 0, Left = 1, Down = 2, Right = 3 }

    [Header("UI Navigation")]
    [SerializeField] private bool enableKeyboardNav = true;
    [Header("UI Elements")]
    [SerializeField] private Text descriptionText;
    [SerializeField] private ScrollFocusController scrollController;
    private SkillNodeBase _focused;
    public SkillNodeBase FocusedNode => _focused;

    private void Start()
    {
        //id가 0인 노드에 최초 포커싱
        var start = nodes.Values.FirstOrDefault(n => n.Definition != null && n.Definition.id == "0");
        if (start != null)
            FocusNode(start);
    }

    private void Update()
    {
        if (!enableKeyboardNav) return;
        //방향키 입력이 들어오면 포커싱 옮기기
        if (Input.GetKeyDown(KeyCode.UpArrow)) MoveFocus(NavDir.Up);
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) MoveFocus(NavDir.Left);
        else if (Input.GetKeyDown(KeyCode.DownArrow)) MoveFocus(NavDir.Down);
        else if (Input.GetKeyDown(KeyCode.RightArrow)) MoveFocus(NavDir.Right);
    }

    public void FocusNode(SkillNodeBase node)
    {
        if (node == null) return;

        // Update internal state
        _focused = node;

        // Push selection into EventSystem (falls back to the node GO if Button is non-interactable)
        var btn = node.GetComponent<UnityEngine.UI.Button>();
        var target = btn ? btn.gameObject : node.gameObject;
        EventSystem.current?.SetSelectedGameObject(target);

        // 포커싱된 노드의 스킬 설명
        if (descriptionText != null && node.Definition != null)
        {
            descriptionText.text = node.Definition.description ?? "";
            //Debug.Log(node.Definition.description);
        }
        if (scrollController != null)
            scrollController.FocusOn(node.transform, node.Definition.skillName);


        // Optional: add focus-specific visuals inside RefreshVisual if desired
        //Debug.Log(node.Id);
        RefreshAll();
    }

    public void MoveFocus(NavDir dir)
    {

        if (_focused == null || _focused.Definition == null) return;

        var links = _focused.Definition.prerequisiteSkills; // SkillNodeDef[4]
        int idx = (int)dir;
        if (links == null || idx < 0 || idx >= links.Length) return;

        //누른 방향에 해당하는 노드가 존재할때만 이동
        var targetDef = links[idx];
        if (targetDef == null) return;

        if (nodeMapByDef.TryGetValue(targetDef, out var targetNode) && targetNode != null)
            FocusNode(targetNode);
    }
}
