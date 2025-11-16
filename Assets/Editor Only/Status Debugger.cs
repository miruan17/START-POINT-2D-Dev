using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

public class StatusDebugger : MonoBehaviour
{
    [Header("GUI Offset")]
    public float PosX = 60f;   // GUI의 X 오프셋
    public float PosY = -20f; // GUI의 Y 오프셋

    private Character character;

    void Awake()
    {
        character = GetComponent<Character>();
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
#if UNITY_EDITOR
        // 표시할 텍스트 여러 줄
        string[] lines =
        {
            $"HP : {character.status.CurrentHP}",
            $"ATK: {character.status.GetFinal(StatId.ATK)}"
        };

        // 월드 위치(오브젝트 중심)
        Vector3 worldPos = transform.position;

        // 월드 → GUI 좌표 변환
        Vector3 guiPos = HandleUtility.WorldToGUIPoint(worldPos);

        // PosX, PosY 오프셋 적용
        guiPos.x += PosX;
        guiPos.y += PosY;

        // 스타일
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.white;
        style.fontSize = 14;

        // 가장 긴 줄 기준으로 배경 크기 계산
        float maxWidth = 0f;
        float lineHeight = style.lineHeight;

        foreach (var line in lines)
        {
            Vector2 size = style.CalcSize(new GUIContent(line));
            if (size.x > maxWidth)
                maxWidth = size.x;
        }

        // 패딩
        float padding = 6f;

        // 배경 박스 위치 및 크기
        Rect rect = new Rect(
            guiPos.x - maxWidth / 2 - padding,
            guiPos.y - (lineHeight * lines.Length) / 2 - padding,
            maxWidth + padding * 2,
            lineHeight * lines.Length + padding * 2
        );

        Handles.BeginGUI();

        // 반투명 검은 배경
        Color prevColor = GUI.color;
        GUI.color = new Color(0, 0, 0, 0.9f);
        GUI.Box(rect, GUIContent.none);
        GUI.color = prevColor;

        // 텍스트 출력
        for (int i = 0; i < lines.Length; i++)
        {
            Rect lineRect = new Rect(
                rect.x + padding,
                rect.y + padding + (i * lineHeight),
                maxWidth,
                lineHeight
            );

            GUI.Label(lineRect, lines[i], style);
        }

        Handles.EndGUI();
#endif
    }
}
