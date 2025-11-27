using UnityEngine;
public enum SkillType { Summon, Attack, Move }
public abstract class Skill : MonoBehaviour
{
    protected EffectManager effectManager;
    public SkillType skillType;
    public float dmg = 0;
    protected virtual void Awake()
    {
        effectManager = new EffectManager();
    }
    public EffectManager getEffect()
    {
        return effectManager;
    }
}