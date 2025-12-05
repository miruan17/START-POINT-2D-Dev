using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    private List<SkillNodeBase> passiveSkillList;
    public GameObject jumpVFX;
    public GameObject landingVFX;
    public AudioClip jumpSFX;
    public AudioClip landingSFX;
    public AudioClip levelupSFX;
    public GameObject lightning;

    private int level = 1;
    private int xpMeter = 5;
    private int currentXp = 0;
    private void Awake()
    {
        base.Awake();
        passiveSkillList = new List<SkillNodeBase>();
    }
    public void setPassiveSkillList(List<SkillNodeBase> list)
    {
        passiveSkillList = list;
        List<Effect> el = new List<Effect>();
        foreach (var node in passiveSkillList)
        {
            Effect effect = EffectLib.playerEffectLib.getEffectbyID(node.Definition.tag);
            el.Add(effect);
        }
        argument.setEffectList(el);
    }
    public override void DeathTrigger()
    {
        if (status.CurrentHP <= 0)
        {
            status.CurrentHP = 0;
            if (GetComponent<PlayerAttack>().nowCoroutine != null)
                StopCoroutine(GetComponent<PlayerAttack>().nowCoroutine);
            GetComponent<PlayerAttack>().DieOnAttack();
            Debug.Log(this.name + "Dead");
            gameObject.SetActive(false);
            GameManager.Instance.GameStateManager.ChangeState(new VillageState());
            GameManager.Instance.setPlayer(new Vector2(-30, 20));
            gameObject.SetActive(true);
            status.CurrentHP = status.GetFinal(StatId.HP);
        }
    }

    public void GetXp()
    {
        currentXp++;
        if (xpMeter <= currentXp)
        {
            currentXp -= 5;
            level++;
            AudioManager.Instance.PlaySFX(levelupSFX);
            FindObjectOfType<SkillTreeManager>(true).AddPoints(2);
            Debug.Log("Level Up");
        }
    }
    public int GetLevel()
    {
        return level;
    }
}
