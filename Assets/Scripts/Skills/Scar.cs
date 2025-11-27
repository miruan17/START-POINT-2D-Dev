using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Unity.Mathematics;

public class Scar : Skill
{
    [SerializeField]
    private GameObject hitboxdef;
    private GameObject hitbox;
    private void Awake()
    {
        base.Awake();
        skillType = SkillType.Attack;
        dmg = 4;
    }
    private void Start()
    {
        Player player = FindObjectOfType<Player>();
        Effect effect = EffectLib.playerEffectLib.getEffectbyID("Bleeding");
        effectManager.AddEffect(effect);
        hitbox = Instantiate(hitboxdef, this.transform);
        hitbox.SetActive(false);
        StartCoroutine(Attack());
    }
    private IEnumerator Attack()
    {
        for (int i = 0; i < 4; i++)
        {
            Debug.Log("Scar");
            hitbox.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            hitbox.SetActive(false);
            yield return new WaitForSeconds(0.1f);
        }
        Destroy(gameObject);
    }

}