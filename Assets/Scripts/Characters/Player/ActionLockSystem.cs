using System.Collections;
using UnityEngine;

public class ActionLockSystem : MonoBehaviour
{
    public static ActionLockSystem Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void LockOnTime(float duration, Animator anim, PlayerInputHub input)
    {
        ForceReleaseAttackIfCharging();
        StartCoroutine(LockOn(duration, anim, input));
    }
    public void LockOnManual(Animator anim, PlayerInputHub input)
    {
        ForceReleaseAttackIfCharging();
        anim.SetBool("ActionLock", true);
        input.DisableInput();

    }
    public void LockOffManual(Animator anim, PlayerInputHub input)
    {
        anim.SetBool("ActionLock", false);
        input.EnableInput();
    }
    private IEnumerator LockOn(float duration, Animator anim, PlayerInputHub input)
    {
        anim.SetBool("ActionLock", true);
        input.DisableInput();

        yield return new WaitForSeconds(duration);

        anim.SetBool("ActionLock", false);
        input.EnableInput();
    }
    private void ForceReleaseAttackIfCharging() // 공격 차징중 ActionLock 발동 시, 공격을 배출하는 시스템
    {
        PlayerAttack atk = FindObjectOfType<PlayerAttack>();
        if (atk != null && atk.isHolding)   // isHolding 검사
        {
            atk.ForceReleaseAttack();
        }
    }
}
