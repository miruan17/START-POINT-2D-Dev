using System;
using System.Collections.Generic;
using UnityEngine;
public class Enemy : Character
{
    [Header("Ground Check")]
    public float groundCheckDepth = 0.1f;
    public LayerMask groundMask;
    protected bool isGrounded = true;
    public bool prevGrounded = false;

    [Header("")]
    public static List<Enemy> AllEnemies = new List<Enemy>();

    [Header("State")]
    public EnemyState state = EnemyState.Idle;
    public bool isRage = false;
    protected float idlePatrolTimer = 0f;
    public float idlePatrolInterval = 1f;   // 1초마다 시도
    public float idleToPatrolChance = 0.3f;
    public float patrolToIdleChance = 0.15f;
    public bool is_Hit = false;
    private float hitTimer = 0f;
    private float hitDuration = 0f;
    [Header("State")]
    public float AttackTime;
    public float AttackTimer = 0f;
    public GameObject hitbox;
    [SerializeField] public GameObject hitboxRoot;

    [Header("ETC.")]
    public float Speed = 4.0f;
    public AttackDef attackDef;
    public bool facingRight = true;

    public override void DeathTrigger()
    {
        if (status.CurrentHP <= 0)
        {
            status.CurrentHP = 0;
            Debug.Log(this.name + "Dead");
            gameObject.SetActive(false);
        }
    }
    private void OnEnable()
    {
        AllEnemies.Add(this);
    }
    protected override void Awake()
    {
        base.Awake();
        AttackTime = attackDef.hitTime + attackDef.preDelay + attackDef.postDelay;
        hitbox = Instantiate(attackDef.Hitbox, hitboxRoot.transform);
        hitbox.GetComponent<Hitbox>().hitVFX = attackDef.hitVFX;
        hitbox.SetActive(false);
    }
    private void OnDisable()
    {
        AllEnemies.Remove(this);
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        isGrounded = OverlapGround();
        if (isGrounded != prevGrounded)
        {
            if (isGrounded) // air -> ground
            {
                // PlatformGroupID 갱신
                PlatformGroupID pid = FindPlatform();
                if (pid != null) currentPlatform = pid;
            }
        }
        prevGrounded = isGrounded;
        StateUpdate();
    }
    private void Update()
    {
        anim.SetBool("Move", Mathf.Abs(rigid.velocity.x) > 0.01f && isGrounded);
        anim.SetBool("Jump", !isGrounded);
    }
    protected virtual void StateUpdate()
    {
        switch (state)
        {
            case EnemyState.Idle:
                IdleUpdate();
                break;
            case EnemyState.Patrol:
                PatrolUpdate();
                break;
            case EnemyState.Chase:
                ChaseUpdate();
                break;
            case EnemyState.Attack:
                AttackUpdate();
                break;
            case EnemyState.Hit:
                HitUpdate();
                break;
        }
    }
    protected virtual void IdleUpdate() { }
    protected virtual void PatrolUpdate() { }
    protected virtual void ChaseUpdate() { }
    protected virtual void AttackUpdate()
    {
    }
    protected virtual void HitUpdate()
    {
        hitTimer += Time.deltaTime;

        // stun animation optional
        anim.SetBool("Hit", true);

        if (hitTimer >= hitDuration)
        {
            is_Hit = false;
            anim.SetBool("Hit", false);

            // return to default logic
            state = EnemyState.Chase;
        }
    }

    public void ApplyHit(String source, float duration)
    {
        if (state == EnemyState.Hit && !source.Equals("Freeze")) return;
        is_Hit = true;
        hitDuration = duration;
        hitTimer = 0f;

        state = EnemyState.Hit;

        ApplyKnockback(new Vector2(0, 0), 0);
    }
    public void ApplyHit(String source, float duration, Vector2 knockback, float attackerx)
    {
        ApplyHit(source, duration);
        ApplyKnockback(knockback, attackerx);
    }
    public void ApplyKnockback(Vector2 force, float attacker)
    {
        if (rigid == null) return;
        Debug.Log("in akb");
        // 공격자가 왼쪽에 있으면 +, 오른쪽에 있으면 - 방향 적용
        float direction = (transform.position.x - attacker) >= 0 ? 1f : -1f;
        Vector2 finalForce = new Vector2(force.x * direction, force.y);

        rigid.velocity = new Vector2(0, rigid.velocity.y); // 기존 수평속도 초기화
        rigid.AddForce(finalForce, ForceMode2D.Impulse);
    }
    protected bool PlayerInRange(float range)
    {
        Player player = FindObjectOfType<Player>(true);
        if (player == null) return false;
        return Vector2.Distance(transform.position, player.transform.position) < range;
    }
    protected bool PlayerInAttackRange(float range)
    {
        Player player = FindObjectOfType<Player>(true);
        if (player == null) return false;
        return Mathf.Abs(player.transform.localPosition.x - transform.localPosition.x) < range;
    }
    protected PlatformGroupID getPlayerPlatformGroupID()
    {
        Player player = FindObjectOfType<Player>(true);
        return player.currentPlatform;
    }
    protected bool OverlapGround()
    {
        if (!bodyCol) return false;

        Bounds b = bodyCol.bounds;
        Vector2 boxCenter = new Vector2(b.center.x, b.min.y - groundCheckDepth * 0.5f);
        Vector2 boxSize = new Vector2(b.size.x * 0.9f, groundCheckDepth);

        bool hit = Physics2D.OverlapBox(boxCenter, boxSize, 0f, groundMask);

        Debug.DrawLine(new Vector2(boxCenter.x - boxSize.x / 2f, boxCenter.y),
                       new Vector2(boxCenter.x + boxSize.x / 2f, boxCenter.y),
                       hit ? Color.green : Color.red);

        return hit;
    }
    protected bool CheckFrontAir()
    {
        Bounds b = bodyCol.bounds;

        float offsetX = facingRight ? b.extents.x : -b.extents.x;
        float checkDepth = groundCheckDepth;

        Vector2 boxCenter = new Vector2(b.center.x + offsetX, b.min.y - checkDepth * 3f);
        Vector2 boxSize = new Vector2(0.1f, checkDepth * 6f);

        bool hit = Physics2D.OverlapBox(boxCenter, boxSize, 0f, groundMask);

        Debug.DrawLine(new Vector2(boxCenter.x, boxCenter.y),
                       new Vector2(boxCenter.x, boxCenter.y + 0.2f),
                       hit ? Color.green : Color.red);

        // 공중이면 true
        return !hit;
    }
    public void Flip()
    {
        facingRight = !facingRight;
        Vector3 s = transform.localScale;
        s.x *= -1;
        transform.localScale = s;
    }
    public Vector3 getPlayerPosition()
    {
        Player player = FindObjectOfType<Player>(true);
        return player.transform.localPosition;
    }

    public void MoveRight()
    {
        Vector2 v = rigid.velocity;
        v.x = Speed;
        rigid.velocity = v;
        if (!facingRight) Flip();
    }
    public void MoveLeft()
    {
        Vector2 v = rigid.velocity;
        v.x = -Speed;
        rigid.velocity = v;
        if (facingRight) Flip();
    }
    public void StopMove()
    {
        Vector2 v = rigid.velocity;
        v.x = 0;
        rigid.velocity = v;
    }
}
