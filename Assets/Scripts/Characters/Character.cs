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
    protected List<ArgumentBase> argList;  // 적용시킬 effect
    protected EffectManager applyEffect;    // 적용될 effect

    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        bodyCol = GetComponent<Collider2D>();
        status = new CharacterStatusManager(characterStatus);
        argList = new();
        applyEffect = new EffectManager();
        argList.Add(new Skill_Bleeding());
        foreach (ArgumentBase arg in argList)
        {
            if (arg.GetType().Equals(new Skill_Bleeding().GetType()))
            {
                Skill_Bleeding skill = (Skill_Bleeding)arg;
                skill.setActiveBleeding();
            }
        }
    }

    #region Status Area




    #endregion



    #region Effect Area

    public List<ArgumentBase> getArgList()
    {
        return argList;
    }
    public EffectManager getApplyEffect()
    {
        return applyEffect;
    }


    #endregion



    private void FixedUpdate()
    {
        if (status.CurrentHP <= 0)
        {
            status.CurrentHP = 0;
            Debug.Log(this.name + "Dead");
            gameObject.SetActive(false);
        }
        List<Effect> deleteList = new();
        foreach (Effect effect in applyEffect.ReturnEffect())
        {
            if (!effect.IsExpired) effect.Runtime(this);
            else deleteList.Add(effect);
        }
        foreach (Effect effect in deleteList)
        {
            applyEffect.RemoveEffect(effect);
        }
    }
}