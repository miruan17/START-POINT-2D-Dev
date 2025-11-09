using UnityEngine;

[CreateAssetMenu(menuName = "Character/Status")]
public class StatusDef : ScriptableObject
{
    [Header("HP(Health Point)")]
    public float DefaultHp;
    [Header("SP(Stamina Point)")]
    public float DefaultSp;
    [Header("ATK(Attack)")]
    public float DefaultAtk;
    [Header("APS(Attack Per Second)")]
    public float DefaultAps;
    [Header("DEF(Defend)")]
    public float DefaultDef;
    [Header("SPD(Speed)")]
    public float DefaultSpd;
}
