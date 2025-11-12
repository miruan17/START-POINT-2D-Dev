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
        effect.BindingManager(this);
        effect.identifier = identifier;
        effect.term = term;
        effectList.Add(effect);
        effectList.Sort((a, b) => a.identifier.CompareTo(b.identifier));
    }
    public void AddEffect(Effect effect)
    {
        effect.toString();
        effectList.Add(effect);
        effect.BindingManager(this);
        effectList.Sort((a, b) => a.identifier.CompareTo(b.identifier));
    }
    public void setEffectList(List<Effect> list)
    {
        effectList = list;
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
        effect.DisposeEffect();
    }

    public List<Effect> ReturnEffect()
    {
        return effectList;
    }

    public void RuntimeEffect()
    {
        // effect 실행 및 지속시간이 끝난 effect 제거
        List<Effect> deleteList = new();
        foreach (Effect effect in effectList)
        {
            if (!effect.IsExpired) effect.Runtime();
            else deleteList.Add(effect);
        }
        foreach (Effect effect in deleteList)
        {
            RemoveEffect(effect);
        }
    }
}