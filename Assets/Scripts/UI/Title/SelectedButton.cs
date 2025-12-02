using UnityEngine;
using UnityEngine.EventSystems;

public class SelectedButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [Header("Text")]
    [SerializeField] private Transform text;

    [Header("Sprite")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite selectedButton;
    [SerializeField] private Sprite unselectedButton;

    [Header("Scale Settings")]
    [SerializeField] private Vector3 normalScale = Vector3.one;
    [SerializeField] private Vector3 selectedScale = new Vector3(1.15f, 1.15f, 1f);
    [SerializeField] private float animDuration = 0.08f;

    [Header("")] 
    private Coroutine scaleRoutine;

    private void Awake()
    {
        if (text == null)
        {
            if (transform.childCount > 0)
                text = transform.GetChild(0) as RectTransform;
        }

        if (text != null)
        {
            normalScale = text.localScale;
        }
        else
        {
            Debug.LogWarning($"{name}: Null Target");
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (text == null) return;
        StartScaleAnimation(selectedScale);
        spriteRenderer.sprite = selectedButton;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (text == null) return;
        StartScaleAnimation(normalScale);
        spriteRenderer.sprite = unselectedButton;
    }
    
    private void StartScaleAnimation(Vector3 targetScale)
    {
        if (scaleRoutine != null)
            StopCoroutine(scaleRoutine);

        scaleRoutine = StartCoroutine(ScaleRoutine(targetScale));
    }

    private System.Collections.IEnumerator ScaleRoutine(Vector3 targetScale)
    {
        Vector3 startScale = text.localScale;
        float t = 0f;

        while (t < animDuration)
        {
            t += Time.unscaledDeltaTime;
            float progress = Mathf.Clamp01(t / animDuration);
            text.localScale = Vector3.Lerp(startScale, targetScale, progress);
            yield return null;
        }

        text.localScale = targetScale;
    }
}
