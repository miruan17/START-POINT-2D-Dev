using System;
using UnityEngine;
using UnityEngine.UI;

public class ScrollFocusController : MonoBehaviour
{
    [Header("Scroll Components")]
    [SerializeField] private ScrollRect scrollRect;      // Scroll View
    [SerializeField] private RectTransform viewport;     // Viewport (고정)
    [SerializeField] private RectTransform content;      // Content (이동 대상)

    // 현재 포커싱 중인 node를 화면 가운데로 놓는 작업
    public void FocusOn(Transform target, String name)
    {
        if (target == null || content == null || viewport == null)
            return;

        scrollRect.StopMovement();

        // Content 기준의 타겟 로컬 좌표
        Vector2 targetLocal = content.InverseTransformPoint(target.position);

        // Viewport 중앙 좌표 (RootNode일 시 높이 조정)
        Vector2 viewportCenter = new Vector2(
            viewport.rect.width / 2f,
            -viewport.rect.height / (name.Equals("Root") ? 1.5f : 2f)
        );

        // 이동할 목표 좌표 계산
        Vector2 desiredPos = viewportCenter - targetLocal;
        content.anchoredPosition = desiredPos;
    }
}
