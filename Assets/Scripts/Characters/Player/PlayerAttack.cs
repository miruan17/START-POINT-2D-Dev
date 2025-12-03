using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Unity.Mathematics;

public class PlayerAttack : MonoBehaviour
{
    [Header("Default Weapon")]
    [SerializeField] public WeaponDef weapon;
    private PlayerInputHub input;
    private PlayerMove move;
    private Animator anim;
    private Player player;
    [SerializeField] public GameObject hitboxRoot;

    //hitboxGenerator
    private List<GameObject> comboHitboxes = new();
    private GameObject enhancedHitbox;
    private int maxCombo;

    private void WeaponUpdate(WeaponDef newWeapon)
    {
        weapon = newWeapon;
        // hitbox reset
        if (enhancedHitbox != null)
        {
            Destroy(enhancedHitbox);
            enhancedHitbox = null;
        }

        foreach (GameObject hitbox in comboHitboxes)
        {
            if (hitbox != null)
                Destroy(hitbox);
        }
        comboHitboxes.Clear();

        // is weapon null
        if (weapon == null)
        {
            Debug.LogWarning("null Weapon");
            return;
        }

        maxCombo = weapon.ComboAttacks.Count;

        // Enhanced hitbox generate
        if (weapon.EnhancedAttack != null && weapon.EnhancedAttack.Hitbox != null)
        {
            enhancedHitbox = Instantiate(weapon.EnhancedAttack.Hitbox, hitboxRoot.transform);
            //hitVFX 바인딩
            enhancedHitbox.GetComponent<Hitbox>().hitVFX = weapon.EnhancedAttack.hitVFX;

            enhancedHitbox.SetActive(false);
            enhancedHitbox.name = $"{weapon.displayName}_Enhanced";
        }
        else
        {
            enhancedHitbox = null;
        }

        // Combo hitbox generate
        for (int i = 0; i < maxCombo; i++)
        {
            var comboDef = weapon.ComboAttacks[i];
            GameObject combo = Instantiate(comboDef.Hitbox, hitboxRoot.transform);

            //hitVFX 바인딩
            combo.GetComponent<Hitbox>().hitVFX = comboDef.hitVFX;

            combo.SetActive(false);
            combo.name = $"{weapon.displayName}_Combo_{i + 1}";
            comboHitboxes.Add(combo);
        }

        int totalCount = comboHitboxes.Count + (enhancedHitbox != null ? 1 : 0);
        Debug.Log($"{totalCount} hitboxes updated " + $"= Enhanced: {(enhancedHitbox != null ? "Exist" : "not Exist")}, MaxCombo: {comboHitboxes.Count}");
    }

    private void Awake()
    {
        input = GetComponent<PlayerInputHub>();
        move = GetComponent<PlayerMove>();
        anim = GetComponentInChildren<Animator>();
        player = GetComponent<Player>();
        WeaponUpdate(weapon);
    }

    // Time variable
    public bool isHolding;                  // 입력 상태 표시
    private float pressTime;                // 키 입력 시간
    private float holdTime = 0;             // 키 입력 지속시간
    private float comboTime = 0;            // 콤보 적용 시간
    private float inputDelay = 0;           // 공격 쿨타임

    // attack variable
    private bool attackCall = false;        // FixedUpdate에 공격 실행 요청
    private int currentCombo = 0;           // 현재 콤보 수
    private bool isAttacking = false;       // 공격여부

    // direction variable
    private Vector2 direction;              // 방향

    //Attack Management
    private IEnumerator AttackOrder()
    {
        input.flip = false;
        AttackDef attack;
        GameObject hitbox;
        Debug.Log("Caster: " + this.name);
        if (holdTime > weapon.ComboDeadline)
        {
            attack = weapon.EnhancedAttack;
            hitbox = enhancedHitbox;
            Debug.Log("AttackType: Enhanced Attack");
        }
        else
        {
            attack = weapon.ComboAttacks[currentCombo];
            hitbox = comboHitboxes[currentCombo];
            Debug.Log("AttackType: " + $"Combo Attack {currentCombo + 1}");
        }
        yield return new WaitForSeconds(attack.preDelay);
        anim.SetFloat("SpeedMultiplier", player.attackClip.length / attack.hitTime);
        anim.SetTrigger("AttackTrigger");
        hitbox.SetActive(true);
        hitbox.GetComponent<Hitbox>().PlayVFX(attack.spawnVFX, attack.hitTime);
        yield return new WaitForSeconds(attack.hitTime);
        hitbox.SetActive(false);
        yield return new WaitForSeconds(attack.postDelay);

        input.flip = true;
        isAttacking = false;
    }

    private void Update()
    {
        if (anim.GetBool("ActionLock")) return;
        direction = input.MoveInput;
        if (direction.y > 0)
        {
            hitboxRoot.transform.rotation = quaternion.Euler(0, 0, 20f);
        }
        else if (direction.y < 0 && !move.isGrounded)
        {
            hitboxRoot.transform.rotation = quaternion.Euler(0, 0, -20f);
        }
        else
        {
            hitboxRoot.transform.rotation = quaternion.Euler(0, 0, 0);
        }

        // FUCKING IMPORTANT POINT
        if (Time.time < inputDelay)
        {
            // InputHub에 쌓인 요청을 해제시켜줘야 다음 프레임에 발동 안됨
            if (input.AttackRequest()) { }
            if (input.AttackReleaseRequest()) { }

            isHolding = false;
            attackCall = false;
            return;
        }

        // Start Measurement
        if (input.AttackRequest() && !isHolding)
        {
            isHolding = true;
            pressTime = Time.time;
        }

        // Finish Measurement
        if (input.AttackReleaseRequest() && isHolding)
        {
            isHolding = false;
            holdTime = Time.time - pressTime;
            attackCall = true;
        }
    }

    private void FixedUpdate()
    {
        if (anim.GetBool("ActionLock")) return;
        // attack pipeline
        if (Time.time < inputDelay)
        {
            attackCall = false;  // 자동 실행 래치 제거
            isHolding = false;  // 누르기 진행 상태 제거
            return;
        }
        if (!attackCall || isAttacking) return;
        if (Time.time < inputDelay) return;

        bool isEnhanced = holdTime > weapon.ComboDeadline;
        if (!isEnhanced)
        {
            if (Time.time <= comboTime && currentCombo < maxCombo - 1)
                currentCombo++;
            else
                currentCombo = 0;
        }

        isAttacking = true;
        StartCoroutine(AttackOrder());

        float start = Time.time;
        if (isEnhanced)
        {
            var ea = weapon.EnhancedAttack;
            inputDelay = start + ea.preDelay + ea.hitTime + ea.postDelay - 0.1f;
            comboTime = 0;
        }
        else
        {
            var ca = weapon.ComboAttacks[currentCombo];
            inputDelay = start + ca.preDelay + ca.hitTime + ca.postDelay - 0.1f;
            comboTime = inputDelay + 0.3f;
        }
        attackCall = false;
    }
    public void ForceReleaseAttack() // 차징중 ActionLock 발동시 즉시 처리
    {
        if (!isHolding) return;

        isHolding = false;
        holdTime = Time.time - pressTime;
        attackCall = true;
    }

}
