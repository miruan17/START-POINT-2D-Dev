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
        StartCoroutine(LockOn(duration, anim, input));
    }

    private IEnumerator LockOn(float duration, Animator anim, PlayerInputHub input)
    {
        anim.SetBool("ActionLock", true);
        input.DisableInput();

        yield return new WaitForSeconds(duration);

        anim.SetBool("ActionLock", false);
        input.EnableInput();
    }
}
