using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public static List<Enemy> AllEnemies = new List<Enemy>();
    public override void DeathTrigger()
    {
        if (status.CurrentHP <= 0)
        {
            status.CurrentHP = 0;
            Debug.Log(this.name + "Dead");
            gameObject.SetActive(false);
        }
    }
    private void OnEnable()
    {
        AllEnemies.Add(this);
    }

    private void OnDisable()
    {
        AllEnemies.Remove(this);
    }
}
