using UnityEngine;
using System.Linq;

public partial class SkillTree
{
    [SerializeField] private SkillNodeBase mainNode;

    /// 방향 벡터(dir)를 따라 메인 노드와 서브 노드를 배치
    /// ScrollRect Content 공간(UI 좌표계) 기준
    public Vector2 LayoutAlongUI(Vector2 dir, RectTransform content, float branchLen, float subOffset)
    {
        if (!mainNode)
        {
            mainNode = GetComponentsInChildren<SkillNodeBase>(true)
                        .FirstOrDefault(n => n.Definition != null && n.Definition.isMainNode);
        }

        var subNodes = GetComponentsInChildren<SkillNodeBase>(true)
                       .Where(n => n && n != mainNode).Take(2).ToList();

        Vector2 dirN = dir.normalized;
        Vector2 mainPos = dirN * branchLen;

        // 메인 노드 배치
        var mainRT = (RectTransform)mainNode.transform;
        PlaceAnchored(mainRT, content, mainPos);

        // 수직 벡터 (왼쪽 우선)
        Vector2 perpLeft = new Vector2(-dirN.y, dirN.x);

        for (int i = 0; i < subNodes.Count; i++)
        {
            Vector2 offset = (i == 0) ? perpLeft * subOffset : -perpLeft * subOffset;
            var subRT = (RectTransform)subNodes[i].transform;
            PlaceAnchored(subRT, content, mainPos + offset);
        }

        return mainPos;
    }

    private static void PlaceAnchored(RectTransform node, RectTransform content, Vector2 anchoredPos)
    {
        if (node.transform.parent != content)
            node.SetParent(content, worldPositionStays: false);

        node.anchorMin = node.anchorMax = new Vector2(0.5f, 0.5f);
        node.pivot = new Vector2(0.5f, 0.5f);
        node.anchoredPosition = anchoredPos;
        node.localRotation = Quaternion.identity;
        node.localScale = Vector3.one;
    }
}
