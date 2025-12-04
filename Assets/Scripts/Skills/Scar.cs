using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.InputSystem;

public class Scar : Skill
{
    [SerializeField]
    private GameObject hitboxdef;
    private GameObject hitbox;
    PlayerInputHub input;
    private void Awake()
    {
        base.Awake();
    }
    public Scar()
    {
        skillType = SkillType.Attack;
        dmg = 4;
    }
    private void Start()
    {
        input = GetComponentInParent<PlayerInputHub>();
        Effect effect = EffectLib.playerEffectLib.getEffectbyID("Bleeding");
        effectManager.AddEffect(effect);
        hitbox = Instantiate(hitboxdef, this.transform);
        hitbox.SetActive(false);
        hitbox.GetComponent<Hitbox>().PlayVFX(spawnVFX, 0.5f);
        hitbox.GetComponent<Hitbox>().hitVFX = hitVFX;
        hitbox.SetActive(true);
        StartCoroutine(Attack());
    }
    private IEnumerator Attack()
    {
        input.flip = false;
        for (int i = 0; i < 5; i++)
        {
            hitbox.SetActive(true);
            yield return new WaitForSeconds(0.05f);
            hitbox.SetActive(false);
            yield return new WaitForSeconds(0.05f);
        }
        input.flip = true;
        Destroy(gameObject);
    }

}