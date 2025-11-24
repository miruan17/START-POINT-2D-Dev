using Unity.VisualScripting;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    public SkillNodeDef[] activeSkill = new SkillNodeDef[4];
    private PlayerInputHub input;
    private Player player;
    private void Awake()
    {
        input = GetComponent<PlayerInputHub>();
        player = GetComponentInParent<Player>();
    }
    private void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            if (activeSkill[i] != null && input.SkillRequest(i))
            {
                Skill skill = SkillLib.Instance.getSkillbyID(activeSkill[i].skillName);
                if (skill.skillType == SkillType.Summon)
                {
                    Instantiate(activeSkill[i].summonSkill, player.transform.position, player.transform.rotation);
                }
            }
        }
    }
    public void UpdateActiveSkill(int idx, SkillNodeDef def)
    {
        activeSkill[idx] = def;
    }
}