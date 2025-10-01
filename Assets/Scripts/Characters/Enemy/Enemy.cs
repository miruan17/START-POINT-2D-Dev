using UnityEngine;

[DisallowMultipleComponent]
public class Enemy : Character
{
    [Header("Enemy Feedback")]
    public Color deadColor = Color.gray;
    public Color hitFlashColor = new Color(1f, 0.6f, 0.6f);
    public float hitFlashTime = 0.06f;

    public float DebugHp => currentHp;

    private bool _isDead;
    private Color _originalColor;
    private float _flashUntil;

    protected override void Awake()
    {
        base.Awake();
        _isDead = false;
        if (spriteRenderer != null)
            _originalColor = spriteRenderer.color;
    }

    private void Update()
    {
        if (!_isDead && spriteRenderer != null && Time.time > _flashUntil)
            spriteRenderer.color = _originalColor;
    }

    public void TakeDamage(float damage)
    {
        if (_isDead) return;

        float finalDamage = Mathf.Max(1f, damage - FinalDef);
        currentHp -= finalDamage;

        if (spriteRenderer != null)
        {
            spriteRenderer.color = hitFlashColor;
            _flashUntil = Time.time + hitFlashTime;
        }

        if (currentHp <= 0) OnDeath();
    }

    private void OnDeath()
    {
        _isDead = true;
        if (spriteRenderer != null) spriteRenderer.color = deadColor;

        if (bodyCol != null) bodyCol.enabled = false;
        if (rigid != null)   rigid.simulated = false;
    }
}
