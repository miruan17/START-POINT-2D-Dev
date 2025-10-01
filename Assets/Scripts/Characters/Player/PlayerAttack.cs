using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerAttack : MonoBehaviour
{
    public enum FacingSource
    {
        ScaleX,
        SpriteFlipX,
        CustomSign
    }

    [Header("Facing")]
    public FacingSource facingSource = FacingSource.SpriteFlipX;
    public bool invertFlipX = false;
    public int customFacingSign = 1;

    [Header("Hitbox (OverlapBox)")]
    public Vector2 boxSize   = new Vector2(1.2f, 0.6f);
    public Vector2 boxOffset = new Vector2(0.7f, 0.1f);
    public float   boxAngleDeg = 0f;
    public bool    rotateWithFacing = true;
    public LayerMask enemyLayers;

    [Header("Combat")]
    public float cooldown = 0.25f;
    public float damageScale = 1.0f;

    [Header("Debug")]
    public float debugDamageOverride = 0f;
    public bool logHits = false;
    public bool drawRuntimeBox = true;

    private float _lastAttackTime = -999f;
    private readonly HashSet<Collider2D> _hitThisSwing = new();

    private Player _player;
    private Character _owner;
    private SpriteRenderer _sprite;

    private void Awake()
    {
        _player  = GetComponent<Player>();
        _owner   = GetComponent<Character>();
        _sprite  = GetComponentInChildren<SpriteRenderer>();
    }

    public void SetFacing(int sign) => customFacingSign = Mathf.Sign(sign) >= 0 ? 1 : -1;

    private void Update()
    {
        if (_player != null && _player.ConsumeAttackRequest())
            TryAttack();
    }

    public void TryAttack()
    {
        if (Time.time - _lastAttackTime < cooldown) return;
        _lastAttackTime = Time.time;
        DoAttackOverlap();
    }

    private int GetFacingSign()
    {
        switch (facingSource)
        {
            case FacingSource.ScaleX:
                return transform.lossyScale.x >= 0 ? 1 : -1;
            case FacingSource.SpriteFlipX:
                if (_sprite == null) return 1;
                bool flip = _sprite.flipX ^ invertFlipX;
                return flip ? -1 : 1;
            case FacingSource.CustomSign:
                return customFacingSign >= 0 ? 1 : -1;
            default:
                return 1;
        }
    }

    private void DoAttackOverlap()
    {
        _hitThisSwing.Clear();

        int facing = GetFacingSign();

        Vector2 localOffset = new Vector2(boxOffset.x * facing, boxOffset.y);
        Vector2 worldCenter = transform.TransformPoint(localOffset);

        float angleDeg = rotateWithFacing && facing < 0 ? -boxAngleDeg : boxAngleDeg;

        LayerMask maskToUse = enemyLayers.value == 0 ? ~0 : enemyLayers;

        Collider2D[] hits = Physics2D.OverlapBoxAll(worldCenter, boxSize, angleDeg, maskToUse);

        if (drawRuntimeBox)
            DrawBoxDebug(worldCenter, boxSize, angleDeg, 0.12f);

        if (logHits) Debug.Log($"[PlayerAttack] Overlap hits = {(hits?.Length ?? 0)}");
        if (hits == null || hits.Length == 0) return;

        float baseDamage = _owner != null ? _owner.FinalAtk : 10f;
        float damage = debugDamageOverride > 0f ? debugDamageOverride : baseDamage * damageScale;

        foreach (var col in hits)
        {
            if (col == null || _hitThisSwing.Contains(col)) continue;
            _hitThisSwing.Add(col);

            var enemy = col.GetComponentInParent<Enemy>() ?? col.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                if (logHits) Debug.Log($"[PlayerAttack] Hit {enemy.name} for {damage}");
            }
            else if (logHits)
            {
                Debug.Log($"[PlayerAttack] Collider {col.name} has no Enemy component.");
            }
        }
    }

    private void DrawBoxDebug(Vector2 center, Vector2 size, float angleDeg, float duration)
    {
        Vector2 half = size * 0.5f;
        Quaternion rot = Quaternion.Euler(0, 0, angleDeg);
        Vector2 a = center + (Vector2)(rot * new Vector3(-half.x, -half.y));
        Vector2 b = center + (Vector2)(rot * new Vector3(-half.x,  half.y));
        Vector2 c = center + (Vector2)(rot * new Vector3( half.x,  half.y));
        Vector2 d = center + (Vector2)(rot * new Vector3( half.x, -half.y));
        Debug.DrawLine(a, b, Color.red, duration);
        Debug.DrawLine(b, c, Color.red, duration);
        Debug.DrawLine(c, d, Color.red, duration);
        Debug.DrawLine(d, a, Color.red, duration);
    }

    private void OnDrawGizmosSelected()
    {
        // Scene 뷰에서 동일 계산 사용 (런타임/에디터 일관성)
        int facing = Application.isPlaying ? GetFacingSign() : 1;
        Vector2 localOffset = new Vector2(boxOffset.x * facing, boxOffset.y);
        Vector2 worldCenter = transform.TransformPoint(localOffset);
        float angleDeg = rotateWithFacing && facing < 0 ? -boxAngleDeg : boxAngleDeg;

        Gizmos.color = new Color(1f, 0.2f, 0.2f, 0.35f);
        Gizmos.matrix = Matrix4x4.TRS(worldCenter, Quaternion.Euler(0, 0, angleDeg), Vector3.one);
        Gizmos.DrawCube(Vector3.zero, new Vector3(boxSize.x, boxSize.y, 1f));
    }
}
