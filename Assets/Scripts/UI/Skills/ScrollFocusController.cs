using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollFocusController : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform content;
    [SerializeField] private RectTransform viewport;

    private Vector2? targetPos = null;
    private const float moveSpeed = 8f;
    private bool isDragging = false;

    public void OnBeginDrag(PointerEventData eventData) => isDragging = true;
    public void OnEndDrag(PointerEventData eventData) => isDragging = false;

    /// <summary>
    /// 포커싱 대상 Transform을 Viewport 중앙으로 오도록 스크롤 이동
    /// </summary>
    public void FocusOn(Transform target)
    {
        if (target == null || scrollRect == null || content == null || viewport == null)
            return;

        targetPos = CalculateNormalizedPosition(target);
    }

    private void LateUpdate()
    {
        if (!targetPos.HasValue || isDragging)
            return;

        Vector2 current = scrollRect.normalizedPosition;
        Vector2 target = targetPos.Value;

        Vector2 newPos = Vector2.Lerp(current, target, Time.deltaTime * moveSpeed);
        scrollRect.normalizedPosition = newPos;

        if (Vector2.Distance(current, target) < 0.001f)
            targetPos = null;
    }

    /// <summary>
    /// Content 기준으로 대상 위치를 Viewport 중앙에 맞추기 위한 normalizedPosition 계산
    /// </summary>
    private Vector2 CalculateNormalizedPosition(Transform target)
    {
        RectTransform targetRect = target as RectTransform;
        if (targetRect == null) return scrollRect.normalizedPosition;

        // ✅ Content 내부 좌표로 변환
        Vector2 targetLocal = content.InverseTransformPoint(targetRect.position);
        Vector2 contentSize = content.rect.size;
        Vector2 viewportSize = viewport.rect.size;

        // Viewport 중앙에 맞추기 위한 보정
        float normalizedX = Mathf.Clamp01(
            (targetLocal.x - viewportSize.x * 0.5f) / (contentSize.x - viewportSize.x)
        );

        // Y축은 Pivot(0,1) 기준 반전
        float normalizedY = Mathf.Clamp01(
            1f - ((-targetLocal.y - viewportSize.y * 0.5f) / (contentSize.y - viewportSize.y))
        );

        Debug.Log($"[ScrollFocus] TargetLocal={targetLocal}, Normalized=({normalizedX:F2}, {normalizedY:F2})");
        return new Vector2(normalizedX, normalizedY);
    }
}
