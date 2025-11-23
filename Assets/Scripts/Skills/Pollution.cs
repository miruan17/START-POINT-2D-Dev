using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Unity.Mathematics;

public class Pollution : Skill
{
    [SerializeField]
    private float duration = 10f;
    [SerializeField]
    private GameObject hitboxdef;
    private GameObject hitbox;
    private void Start()
    {
        Player player = FindObjectOfType<Player>();
        Effect effect = player.getEffectLib().getEffectbyID("Poison");
        if (effect == null) effect = EffectLib.Instance.getEffectbyID("Poison");
        effectManager.AddEffect(effect);
        hitbox = Instantiate(hitboxdef, this.transform);
        hitbox.SetActive(false);
        StartCoroutine(Attack());
    }
    private IEnumerator Attack()
    {
        for (int i = 0; i < (int)duration; i++)
        {
            hitbox.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            hitbox.SetActive(false);
            yield return new WaitForSeconds(0.9f);
        }
    }

}