using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    public SkillNodeDef[] activeSkill = new SkillNodeDef[4];
    private PlayerInputHub input;
    private Player player;
    private Animator anim;
    private Collider2D bodyCol;
    private SpriteRenderer sr;
    private void Awake()
    {
        input = GetComponent<PlayerInputHub>();
        player = GetComponentInParent<Player>();
        anim = GetComponentInChildren<Animator>();
        bodyCol = GetComponent<Collider2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        Debug.Log(sr);
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
                        anim.SetBool("Dash", true);
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
        float gap = bodyCol.bounds.center.x - bodyCol.bounds.min.x;
        Vector3 spawnPos = new Vector3(bodyCol.bounds.center.x + (input.facingRight ? -gap : gap), bodyCol.bounds.center.y);
        Instantiate(player.jumpVFX, spawnPos, input.facingRight ? Quaternion.Euler(0f, 0f, 90f) : Quaternion.Euler(0f, 0f, 270f));
        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        float dir = input.facingRight ? 1f : -1f;
        rigid.velocity = new Vector2(dir * dashSpeed, 0f);

        float start = Time.time;
        while (Time.time < start + dashDuration)
        {
            rigid.velocity = new Vector2(dir * dashSpeed, 0f);
            yield return null;
        }
        rigid.velocity = new Vector2(0f, 0f);
        anim.SetBool("Dash", false);
    }

    private IEnumerator FastDrop()
    {
        float downwardSpeed = -50f;     // 빠른 하강 속도
        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        PlayerMove move = GetComponent<PlayerMove>();
        // ActionLock과 입력 차단
        ActionLockSystem.Instance.LockOnManual(anim, input);
        Coroutine coroutine = StartCoroutine(DropVFX());
        //start drop
        while (!move.isGrounded)
        {
            // x축을 0으로 고정하고 y축만 강제 하강
            rigid.velocity = new Vector2(0f, downwardSpeed);
            yield return null;
        }
        //end drop
        StopCoroutine(coroutine);
        // Grounded → unlock
        ActionLockSystem.Instance.LockOffManual(anim, input);
    }
    private IEnumerator DropVFX()
    {
        while (true)
        {
            Vector3 spawnPos = new Vector3(bodyCol.bounds.center.x, (bodyCol.bounds.center.y + bodyCol.bounds.min.y * 3) / 4, 0f);
            var vfx = Instantiate(player.jumpVFX, spawnPos, Quaternion.identity);
            vfx.transform.SetParent(player.transform);
            var animator = vfx.GetComponent<VFXAnimator>();
            if (!input.facingRight)
            {
                Vector3 scale = vfx.transform.localScale;
                scale.x *= -1;
                vfx.transform.localScale = scale;
            }
            yield return new WaitForSeconds(animator.duration);
        }
    }

}