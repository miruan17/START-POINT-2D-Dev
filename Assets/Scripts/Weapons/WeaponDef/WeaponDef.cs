using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/WeaponDef")]
public class WeaponDef : ScriptableObject
{
    [Header("Identity")]
    public string id;
    public string displayName;

    [Header("icon / Prefab")]
    public Sprite icon;
    public GameObject equippedPrefab;

    [Header("Damaage Multipler")]
    public float DamageMultipler = 1.0f;

    [Header("AttackDef")]
    public AttackDef EnhencedAttack = new();
    public List<AttackDef> CombodAttacks = new();

    [Header("Input Cooldown")]
    public float globalCooldown = 0.1f;

    [TextArea]
    public string Notes;
}
