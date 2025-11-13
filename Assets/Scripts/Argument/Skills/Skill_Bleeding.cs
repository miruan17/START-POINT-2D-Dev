using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Bleeding")]
public class Skill_Bleeding : ArgumentBase
{
    public override void setActive()
    {
        effect = new Effect_Bleeding();
        effect.identifier = "Bleeding";
        effect.term = 5f;
        effect.can_stack = true;
        effect.max_stack = 11;
    }
}