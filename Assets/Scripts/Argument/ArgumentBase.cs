using UnityEngine;

public abstract class ArgumentBase : ScriptableObject
{
    public Effect effect;
    public abstract void setActive();
}