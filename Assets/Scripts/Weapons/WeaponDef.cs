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

    [Header("AttackDef")]
    public AttackDef EnhancedAttack = null;
    public List<AttackDef> ComboAttacks = new();
    public float ComboDeadline = 0.5f;

    [TextArea]
    public string Notes;
}
