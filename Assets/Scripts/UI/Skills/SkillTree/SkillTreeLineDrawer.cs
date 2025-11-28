using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeLineDrawer : MonoBehaviour
{
    [SerializeField] private RectTransform content;
    [SerializeField] private Image linePrefab;
    private readonly List<Image> activeLines = new();

    [SerializeField] private SkillTree tree;
    public RectTransform skillRootRect;

    void Awake()
    {
        skillRootRect = GetComponent<RectTransform>();
    }

    public void RedrawLines()
    {
        foreach (var line in activeLines)
            Destroy(line.gameObject);
        activeLines.Clear();

        foreach (var kvp in tree.Nodes)
        {
            var fromNode = kvp.Value;
            var fromRT = fromNode.transform as RectTransform;


            // prerequisites
            for (int i = 0; i < 4; i++)
            {
                var prereq = fromNode.Definition.prerequisiteSkills[i];
                if (prereq == null) continue;
                if (!tree.TryGetNode(prereq, out var toNode)) continue;

                CreateLine(fromRT, toNode.transform as RectTransform);
            }
        }
    }

    // SkillTreeLineDrawer.cs - 오프셋 보정 테스트 코드

    private void CreateLine(RectTransform from, RectTransform to)
    {
        var line = Instantiate(linePrefab, content);
        activeLines.Add(line);

        // 1. 노드의 World Position을 Content의 Local Position으로 변환
        Vector2 p1 = content.InverseTransformPoint(from.position);
        Vector2 p2 = content.InverseTransformPoint(to.position);

        // 노드 크기의 절반 (50, 50)을 Content Local 좌표에 더합니다.
        // 이는 Content의 Pivot (0,0)과 SkillRoot의 Pivot (0.5, 0.5) 간의 
        // 좌표계 기준점 차이를 보정하는 역할을 시도합니다.
        // 만약 노드가 100x100이고 SkillRoot가 중앙 앵커라면 (50, 50)만큼 이동해야 합니다.
        p1 += new Vector2(50f, 50f);
        p2 += new Vector2(50f, 50f);

        // 2. 선의 방향 및 길이
        Vector2 dir = p2 - p1;
        float length = dir.magnitude;

        RectTransform rt = line.rectTransform;

        // 3. 라인의 앵커와 피벗을 왼쪽 시작점 기준으로 강제 설정 (유지)
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.zero;
        rt.pivot = new Vector2(0f, 0.5f);

        // 4. 라인의 시작점 설정
        rt.anchoredPosition = p1;
        rt.sizeDelta = new Vector2(length, 4f);

        // 5. 회전 계산
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        rt.localRotation = Quaternion.Euler(0, 0, angle);
    }
}
