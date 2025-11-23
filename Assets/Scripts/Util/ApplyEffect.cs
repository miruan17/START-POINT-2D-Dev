public static class ApplyEffect
{
    public static void applyEffect(Character to, EffectManager manager)
    {
        EffectManager toManager = to.getEffect();
        foreach (var effect in manager.ReturnEffect())
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
                getter.Refresh(effect);
                toManager.AddEffect(getter);
            }
        }
    }
}