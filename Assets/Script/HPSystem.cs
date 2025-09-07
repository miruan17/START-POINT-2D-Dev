using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HPBarController : MonoBehaviour
{
    public Image hpImage;   // Fill Amount 조절할 이미지
    public TextMeshProUGUI hpText;

    void Start()
    {
        PlayerStat.Instance.currentHP = PlayerStat.Instance.maxHP;
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
        PlayerStat.Instance.currentHP -= damage;
        PlayerStat.Instance.currentHP = Mathf.Clamp(PlayerStat.Instance.currentHP, 0, PlayerStat.Instance.maxHP); // HP 범위 제한
        UpdateHPBar();
    }

    public void TakeHeal(int healling)
    {
        PlayerStat.Instance.currentHP += healling;
        PlayerStat.Instance.currentHP = Mathf.Clamp(PlayerStat.Instance.currentHP, 0, PlayerStat.Instance.maxHP); // HP 범위 제한
        Debug.Log("회복된 hp : " + healling);
        UpdateHPBar();
    }

    void UpdateHPBar()
    {
        // 현재 체력 비율 계산해서 fillAmount에 반영
        hpImage.fillAmount = (float)PlayerStat.Instance.currentHP / PlayerStat.Instance.maxHP;
        hpText.text = PlayerStat.Instance.currentHP + "/100";
    }
}
