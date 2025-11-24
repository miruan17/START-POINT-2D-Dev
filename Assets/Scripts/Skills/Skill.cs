using UnityEngine;
public enum SkillType { Summon, Attack, Move }
public abstract class Skill : MonoBehaviour
{
    protected EffectManager effectManager;
    public SkillType skillType;
    protected virtual void Awake()
    {
        effectManager = new EffectManager();
    }
    public EffectManager getEffect()
    {
        return effectManager;
    }
}