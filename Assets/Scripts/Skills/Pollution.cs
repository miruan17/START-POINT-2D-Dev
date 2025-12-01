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
    private void Awake()
    {
        base.Awake();
    }
    public Pollution()
    {
        skillType = SkillType.Summon;
    }
    private void Start()
    {
        Player player = FindObjectOfType<Player>();
        Effect effect = EffectLib.playerEffectLib.getEffectbyID("Poison");
        effectManager.AddEffect(effect);
        hitbox = Instantiate(hitboxdef, this.transform);
        hitbox.SetActive(false);
        hitbox.GetComponent<Hitbox>().PlayVFX(spawnVFX, 1);
        hitbox.GetComponent<Hitbox>().hitVFX = hitVFX;
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
            yield return new WaitForSeconds(0.9f);
        }
        Destroy(gameObject);
    }

}