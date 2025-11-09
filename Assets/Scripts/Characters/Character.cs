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
    protected EffectManager effects;

    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        bodyCol = GetComponent<Collider2D>();
        status = new CharacterStatusManager(characterStatus);
    }

    #region Status Area




    #endregion



    #region Effect Area




    #endregion



    private void FixedUpdate()
    {
        if (status.CurrentHP <= 0)
        {
            status.SetCurrentHP(0);
            Debug.Log(this.name + "Dead");
            gameObject.SetActive(false);
        }
    }
}