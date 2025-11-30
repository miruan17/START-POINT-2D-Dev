using System.Collections;
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

                if (activeSkill[i].summonSkill != null)
                {
                    var obj = Instantiate(activeSkill[i].summonSkill, player.transform.position, player.transform.rotation);
                    Vector3 scale = obj.transform.localScale;
                    scale.x = Mathf.Abs(scale.x) * (input.facingRight ? 1f : -1f);
                    obj.transform.localScale = scale;
                    if (skill.skillType == SkillType.Attack)
                        obj.transform.SetParent(player.transform);
                }
                if (skill.skillType == SkillType.Move)
                {
                    if (activeSkill[i].skillName == "Dash")
                    {
                        StartCoroutine(Dash());
                    }
                    if (activeSkill[i].skillName == "FastDrop")
                    {
                        if (anim.GetBool("Jump"))   // 공중에서만 가능
                            StartCoroutine(FastDrop());
                    }
                }
            }
        }
    }
    public void UpdateActiveSkill(int idx, SkillNodeDef def)
    {
        activeSkill[idx] = def;
    }

    private IEnumerator Dash()
    {
        float dashDuration = 0.2f;
        float dashSpeed = 40f;

        ActionLockSystem.Instance.LockOnTime(dashDuration, anim, input);

        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        float dir = input.facingRight ? 1f : -1f;
        Debug.Log(input.flip);
        rigid.velocity = new Vector2(dir * dashSpeed, 0f);

        float start = Time.time;
        while (Time.time < start + dashDuration)
        {
            rigid.velocity = new Vector2(dir * dashSpeed, 0f);
            yield return null;
        }
        rigid.velocity = new Vector2(0f, 0f);
    }

    private IEnumerator FastDrop()
    {
        float downwardSpeed = -40f;     // 빠른 하강 속도
        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        PlayerMove move = GetComponent<PlayerMove>();

        // ActionLock과 입력 차단
        ActionLockSystem.Instance.LockOnManual(anim, input);

        while (!move.isGrounded)
        {
            // x축을 0으로 고정하고 y축만 강제 하강
            rigid.velocity = new Vector2(0f, downwardSpeed);

            yield return null;
        }

        // Grounded → unlock
        ActionLockSystem.Instance.LockOffManual(anim, input);
    }

}