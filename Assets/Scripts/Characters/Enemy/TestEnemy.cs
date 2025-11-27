using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : Enemy
{
    [Header("AttackPattern(WeaponDef)")]
    [SerializeField]
    public WeaponDef attack;
    private List<GameObject> hitboxes = new();



}
