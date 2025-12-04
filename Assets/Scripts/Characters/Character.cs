using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

[DisallowMultipleComponent]

public abstract class Character : MonoBehaviour
{
    [Header("StatusDef")]
    public CharacterStatusDef characterStatus;

    protected SpriteRenderer spriteRenderer;
    protected Rigidbody2D rigid;
    protected Animator anim;
    public AnimationClip attackClip;
    protected Collider2D bodyCol;

    public CharacterStatusManager status;
    protected EffectManager effect;
    protected EffectManager argument;
    public PlatformGroupID currentPlatform;
    public RectTransform IconContainer;
    public Dictionary<String, Sprite> iconSprite = new Dictionary<String, Sprite>();
    public List<Sprite> sampleSprite;
    public List<Sprite> resultSprite;
    public GameObject iconPrefab;
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
        resultSprite = new List<Sprite>();
        RuntimeAnimatorController ac = anim.runtimeAnimatorController;
        iconSprite["Bleeding"] = sampleSprite[0];
        iconSprite["Poison"] = sampleSprite[1];
        iconSprite["Frozen"] = sampleSprite[2];
        iconSprite["Ice"] = sampleSprite[2];
        iconSprite["Fire"] = sampleSprite[3];
        foreach (var clip in ac.animationClips)
        {
            if (clip.name == "PlayerAttack")
            {
                attackClip = clip;
                break;
            }
        }
    }
    protected virtual void Update()
    {
        resultSprite.Clear();
        Debug.Log(name + " " + effect.ReturnEffect());
        if (effect.ReturnEffect() != null)
        {
            foreach (var e in effect.ReturnEffect())
            {
                Debug.Log(name + " " + e);
                if (e == null) continue;
                if (iconSprite.TryGetValue(e.identifier, out Sprite sp))
                {
                    if (e.enable)
                    {
                        Debug.Log(e.identifier);
                        resultSprite.Add(sp);
                    }
                }
            }
        }
        RefreshStatusIcons();
    }
    public void RefreshStatusIcons()
    {
        // 1. 기존 아이콘 모두 제거
        for (int i = IconContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(IconContainer.GetChild(i).gameObject);
        }

        // 2. resultSprite 기반으로 새 아이콘 생성
        foreach (var sp in resultSprite)
        {
            if (sp == null) continue;

            GameObject iconObj = Instantiate(iconPrefab, IconContainer);
            var img = iconObj.GetComponent<UnityEngine.UI.Image>();
            img.sprite = sp;
        }
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

    public PlatformGroupID FindPlatform()
    {
        int platformMask = LayerMask.GetMask("Platform");

        float halfWidth = bodyCol.bounds.extents.x * 0.9f;
        float rayLength = 0.3f;

        // ★ Collider의 정확한 발바닥 위치
        float footY = bodyCol.bounds.min.y - 0.05f;

        Vector2[] offsets =
        {
        new Vector2(-halfWidth, 0),
        new Vector2(0, 0),
        new Vector2(halfWidth, 0),
    };

        foreach (var off in offsets)
        {
            Vector2 origin = new Vector2(transform.position.x + off.x, footY);

            RaycastHit2D hit = Physics2D.Raycast(
                origin,
                Vector2.down,
                rayLength,
                platformMask
            );

            if (hit.collider != null)
                return hit.collider.GetComponent<PlatformGroupID>();
        }

        return null;
    }
}