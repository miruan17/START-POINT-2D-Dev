public interface ISkillPointProvider
{
    int GetAvailable();
    bool TrySpend(int cost);
    void AddPoints(int amount);
}
