using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LVUISystem : MonoBehaviour
{
    public TMP_Text lv;

    public void Start()
    {
        lv.text = "LV.0";
    }

    public void Update()
    {
        if (PlayerStat.Instance.character_Lv > 10) PlayerStat.Instance.character_Lv = 10;
        if (Input.GetKeyDown(KeyCode.L))
        {
            PlayerStat.Instance.character_Lv++;
        }

        UpdateLv(PlayerStat.Instance.character_Lv);
    }

    public void UpdateLv(int PlayerLv) {
        lv.text = "LV." + PlayerLv.ToString();
    }

}


