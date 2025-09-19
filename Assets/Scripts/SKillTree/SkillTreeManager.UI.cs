using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public sealed partial class SkillTreeManager
{
    [Header("UI Layout (World-Space ScrollRect)")]
    [SerializeField] private RectTransform content;   // ScrollView Content
    [SerializeField] private float branchLength = 300f;
    [SerializeField] private float subOffset = 120f;
    [SerializeField] private float startAngleDeg = 0f;   // 기준 각도
    [SerializeField] private bool clockwise = false;
    [SerializeField] private float contentMargin = 200f;
    [SerializeField] private bool autoResizeContent = true;

    /// <summary>
    /// 트리들을 원형으로 배치합니다. ScrollRect Content 기준.
    /// </summary>
    [ContextMenu("Arrange Trees (UI)")]
    public void ArrangeTreesUI()
    {
        if (!content)
        {
            Debug.LogWarning("SkillTreeManager: Content is not assigned.");
            return;
        }
        if (trees == null || trees.Count == 0)
            trees = GetComponentsInChildren<SkillTree>(true).ToList();

        int n = trees.Count;
        if (n == 0) return;

        float step = 360f / n;
        var placed = new List<Vector2>(n);

        for (int i = 0; i < n; i++)
        {
            float angle = startAngleDeg + (clockwise ? -i : i) * step;
            float rad = angle * Mathf.Deg2Rad;
            Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

            var tree = trees[i];
            if (!tree) continue;

            Vector2 mainPos = tree.LayoutAlongUI(dir, content, branchLength, subOffset);
            placed.Add(mainPos);
        }

        if (autoResizeContent && placed.Count > 0)
        {
            float minX = placed.Min(p => p.x), maxX = placed.Max(p => p.x);
            float minY = placed.Min(p => p.y), maxY = placed.Max(p => p.y);

            Vector2 size = new Vector2(
                Mathf.Max(content.sizeDelta.x, (maxX - minX) + contentMargin),
                Mathf.Max(content.sizeDelta.y, (maxY - minY) + contentMargin)
            );
            content.sizeDelta = size;
        }
    }
}
