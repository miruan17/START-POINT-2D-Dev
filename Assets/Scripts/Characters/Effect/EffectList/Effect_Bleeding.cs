using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
public enum StatId_Effect_Bleeding { BLD, BLD_tick, BLD_stack, EXC_BLD }
public class Effect_Bleeding : Effect   //Manager class
{
    private Dictionary<StatId_Effect_Bleeding, Status> _stats = new();
    private float prevTime = 0;
    public Effect_Bleeding(float term, float dmg, float tick, int max_stack, bool is_promotion)
    {
        updateValue(term, dmg, tick, max_stack, is_promotion);
    }

    public override void Runtime()
    {
        if (_stats[StatId_Effect_Bleeding.BLD_tick].Get() <= 0) return;
        if (stack == max_stack && is_promotion)
        {
            Debug.Log("과다출혈!!!");
            stack = 0;
        }
        if (Time.time - prevTime >= _stats[StatId_Effect_Bleeding.BLD_tick].Get())
        {
            prevTime = Time.time;
            Debug.Log("출혈 틱 발생. 스택: " + stack);
            Character obj = manager.GetCharacter();
            obj.status.CurrentHP -= _stats[StatId_Effect_Bleeding.BLD].Get() * stack;
            Debug.Log("남은 HP: " + obj.status.CurrentHP);
        }
    }
    public override void Refresh(Effect effect)
    {
        base.Refresh(effect);
        Effect_Bleeding eff = (Effect_Bleeding)effect;
        if (stack < max_stack) stack++;
        _stats = eff._stats;
        max_stack = eff.max_stack;
    }
    public override void updateValue(float term, float dmg, float tick, int max_stack, bool is_promotion)
    {
        this.term = term;
        this.max_stack = max_stack;
        _stats[StatId_Effect_Bleeding.BLD] = new Status(dmg);
        _stats[StatId_Effect_Bleeding.BLD_tick] = new Status(1f);
        _stats[StatId_Effect_Bleeding.BLD_stack] = new Status(stack);
        _stats[StatId_Effect_Bleeding.EXC_BLD] = new Status(is_promotion ? 1 : 0);
    }
}
