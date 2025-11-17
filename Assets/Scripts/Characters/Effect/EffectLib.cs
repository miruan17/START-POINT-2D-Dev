using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;

public sealed class EffectLib
{
    public Dictionary<string, Effect> effectMap = new Dictionary<string, Effect>();
    private static readonly EffectLib effectLib = new EffectLib();
    static EffectLib() { }
    public EffectLib()
    {
        effectMap["Bleeding"] = new Effect_Bleeding(5f, 1f, 1f, 10);
        effectMap["Poison"] = new Effect_Poison(7f, 2f, 1f, 5);
    }
    public static EffectLib Instance
    {
        get
        {
            return effectLib;
        }
    }
    public Effect getEffectbyID(String identifier)
    {
        if (effectMap.TryGetValue(identifier, out var effect))
            return effect;

        return null;
    }
}
