using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Collider2D))]
public abstract class Character : MonoBehaviour
{
    [Header("StatusDef")]
    public StatusDef status;

    protected SpriteRenderer spriteRenderer;
    protected Rigidbody2D rigid;
    protected Animator anim;
    protected Collider2D bodyCol;

    protected StatusManager stats;

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
    }

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
    public float FinalHp => stats.GetFinal(StatusType.Hp);
    public float FinalSp => stats.GetFinal(StatusType.Sp);
    public float FinalAtk => stats.GetFinal(StatusType.Atk);
    public float FinalAps => stats.GetFinal(StatusType.Aps);
    public float FinalDef => stats.GetFinal(StatusType.Def);
    public float FinalSpd => stats.GetFinal(StatusType.Spd);

    // Wrapper Method
    public void ApplyAdditional(string sourceId, StatusSourceKind kind, StatusType type, float value)
        => stats.AddModifier(new StatusModifier(sourceId, kind, type, StatusModKind.Additional, value));

    public void ApplyMultiple(string sourceId, StatusSourceKind kind, StatusType type, float valueAsRatio)
        => stats.AddModifier(new StatusModifier(sourceId, kind, type, StatusModKind.Multiple, valueAsRatio));

    public void RemoveBySource(string sourceId) => stats.RemoveBySource(sourceId);

    //System.collections.Generic namespace의 Dictionary
    public System.Collections.Generic.Dictionary<StatusSourceKind, StatContribution>
        GetContributionsByKind(StatusType stat) => stats.GetContributionsByKind(stat);
}