using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HPBarController : MonoBehaviour
{
    public Image hpImage;   // Fill Amount 조절할 이미지

    int currentHp;
    int maxHp = 100;
    void Start()
    {
        currentHp = maxHp;
        UpdateHPBar();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            TakeDamage(5);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeHeal(5);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp); // HP 범위 제한
        UpdateHPBar();
    }

    public void TakeHeal(int healling)
    {
        currentHp += healling;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp); // HP 범위 제한
        Debug.Log("회복된 hp : " + healling);
        UpdateHPBar();
    }

    void UpdateHPBar()
    {
        // 현재 체력 비율 계산해서 fillAmount에 반영
        hpImage.fillAmount = (float)currentHp / maxHp;
    }
}