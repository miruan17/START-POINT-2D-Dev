using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HPBarController : MonoBehaviour
{
    public Image hpImage;   // Fill Amount 조절할 이미지
    public Player player;
    private float currentHp;
    private float maxHp;
    void Start()
    {
        maxHp = player.status.GetFinal(StatId.HP);
        currentHp = maxHp;
        UpdateHPBar();
    }

    void Update()
    {
        maxHp = player.status.GetFinal(StatId.HP);
        currentHp = player.status.CurrentHP;
        hpImage.fillAmount = currentHp / maxHp;
    }

    void UpdateHPBar()
    {
        // 현재 체력 비율 계산해서 fillAmount에 반영
        hpImage.fillAmount = currentHp / maxHp;
    }
}