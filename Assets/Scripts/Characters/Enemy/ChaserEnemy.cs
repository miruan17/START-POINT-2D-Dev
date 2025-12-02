using UnityEngine;

public class ChaserEnemy : NormalEnemy
{
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
        isRage = true;

        if (PlayerInRange(attackRange))
        {
            state = EnemyState.Attack;
            return;
        }
    }
}
