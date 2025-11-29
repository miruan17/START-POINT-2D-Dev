using UnityEngine;

[CreateAssetMenu(menuName = "Character/CharacterStatus")]
public class CharacterStatusDef : ScriptableObject
{
    [Header("HP(Health Point)")]
    public float HP;
    [Header("SP(Stamina Point)")]
    public float SP;
    [Header("ATK(Attack)")]
    public float ATK;
    [Header("APS(Attack Per Second)")]
    public float APS;
    [Header("DEF(Defend)")]
    public float DEF;
    [Header("SPD(Speed)")]
    public float SPD;
    [Header("Jump(JP)")]
    public float JP;
}
