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
    public void AddEffect(String identifier, Effect effect, float term, params float[] argv)
    {

        effectList.Add(effect);
        effectList.Sort();
    }

    public void SearchEffect(String identifier)
    {
        
    }
    
    public void RemoveEffect(String identifier)
    {
        
    }

    public List<Effect> ReturnEffect()
    {
        return effectList;
    }

    public void RuntimeEffect()
    {
        
    }
}