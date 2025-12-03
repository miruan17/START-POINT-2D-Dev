using Unity.Burst.Intrinsics;
using UnityEngine;

public class NormalEnemy : Enemy
{
    public float detectRange = 5f;
    public float detectRangeOnChase = 10f;
    public float attackRange = 1.5f;

    protected override void IdleUpdate()
    {
        if (is_Freeze)
        {
            idlePatrolTimer = 0;
            state = EnemyState.Frozen;
            return;
        }
        if (PlayerInRange(detectRange) || isRage)
        {
            idlePatrolTimer = 0;
            state = EnemyState.Chase;
            return;
        }
        idlePatrolTimer += Time.deltaTime;

        if (idlePatrolTimer >= idlePatrolInterval)
        {
            idlePatrolTimer = 0f;

            if (Probability.Attempt(idleToPatrolChance))
            {
                state = EnemyState.Patrol;
                return;
            }
        }

        // 일반 Idle 행동(대기 애니메이션 등)
    }

    protected override void PatrolUpdate()
    {
        if (is_Freeze)
        {
            idlePatrolTimer = 0;
            state = EnemyState.Frozen;
            return;
        }
        if (PlayerInRange(detectRange) || isRage)
        {
            idlePatrolTimer = 0;
            state = EnemyState.Chase;
            return;
        }
        idlePatrolTimer += Time.deltaTime;

        if (idlePatrolTimer >= idlePatrolInterval)
        {
            idlePatrolTimer = 0f;

            if (Probability.Attempt(patrolToIdleChance))
            {
                state = EnemyState.Idle;
                return;
            }
        }
        // 일반 Patrol 행동(이동 등)
        if (isGrounded)
        {
            Vector2 v = rigid.velocity;
            v.x = facingRight ? Speed : -Speed;
            rigid.velocity = v;
            if (CheckFrontAir() || Probability.Attempt(0.01f)) Flip();
        }
    }


    protected override void ChaseUpdate()
    {
        if (is_Freeze)
        {
            state = EnemyState.Frozen;
            return;
        }
        // Chase → Attack
        if (PlayerInRange(attackRange))
        {
            state = EnemyState.Attack;
            return;
        }
        // Chase → Idle
        if (!isRage && !PlayerInRange(detectRangeOnChase))
        {
            state = EnemyState.Idle;
            return;
        }
    }

    protected override void AttackUpdate()
    {
        // Attack이 끝나면 다시 Chase
        state = EnemyState.Chase;
    }
}
