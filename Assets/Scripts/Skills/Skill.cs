using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    protected EffectManager effectManager;
    private void Awake()
    {
        effectManager = new EffectManager();
    }
    public EffectManager getEffect()
    {
        return effectManager;
    }
}