#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;

//[ExecuteAlways]
public class FlexibleGridLayout : GridLayoutGroup
{
    [SerializeField][Range(1, 100)] public int columns = 6;   // 한 줄당 열 개수
    [SerializeField] public float topPadding = 20f;
    [SerializeField] public float sidePadding = 20f;
    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();

        // 현재 Content의 가로 폭
        float totalWidth = rectTransform.rect.width - (sidePadding * 2);
        constraintCount = columns;
        // 셀 너비 계산 (균등 배치)
        float cellWidth = (totalWidth / columns);
        cellSize = new Vector2(cellWidth, cellSize.y);

        // Spacing 자동 계산: 남은 공간을 각 셀 사이에 균등 분배
        float usedWidth = cellWidth * columns;
        float totalSpacing = Mathf.Max(0, totalWidth - usedWidth);
        float spacingValue = totalSpacing / (columns + 1);

        spacing = new Vector2(spacingValue, spacing.y);

        // Padding 반영
        padding.left = (int)sidePadding;
        padding.right = (int)sidePadding;
        padding.top = (int)topPadding;
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(FlexibleGridLayout))]
public class FlexibleGridLayoutEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var layout = (FlexibleGridLayout)target;

        EditorGUILayout.LabelField("Flexible Grid Layout", EditorStyles.boldLabel);
        layout.columns = EditorGUILayout.IntSlider("Columns", layout.columns, 1, 100);
        layout.topPadding = EditorGUILayout.FloatField("Top Padding", layout.topPadding);
        layout.sidePadding = EditorGUILayout.FloatField("Side Padding", layout.sidePadding);

        if (GUI.changed)
            EditorUtility.SetDirty(layout);
    }
}
#endif