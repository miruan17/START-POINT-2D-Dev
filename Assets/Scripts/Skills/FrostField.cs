using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Unity.Mathematics;

public class FrostField : Skill
{
    [SerializeField]
    private float duration = 2f;
    [SerializeField]
    private GameObject hitboxdef;
    private GameObject hitbox;
    private void Awake()
    {
        base.Awake();
        skillType = SkillType.Summon;
    }
    private void Start()
    {
        Player player = FindObjectOfType<Player>();
        Effect effect = player.getEffectLib().getEffectbyID("Ice");
        effectManager.AddEffect(effect);
        hitbox = Instantiate(hitboxdef, this.transform);
        hitbox.SetActive(false);
        StartCoroutine(Attack());
    }
    private IEnumerator Attack()
    {
        float start = Time.time;
        while (true)
        {
            if (Time.time - start >= duration) break;
            hitbox.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            hitbox.SetActive(false);
            yield return new WaitForSeconds(0.2f);
        }
        Destroy(gameObject);
    }

}