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
    }
    public FrostField()
    {
        skillType = SkillType.Summon;
    }
    private void Start()
    {
        Player player = FindObjectOfType<Player>();
        Effect effect = EffectLib.playerEffectLib.getEffectbyID("Ice");
        effectManager.AddEffect(effect);
        hitbox = Instantiate(hitboxdef, this.transform);
        hitbox.SetActive(false);
        hitbox.SetActive(true);
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
            yield return new WaitForSeconds(0.7f);
        }
        Destroy(gameObject);
    }

}