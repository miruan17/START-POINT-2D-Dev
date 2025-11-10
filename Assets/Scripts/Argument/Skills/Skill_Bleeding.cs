public class Skill_Bleeding : ArgumentBase
{
    public void setActiveBleeding()
    {
        effectManager.AddEffect("Bleeding", new Effect_Bleeding(), 5f);
    }
}