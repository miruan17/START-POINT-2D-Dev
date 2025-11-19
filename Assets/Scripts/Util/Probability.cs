public static class Probability
{
    public static bool Attempt(float chance)
    {
        return (UnityEngine.Random.value <= chance);
    }
}