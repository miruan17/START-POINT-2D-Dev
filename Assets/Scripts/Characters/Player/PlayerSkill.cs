using Unity.VisualScripting;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    public SkillNodeDef[] activeSkill = new SkillNodeDef[4];
    private PlayerInputHub input;
    private Player player;
    private Animator anim;
    private void Awake()
    {
        input = GetComponent<PlayerInputHub>();
        player = GetComponentInParent<Player>();
        anim = GetComponentInChildren<Animator>();
    }
    private void Update()
    {
        if (anim.GetBool("ActionLock")) return;
        for (int i = 0; i < 4; i++)
        {
            if (activeSkill[i] != null && input.SkillRequest(i))
            {
                Skill skill = SkillLib.Instance.getSkillbyID(activeSkill[i].skillName);
                if (skill.skillType == SkillType.Summon)
                {
                    Instantiate(activeSkill[i].summonSkill, player.transform.position, player.transform.rotation);
                    //ActionLockSystem.Instance.LockOnTime(0.5f, anim, input);
                }
                else if (skill.skillType == SkillType.Attack)
                {
                    Instantiate(activeSkill[i].summonSkill, player.transform.position, player.transform.rotation);
                    //ActionLockSystem.Instance.LockOnTime(0.5f, anim, input);
                }
                else if (skill.skillType == SkillType.Move)
                {
                    //ActionLockSystem.Instance.LockOnTime(0.3f, anim, input);
                }
            }
        }
    }
    public void UpdateActiveSkill(int idx, SkillNodeDef def)
    {
        activeSkill[idx] = def;
    }
}