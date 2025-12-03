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
        if (PlayerInAttackRange(attackRange))
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
        if (isGrounded)
        {
            // dx > 0 -> enemy, player || dx < 0 -> player, enemy
            float dx = getPlayerPosition().x - transform.localPosition.x;

            if (getPlayerPlatformGroupID().platformGroupID == currentPlatform.platformGroupID || currentPlatform.leftBlocked && currentPlatform.rightBlocked)
            {
                // player와 enemy가 같은 블럭 또는 양쪽이 막혀있는 경우(가장 아래층)
                if (dx > 0) MoveRight();
                else if (dx == 0) StopMove();
                else MoveLeft();
            }
            else
            {
                // 다른 블럭일 경우
                if (dx > 0)
                {
                    if (currentPlatform.rightBlocked) MoveLeft();
                    else MoveRight();
                }
                else
                {
                    if (currentPlatform.leftBlocked) MoveRight();
                    else MoveLeft();
                }
            }
        }
    }

    protected override void AttackUpdate()
    {
        // Attack이 끝나면 다시 Chase
        state = EnemyState.Chase;
    }
}
