using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HeartDisplay : MonoBehaviour
{
    public Image[] sp; 
    public Sprite fullsp;
    public Sprite emptysp;


    private Coroutine[] shakeCoroutines;

    private Vector3[] originalPositions;

    void Start()
    {
        shakeCoroutines = new Coroutine[sp.Length];
        originalPositions = new Vector3[sp.Length];

        // 하트 원위치 저장
        for (int i = 0; i < sp.Length; i++)
        {
            originalPositions[i] = sp[i].rectTransform.localPosition;
        }

        UpdateHearts();
    }

    void Update()
    {
        UpdateHearts();
        if (Input.GetKeyDown(KeyCode.D))
        {
            TakeDamage(1);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            Heal(1);
        }


    }

    public void TakeDamage(int damage)
    {
        int previousSP = PlayerStat.Instance.currentSP;
        PlayerStat.Instance.currentSP -= damage;
        if (PlayerStat.Instance.currentSP < 0) PlayerStat.Instance.currentSP = 0;

        for (int i = PlayerStat.Instance.currentSP; i < Mathf.Min(previousSP, sp.Length); i++)
        {
            if (shakeCoroutines[i] != null)
            {
                StopCoroutine(shakeCoroutines[i]);
                sp[i].rectTransform.localPosition = originalPositions[i];
            }
            shakeCoroutines[i] = StartCoroutine(Shake(sp[i], originalPositions[i]));
        }

        for (int i = 0; i < PlayerStat.Instance.currentSP && i < sp.Length; i++)
        {
            if (shakeCoroutines[i] != null)
            {
                StopCoroutine(shakeCoroutines[i]);
                sp[i].rectTransform.localPosition = originalPositions[i];
                shakeCoroutines[i] = null;
            }
        }

        UpdateHearts();
    }

    public void Heal(int amount)
    {
        PlayerStat.Instance.currentSP += amount;
        if (PlayerStat.Instance.currentSP > PlayerStat.Instance.maxSP) PlayerStat.Instance.currentSP = PlayerStat.Instance.maxSP;

        for (int i = 0; i < sp.Length; i++)
        {
            if (shakeCoroutines[i] != null)
            {
                StopCoroutine(shakeCoroutines[i]);
                sp[i].rectTransform.localPosition = originalPositions[i];
                shakeCoroutines[i] = null;
            }
        }

        UpdateHearts();
    }

    void UpdateHearts()
    {
        for (int i = 0; i < sp.Length; i++)
        {
            if (i < PlayerStat.Instance.currentSP)
            {
                sp[i].sprite = fullsp;
                sp[i].enabled = true;
            }
            else
            {
                sp[i].sprite = emptysp;
                sp[i].enabled = true;
            }
        }
    }

    IEnumerator Shake(Image image, Vector3 originalPos, float duration = 0.2f, float magnitude = 5f)
    {
        RectTransform rt = image.rectTransform;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;

            rt.localPosition = originalPos + new Vector3(offsetX, offsetY, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        rt.localPosition = originalPos;
    }
}
