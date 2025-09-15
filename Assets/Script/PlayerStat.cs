using System.Collections;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public  static PlayerStat Instance;

    public int character_Lv = 1;
    // public int[] needExp;
    public int currentEXP;

    public int maxHP = 100;
    public int currentHP;

    public int atk = 10;
    public int speed = 10;
    public int atkSpeed = 10;

    public int maxSP = 4;
    public int currentSP;
    public float spRegenRate = 3f; // sp ȸ���ӵ�
    public int spRegenAmount = 1;



    void Start()
    {
        Instance = this;
        currentSP = maxSP;
        Debug.Log($"Start SP: {currentSP}/{maxSP}");
        StartCoroutine(ManaRegenCoroutine());
    }


    IEnumerator ManaRegenCoroutine()
    {
        while (true)
        {
            Debug.Log($"Waiting for {spRegenRate} seconds...");
            yield return new WaitForSeconds(spRegenRate);

            if (currentSP < maxSP)
            {
                currentSP += spRegenAmount;

                if (currentSP > maxSP)
                    currentSP = maxSP;

                Debug.Log($"SP: {currentSP}/{maxSP}");
            }
            else
            {
                Debug.Log("SP is full, skipping...");
            }
        }
    }

}
