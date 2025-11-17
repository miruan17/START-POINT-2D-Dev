using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    private List<SkillNodeBase> passiveSkillList;
    private EffectLib effectLib;
    private void Awake()
    {
        base.Awake();
        effectLib = new EffectLib();
        passiveSkillList = new List<SkillNodeBase>();
    }
    public void setPassiveSkillList(List<SkillNodeBase> list)
    {
        passiveSkillList = list;
        List<Effect> el = new List<Effect>();
        foreach (var node in passiveSkillList)
        {
            Effect effect = effectLib.getEffectbyID(node.Definition.id);
            effect.identifier = node.Definition.id;
            el.Add(effect);
        }
        argument.setEffectList(el);
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
