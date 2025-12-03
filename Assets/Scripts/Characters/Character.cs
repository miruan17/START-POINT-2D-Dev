using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

[DisallowMultipleComponent]

public abstract class Character : MonoBehaviour
{
    [Header("StatusDef")]
    public CharacterStatusDef characterStatus;

    protected SpriteRenderer spriteRenderer;
    protected Rigidbody2D rigid;
    protected Animator anim;
    protected Collider2D bodyCol;

    public CharacterStatusManager status;
    protected EffectManager effect;
    protected EffectManager argument;
    public bool is_Freeze = false;

    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        bodyCol = GetComponent<Collider2D>();
        status = new CharacterStatusManager(characterStatus);
        effect = new EffectManager(this);
        argument = new EffectManager(this);
    }

    #region Status Area




    #endregion



    #region Effect Area

    public EffectManager getEffect()
    {
        return effect;
    }
    public EffectManager getArgument()
    {
        return argument;
    }
    #endregion

    public abstract void DeathTrigger();

    protected virtual void FixedUpdate()
    {
        // 지향 구조
        DeathTrigger();
        effect.RuntimeEffect();
    }
}