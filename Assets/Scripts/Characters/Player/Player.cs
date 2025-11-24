using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    private List<SkillNodeBase> passiveSkillList;
    private EffectLib effectLib;
    public PlayerState State { get; private set; } = PlayerState.Normal;
    public void SetState(PlayerState s) => State = s;

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
            Effect effect = effectLib.getEffectbyID(node.Definition.tag);
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
    public EffectLib getEffectLib() { return effectLib; }
}
