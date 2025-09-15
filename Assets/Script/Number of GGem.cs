using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextChanger : MonoBehaviour
{
    public TMP_Text ggem;
    public int ggem_number = 0;

    void Start()
    {
        ggem.text = "0";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            ggem_number += 100;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            ggem_number--;
        }

        if (ggem_number < 0) ggem_number = 0;

        // 변경된 값을 텍스트에 반영
        ggem.text = ggem_number.ToString();
        // 또는 ChangeText(ggem_number);

        ChangeText(ggem_number);
    }

    public void ChangeText(int ggem_number)
    {
        if (ggem_number >= 1000)
        {
            int a = ggem_number % 1000;
            ggem.text = (ggem_number / 1000).ToString() + "."+ (a/100).ToString() + "K";
        }
        else
        {
            ggem.text = ggem_number.ToString();    
        }
        
    }
}
