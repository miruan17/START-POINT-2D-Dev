using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class FlexibleGridLayout : LayoutGroup
{
    [Range(1, 10)] public int columns = 6;          // 가로 열 개수
    [SerializeField] private float topPadding = 20f;
    [SerializeField] private float sidePadding = 20f;
    [SerializeField] private float sizeRatio = 0.8f; // 셀 비율 (1=꽉참, 0.8=공간 여유)
    [SerializeField] private float aspectRatio = 1f; // 가로:세로 비율 (1이면 정사각형)
    [SerializeField] private float verticalSpacingRatio = 0.3f; // 세로 간격 비율

    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();
        UpdateLayout();
    }

    public override void CalculateLayoutInputVertical() => UpdateLayout();
    public override void SetLayoutHorizontal() => UpdateLayout();
    public override void SetLayoutVertical() => UpdateLayout();

    private void UpdateLayout()
    {
        if (rectChildren.Count == 0 || rectTransform.rect.width <= 0)
            return;

        float totalWidth = rectTransform.rect.width - sidePadding * 2f;
        float cellFullWidth = totalWidth / columns; // 각 칸 전체 영역
        float cellWidth = cellFullWidth * sizeRatio;
        float cellHeight = cellWidth / aspectRatio;
        float spacingX = (cellFullWidth - cellWidth); // 남은 공간의 절반이 양쪽 여백으로 감

        int rowCount = Mathf.CeilToInt(rectChildren.Count / (float)columns);
        float spacingY = cellHeight * verticalSpacingRatio;

        // 컨텐츠 높이 계산
        float totalHeight = topPadding + rowCount * (cellHeight + spacingY);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, totalHeight);

        // 배치 시작 좌표 (왼쪽 상단 기준)
        float startX = sidePadding + spacingX / 2f;
        float startY = -topPadding; // 첫 행은 위쪽 여백만큼 아래에서 시작

        for (int i = 0; i < rectChildren.Count; i++)
        {
            int row = i / columns;
            int col = i % columns;

            RectTransform child = rectChildren[i];
            float xPos = startX + col * (cellWidth + spacingX);
            float yPos = startY + row * (cellHeight + spacingY); // 아래로 쌓이도록 수정됨

            SetChildAlongAxis(child, 0, xPos, cellWidth);
            SetChildAlongAxis(child, 1, yPos, cellHeight);
        }

    }
}
