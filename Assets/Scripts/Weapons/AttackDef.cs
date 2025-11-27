using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/AttackDef")]
public class AttackDef : ScriptableObject
{
    [Header("Basic")]
    public string attackId;
    public float useSP;

    [Header("Hitbox")]
    public GameObject Hitbox;
    public float speed = 0f;

    [Tooltip("Delay")]
    [Min(0f)] public float preDelay = 0.1f;
    [Min(0f)] public float hitTime = 0.1f;
    [Min(0f)] public float postDelay = 0.1f;

    [Header("Combat")]
    [Min(0f)] public float DamageScale = 1.0f;
    public Vector2 knockback = new Vector2(0f, 0f);
    public LayerMask targetMask;

    [Header("Visuals / SFX")]
    public GameObject spawnVFX;
    public GameObject hitVFX;
    public AudioClip spawnSFX;
    public AudioClip hitSFX;

    [Header("Misc")]
    public bool flipOffsetWithFacing = true;
}