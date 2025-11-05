using System.Numerics;
using UnityEngine;

[DisallowMultipleComponent]

public abstract class Character : MonoBehaviour
{
    [Header("StatusDef")]
    public StatusDef status;

    protected SpriteRenderer spriteRenderer;
    protected Rigidbody2D rigid;
    protected Animator anim;
    protected Collider2D bodyCol;

    protected StatusManager stats;
    protected EffectManager effects;

    // Runtime value
    protected float currentHp;
    protected float currentSp;

    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        bodyCol = GetComponent<Collider2D>();

        BuildStatus();
        currentHp = GetMaxHp;
        currentSp = GetMaxSp;
    }

    #region Status Area
    private void BuildStatus()
    {
        stats = new StatusManager();

        stats.SetBase(StatusType.Hp, status.DefaultHp);
        stats.SetBase(StatusType.Sp, status.DefaultSp);
        stats.SetBase(StatusType.Atk, status.DefaultAtk);
        stats.SetBase(StatusType.Aps, status.DefaultAps);
        stats.SetBase(StatusType.Def, status.DefaultDef);
        stats.SetBase(StatusType.Spd, status.DefaultSpd);

        // runtime value
        currentHp = stats.GetFinal(StatusType.Hp);
        currentSp = stats.GetFinal(StatusType.Sp);
    }

    // Final value
    public float GetMaxHp => stats.GetFinal(StatusType.Hp);
    public float GetMaxSp => stats.GetFinal(StatusType.Sp);
    public float GetAtk => stats.GetFinal(StatusType.Atk);
    public float GetAps => stats.GetFinal(StatusType.Aps);
    public float GetDef => stats.GetFinal(StatusType.Def);
    public float GetSpd => stats.GetFinal(StatusType.Spd);

    // Wrapper Method - Modifier
    public void ApplyAdditional(string sourceId, StatusSourceKind kind, StatusType type, float value)
        => stats.AddModifier(new StatusModifier(sourceId, kind, type, StatusModKind.Additional, value));

    public void ApplyMultiple(string sourceId, StatusSourceKind kind, StatusType type, float valueAsRatio)
        => stats.AddModifier(new StatusModifier(sourceId, kind, type, StatusModKind.Multiple, valueAsRatio));

    public void RemoveBySource(string sourceId) => stats.RemoveBySource(sourceId);

    //System.collections.Generic namespace의 Dictionary
    public System.Collections.Generic.Dictionary<StatusSourceKind, StatContribution>
        GetContributionsByKind(StatusType stat) => stats.GetContributionsByKind(stat);

    public void getDamage(float value)
    {
        currentHp -= value;

    }

    #endregion



    #region Effect Area
    
    
    
    #endregion



    private void FixedUpdate()
    {
        if (currentHp > GetMaxHp) currentHp = GetMaxHp;
        if (currentSp > GetMaxSp) currentSp = GetMaxSp;
        if (currentHp <= 0)
        {
            currentHp = 0;
            Debug.Log(this.name + "Dead");
            gameObject.SetActive(false);
        }
    }
}