using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Hitbox : MonoBehaviour
{
    public LayerMask targetLayer;
    private BoxCollider2D boxCollider;
    private Character caller;
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
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            caller = GetComponentInParent<Character>();
            enemy.status.CurrentHP -= caller.status.GetFinal(StatId.ATK);
            Debug.Log("Hit " + other.name + ", Damage " + caller.status.GetFinal(StatId.ATK));
            // send Player's enchantableEffectList to Enemy
            EffectManager enemyManager = enemy.getApplyEffect();
            List<ArgumentBase> playerArgList = caller.getArgList();

            foreach (ArgumentBase arg in playerArgList)
            {
                foreach (Effect effect in arg.effectManager.ReturnEffect())
                {
                    if (effect.identifier.Equals("Bleeding") && enemyManager.SearchNumEffect("Bleeding") >= 5) continue;
                    enemyManager.AddEffect(effect.copy());
                }
            }

        }
    }
}