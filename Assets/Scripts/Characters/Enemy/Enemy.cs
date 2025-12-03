using System.Collections.Generic;
using UnityEngine;
public class Enemy : Character
{
    public float groundCheckDepth = 0.1f;
    public LayerMask groundMask;
    public static List<Enemy> AllEnemies = new List<Enemy>();
    public EnemyState state = EnemyState.Idle;
    public bool isRage = false;
    protected float idlePatrolTimer = 0f;
    public float idlePatrolInterval = 1f;   // 1초마다 시도
    public float idleToPatrolChance = 0.3f;
    public float patrolToIdleChance = 0.15f;
    public float frozenTimer = 0f;
    public float Speed = 4.0f;
    protected bool isGrounded = true;

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
    private void OnDisable()
    {
        AllEnemies.Remove(this);
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        isGrounded = OverlapGround();
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
            case EnemyState.Frozen:
                FrozenUpdate();
                break;
        }
    }
    protected virtual void IdleUpdate() { }
    protected virtual void PatrolUpdate() { }
    protected virtual void ChaseUpdate() { }
    protected virtual void AttackUpdate()
    {
        if (is_Freeze)
        {
            state = EnemyState.Frozen;
            return;
        }
    }
    protected virtual void FrozenUpdate()
    {
        frozenTimer += Time.deltaTime;
        if (frozenTimer >= ((Effect_Freeze)EffectLib.playerEffectLib.getEffectbyID("Freeze"))._stats[StatId_Effect_Freeze.FRZ_term].Get())
        {
            frozenTimer = 0;
            if (isRage) state = EnemyState.Chase;
            else state = EnemyState.Idle;
        }
    }
    protected bool PlayerInRange(float range)
    {
        Player player = FindObjectOfType<Player>();
        if (player == null) return false;
        return Vector2.Distance(transform.position, player.transform.position) < range;
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

}
