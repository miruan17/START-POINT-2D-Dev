using UnityEngine;

public class NormalEnemy : Enemy
{
    public float detectRange = 5f;
    public float attackRange = 1.5f;

    protected override void IdleUpdate()
    {
        if (PlayerInRange(detectRange) || isRage)
        {
            idlePatrolTimer = 0;
            state = EnemyState.Chase;
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
        if (PlayerInRange(detectRange) || isRage)
        {
            idlePatrolTimer = 0;
            state = EnemyState.Chase;
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
    }


    protected override void ChaseUpdate()
    {
        // Chase → Attack
        if (PlayerInRange(attackRange))
        {
            state = EnemyState.Attack;
            return;
        }

        // Chase → Idle
        if (!isRage && !PlayerInRange(detectRange))
            state = EnemyState.Idle;
    }

    protected override void AttackUpdate()
    {
        // Attack이 끝나면 다시 Chase
        state = EnemyState.Chase;
    }
}
