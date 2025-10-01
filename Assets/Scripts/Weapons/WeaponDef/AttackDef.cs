using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/AttackDef")]
public class AttackDef : ScriptableObject
{
    [Header("Delay")]
    public float preDelay;
    public float HitTime;
    public float postDelay;

    [Header("Damage")]
    public float Damage;

    [Header("Hitbox")]
    public GameObject hitbox;
}
