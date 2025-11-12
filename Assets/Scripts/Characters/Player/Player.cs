using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    private List<SkillNodeBase> passiveSkillList;
    private void Awake()
    {
        base.Awake();
        passiveSkillList = new List<SkillNodeBase>();
    }
    public void setPassiveSkillList(List<SkillNodeBase> list)
    {
        passiveSkillList = list;
        List<Effect> effectList = new List<Effect>();
        foreach (var node in list)
        {
            ArgumentBase arg = node.Definition.argument;
            arg.setActive();
            effectList.Add(arg.effect);
        }
        argument.setEffectList(effectList);
    }
    public override void DeathTrigger()
    {
        if (status.CurrentHP <= 0)
        {
            status.CurrentHP = 0;
            Debug.Log(this.name + "Dead");
            gameObject.SetActive(false);
        }
    }
}
