using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Hitbox : MonoBehaviour
{
    public GameObject hitVFX;
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
    public void PlayVFX(GameObject VFX, float duration)
    {
        if (VFX == null) return;

        GameObject vfxObj = Instantiate(VFX, transform.position, transform.rotation);

        BoxCollider2D col = GetComponent<BoxCollider2D>();

        Vector2 worldHitboxSize = Vector2.Scale(col.size, transform.lossyScale);

        VFXFollow follow = vfxObj.GetComponent<VFXFollow>();
        if (follow != null)
            follow.target = this.transform;

        SpriteRenderer sr = vfxObj.GetComponent<SpriteRenderer>();
        VFXAnimator animator = vfxObj.GetComponent<VFXAnimator>();

        if (sr != null)
        {
            Vector2 spriteSize = sr.sprite.bounds.size;

            float scaleX = worldHitboxSize.x / spriteSize.x;
            float scaleY = worldHitboxSize.y / spriteSize.y;

            if (animator != null)
            {
                scaleX *= animator.xsize;
                scaleY *= animator.ysize;
            }
            vfxObj.transform.localScale = new Vector3(scaleX, scaleY, 1f);
        }

        if (animator != null)
            animator.duration = duration;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if ((targetLayer & (1 << other.gameObject.layer)) == 0)
            return;
        if (other.CompareTag("Enemy")) // Hitbox meets Enemy
        {
            Enemy enemy = other.GetComponent<Enemy>();
            caller = GetComponentInParent<Character>();
            Player player = FindObjectOfType<Player>();
            skill = GetComponentInParent<Skill>();
            if (skill == null)
            {
                if (caller.CompareTag("Player")) //caller = player
                {
                    createHitVFX(other);
                    enemy.status.CurrentHP -= caller.status.GetFinal(StatId.ATK);
                    Debug.Log("Hit " + other.name + ", Damage " + caller.status.GetFinal(StatId.ATK));
                    EffectManager enemyManager = enemy.getEffect();
                    EffectManager playerManager = player.getArgument();
                    ApplyEffect.applyEffect(enemy, playerManager.ReturnEffect());
                    if (enemy.is_Freeze)
                    {
                        enemy.status.CurrentHP -= ((Effect_Freeze)EffectLib.playerEffectLib.getEffectbyID("Freeze")).getDmg();
                    }
                }
            }
            else // called by skill
            {
                if (skill.dmg != 0)
                {
                    enemy.status.CurrentHP -= skill.dmg;
                }
                if (skill.skillType == SkillType.Attack) createHitVFX(other);
                EffectManager skillManager = skill.getEffect();
                ApplyEffect.applyEffect(enemy, skillManager.ReturnEffect());
            }
        }
        else
        {
            caller = GetComponentInParent<Character>();
            Player player = FindObjectOfType<Player>();
            createHitVFX(other);
            player.status.CurrentHP -= caller.status.GetFinal(StatId.ATK);
            ApplyEffect.applyEffect(player, caller.getArgument().ReturnEffect());
        }
    }
    void createHitVFX(Collider2D other)
    {
        if (hitVFX == null) return;
        Vector2 hitPoint = Physics2D.ClosestPoint(transform.position, other);
        GameObject obj = Instantiate(hitVFX, hitPoint, Quaternion.identity);
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        VFXAnimator animator = obj.GetComponent<VFXAnimator>();

    }
}