using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;

public class EffectManager  //Manager class
{
    // Effect Manager
    private List<Effect> effectList;
    private Character character;
    public EffectManager(Character ch)
    {
        effectList = new List<Effect>();
        character = ch;
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
        effectList.Add(effect);
        effect.BindingManager(this);
        effectList.Sort((a, b) => a.identifier.CompareTo(b.identifier));
    }
    public void setEffectList(List<Effect> list)
    {
        effectList = list;
    }
    public Effect SearchEffectbyId(String identifier)
    {
        foreach (Effect effect in effectList)
        {
            if (effect.identifier.Equals(identifier)) return effect;
        }
        return null;
    }
    public void RemoveEffect(Effect effect)
    {
        effectList.Remove(effect);
        effect.stack = 0;
    }

    public List<Effect> ReturnEffect()
    {
        return effectList;
    }
    public Character GetCharacter()
    {
        return character;
    }
    public void RuntimeEffect()
    {
        // effect 실행 및 지속시간이 끝난 effect 제거
        List<Effect> deleteList = new();
        List<Effect> updateList = new();
        foreach (Effect effect in effectList)
        {
            if (!effect.IsExpired && effect.stack > 0) effect.Runtime();
            else deleteList.Add(effect);
        }
        foreach (Effect effect in deleteList)
        {
            RemoveEffect(effect);
        }
    }
}