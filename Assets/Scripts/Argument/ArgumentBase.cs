public abstract class ArgumentBase
{
    public EffectManager effectManager { get; protected set; }

    public ArgumentBase()
    {
        effectManager = new EffectManager();
    }
}