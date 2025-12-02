using System.Collections.Generic;
using UnityEngine;
public class Enemy : Character
{
    public static List<Enemy> AllEnemies = new List<Enemy>();
    public EnemyState state = EnemyState.Idle;
    public bool isRage = false;
    protected float idlePatrolTimer = 0f;
    public float idlePatrolInterval = 1f;   // 1초마다 시도
    public float idleToPatrolChance = 0.15f;
    public float patrolToIdleChance = 0.15f;

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
        StateUpdate();
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
        }
    }
    protected virtual void IdleUpdate() { }
    protected virtual void PatrolUpdate() { }
    protected virtual void ChaseUpdate() { }
    protected virtual void AttackUpdate() { }
    protected bool PlayerInRange(float range)
    {
        Player player = FindObjectOfType<Player>();
        if (player == null) return false;
        return Vector2.Distance(transform.position, player.transform.position) < range;
    }
}
