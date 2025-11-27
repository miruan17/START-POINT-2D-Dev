using System.Collections.Generic;
using UnityEngine;

public static class ApplyEffect
{
    public static void applyEffect(Character to, List<Effect> list)
    {
        EffectManager toManager = to.getEffect();
        foreach (var effect in list)
        {
            if (!Probability.Attempt(effect.chance)) continue;
            Effect getter = toManager.SearchEffectbyId(effect.identifier);

            if (getter != null)
            {
                getter.Refresh(effect);
            }
            else
            {
                getter = effect.copy();
                toManager.AddEffect(getter);
                getter.Refresh(effect);
            }
        }
    }
}