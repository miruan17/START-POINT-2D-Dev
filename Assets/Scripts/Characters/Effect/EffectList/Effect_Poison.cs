using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
public enum StatId_Effect_Poison { Pos, Pos_tick, Pos_stack, EXC_Pos }
public class Effect_Poison : Effect   //Manager class
{
    private Dictionary<StatId_Effect_Poison, Status> _stats = new();
    private float prevTime = 0;
    public Effect_Poison(float term, float dmg, float tick, int max_stack, bool is_promotion)
    {
        this.term = term;
        this.max_stack = max_stack;
        _stats[StatId_Effect_Poison.Pos] = new Status(dmg);
        _stats[StatId_Effect_Poison.Pos_tick] = new Status(tick);
        _stats[StatId_Effect_Poison.Pos_stack] = new Status(stack);
        _stats[StatId_Effect_Poison.EXC_Pos] = new Status(is_promotion ? 1 : 0);
    }
    public override void Runtime()
    {
        if (_stats[StatId_Effect_Poison.Pos_tick].Get() <= 0) return;
        if (Time.time - prevTime >= _stats[StatId_Effect_Poison.Pos_tick].Get())
        {
            prevTime = Time.time;
            Debug.Log("중독 틱 발생. 스택: " + stack);
            Character obj = manager.GetCharacter();
            obj.status.CurrentHP -= _stats[StatId_Effect_Poison.Pos].Get() * stack;
            Debug.Log("남은 HP: " + obj.status.CurrentHP);
        }
    }
    public override void Refresh(Effect effect)
    {
        base.Refresh(effect);
        Effect_Poison eff = (Effect_Poison)effect;
        if (stack < max_stack) stack++;
        _stats = eff._stats;
        max_stack = eff.max_stack;
    }

}
