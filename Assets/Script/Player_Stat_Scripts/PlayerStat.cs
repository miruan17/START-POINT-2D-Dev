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



    void Start()
    {
        Instance = this;
    }

}
