using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Unity.Collections;
using UnityEngine;

public sealed class EffectLib
{
    public Dictionary<string, Effect> effectMap = new Dictionary<string, Effect>();
    public static readonly EffectLib playerEffectLib = new EffectLib();
    private static readonly EffectLib effectLib = new EffectLib();
    static EffectLib() { }
    public EffectLib()
    {
        effectMap["Bleeding"] = new Effect_Bleeding(5f, 1f, 1f, 10);
        effectMap["Poison"] = new Effect_Poison(7f, 2f, 1f, 5);
        effectMap["Ice"] = new Effect_Ice(10f, 10f, 3);
        effectMap["Fire"] = new Effect_Fire(10f, 3f, 1f);
        effectMap["Lightning"] = new Effect_Lightning(2, 3, 10);
        effectMap["Freeze"] = new Effect_Freeze(3, 5);
        foreach (var (x, y) in effectMap)
        {
            y.identifier = x;
        }
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
