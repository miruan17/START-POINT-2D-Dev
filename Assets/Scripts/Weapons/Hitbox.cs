using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Hitbox : MonoBehaviour
{
    public LayerMask targetLayer;
    private BoxCollider2D boxCollider;
    private Character caller;
    private Skill skill;
    private void Awake()
    {
        caller = GetComponentInParent<Character>();
    }
    private void OnDrawGizmos()
    {
        if (!gameObject.activeInHierarchy)
            return;

        if (boxCollider == null)
            boxCollider = GetComponent<BoxCollider2D>();

        if (boxCollider == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.matrix = transform.localToWorldMatrix;

        Gizmos.DrawWireCube(boxCollider.offset, boxCollider.size);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy")) // Hitbox meets Enemy
        {
            Enemy enemy = other.GetComponent<Enemy>();
            caller = GetComponentInParent<Character>();
            Player player = FindObjectOfType<Player>();
            if (caller != null)
            {
                enemy.status.CurrentHP -= caller.status.GetFinal(StatId.ATK);
                Debug.Log("Hit " + other.name + ", Damage " + caller.status.GetFinal(StatId.ATK));
                EffectManager enemyManager = enemy.getEffect();
                if (caller.CompareTag("Player")) //caller = Player
                {
                    EffectManager playerManager = player.getArgument();
                    ApplyEffect.applyEffect(enemy, playerManager);
                }
            }
            else // caller isn't Player (called by skill)
            {
                skill = GetComponentInParent<Skill>();
                EffectManager skillManager = skill.getEffect();
                ApplyEffect.applyEffect(enemy, skillManager);
            }
        }
    }
}