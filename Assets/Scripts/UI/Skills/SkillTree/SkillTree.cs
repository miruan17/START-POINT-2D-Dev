using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public partial class SkillTree : MonoBehaviour
{
    [SerializeField] private string treeId;
    [SerializeField] private Button LvUpBtn;
    private ISkillPointProvider points;
    private SkillTreeManager parent;

    private readonly Dictionary<string, SkillNodeBase> nodes = new();
    private readonly Dictionary<SkillNodeDef, SkillNodeBase> nodeMapByDef = new();
    public IReadOnlyDictionary<string, SkillNodeBase> Nodes => nodes;
    public void BindPointProvider(ISkillPointProvider provider) => points = provider;
    public void BindSkillTreeManager(SkillTreeManager parent) => this.parent = parent;
    void Awake()
    {
        foreach (var node in GetComponentsInChildren<SkillNodeBase>(true))
        {
            if (node == null || string.IsNullOrEmpty(node.Id)) continue;
            nodeMapByDef[node.Definition] = node;
            nodes[node.Id] = node;
            node.Bind(this);
        }
        LvUpBtn.onClick.AddListener(OnClicked);
        RefreshAll();
    }
    void OnClicked()
    {
        FocusedNode.usePoint();
    }
    public int AvailablePoints => points?.GetAvailable() ?? 0;
    public bool TrySpendPoints(int cost) => points != null && points.TrySpend(cost);

    public bool IsNodeUnlocked(string id)
        => nodes.TryGetValue(id, out var n) && n != null && n.IsUnlocked;

    public SkillNodeBase FindNode(SkillNodeDef def)
    {
        return def != null && nodeMapByDef.TryGetValue(def, out var node) ? node : null;
    }
    public void NotifyUnlocked(SkillNodeBase node)
    {
    }

    public void addUnlockedSkilltoList(SkillNodeBase unlocked)
    {
        parent.addUnlockedSkilltoList(unlocked);
    }

    public void RefreshDependents(SkillNodeBase changed)
    {
        RefreshAll();
    }

    public void RefreshAll()
    {
        foreach (var n in nodes.Values) n.RefreshVisual();
    }
    public string getTreeId()
    {
        return treeId;
    }
    public bool TryGetNode(SkillNodeDef def, out SkillNodeBase node)
    {
        return nodeMapByDef.TryGetValue(def, out node);
    }
}
