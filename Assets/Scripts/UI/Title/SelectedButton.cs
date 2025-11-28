using UnityEngine;
using UnityEngine.EventSystems;

public class SelectedButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Scale Settings")]
    [SerializeField] private Vector3 normalScale = Vector3.one;
    [SerializeField] private Vector3 selectedScale = new Vector3(1.15f, 1.15f, 1f);
    [SerializeField] private float animDuration = 0.08f;

    private Coroutine scaleRoutine;

    private void Awake()
    {
        if (target == null)
        {
            if (transform.childCount > 0)
                target = transform.GetChild(0) as RectTransform;
        }

        if (target != null)
        {
            normalScale = target.localScale;
        }
        else
        {
            Debug.LogWarning($"{name}: Null Target");
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (target == null) return;
        StartScaleAnimation(selectedScale);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (target == null) return;
        StartScaleAnimation(normalScale);
    }
    
    private void StartScaleAnimation(Vector3 targetScale)
    {
        if (scaleRoutine != null)
            StopCoroutine(scaleRoutine);

        scaleRoutine = StartCoroutine(ScaleRoutine(targetScale));
    }

    private System.Collections.IEnumerator ScaleRoutine(Vector3 targetScale)
    {
        Vector3 startScale = target.localScale;
        float t = 0f;

        while (t < animDuration)
        {
            t += Time.unscaledDeltaTime;
            float progress = Mathf.Clamp01(t / animDuration);
            target.localScale = Vector3.Lerp(startScale, targetScale, progress);
            yield return null;
        }

        target.localScale = targetScale;
    }
}
