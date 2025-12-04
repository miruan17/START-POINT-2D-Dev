using TMPro;
using UnityEngine;

public class UI_Level : MonoBehaviour
{
    private TextMeshProUGUI text;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void FixedUpdate()
    {
        text.text = GameManager.Instance.getPlayer().GetLevel().ToString();
    }
}
