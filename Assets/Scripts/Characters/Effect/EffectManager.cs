using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;

public class EffectManager  //Manager class
{
    // Effect Manager
    private List<Effect> effectList;
    public EffectManager()
    {
        effectList = new List<Effect>();
    }

    // Effect 추가
    public void AddEffect(String identifier, Effect effect, float term, params float[]? argv)
    {
        effect.manager = this;
        effect.identifier = identifier;
        effect.term = term;
        effectList.Add(effect);
        effectList.Sort((a, b) => a.identifier.CompareTo(b.identifier));
    }
    public void AddEffect(Effect effect)
    {
        effectList.Add(effect);
        effectList.Sort((a, b) => a.identifier.CompareTo(b.identifier));
    }

    public int SearchNumEffect(String identifier)
    {
        int cnt = 0;
        foreach (Effect effect in effectList)
        {
            if (effect.identifier.Equals(identifier)) cnt++;
        }
        return cnt;
    }

    public void RemoveEffect(Effect effect)
    {
        effectList.Remove(effect);
    }

    public List<Effect> ReturnEffect()
    {
        return effectList;
    }

    public void RuntimeEffect()
    {
        foreach (Effect effect in effectList)
        {
            effect.Runtime();
        }
    }
}