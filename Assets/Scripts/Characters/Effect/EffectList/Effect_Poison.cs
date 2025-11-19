using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using UnityEngine;
public enum StatId_Effect_Poison { Pos, Pos_tick, Pos_stack, Pos_term } // dmg tick max_stack term
public class Effect_Poison : Effect   //Manager class
{
    private Dictionary<StatId_Effect_Poison, Status> _stats = new();

    private readonly float[,] upgradeValue = { { 0, 1, 0, 0 }, { 2, 0, 0, 0 } };
    private float prevTime = 0;
    private int level = 0;
    private int stack = 0;
    private bool weaken = false;
    public Effect_Poison(float term, float dmg, float tick, int max_stack)
    {
        probability = 0.4f;
        can_stack = true;
        _stats[StatId_Effect_Poison.Pos] = new Status(dmg);
        _stats[StatId_Effect_Poison.Pos_tick] = new Status(tick);
        _stats[StatId_Effect_Poison.Pos_stack] = new Status(max_stack);
        _stats[StatId_Effect_Poison.Pos_term] = new Status(term);
    }
    public override void Runtime()
    {
        if (_stats[StatId_Effect_Poison.Pos_tick].Get() <= 0) return;
        if (stack >= _stats[StatId_Effect_Poison.Pos_stack].Get())
        {

            if (!weaken)
            {
                Debug.Log("취약 적용중");
                enableWeaken();
                weaken = true;
            }
        }
        else
        {
            if (weaken)
            {
                disableWeaken();
                weaken = false;
            }
        }
        if (Time.time - prevTime >= _stats[StatId_Effect_Poison.Pos_tick].Get())
        {
            prevTime = Time.time;
            Debug.Log(level + 1 + "단계 중독 틱 발생. 스택: " + stack);
            applyDamage(_stats[StatId_Effect_Poison.Pos].Get() * stack);
        }
    }
    public override void Refresh(Effect effect)
    {
        base.Refresh(effect);
        Effect_Poison eff = (Effect_Poison)effect;
        _stats = eff._stats;
        if (stack < _stats[StatId_Effect_Poison.Pos_stack].Get()) stack++;
        term = _stats[StatId_Effect_Poison.Pos_term].Get();
        level = eff.level;
    }
    public override void updateValue(float term, float dmg, float tick, int max_stack)
    {
        _stats[StatId_Effect_Poison.Pos].SetDefaultValue(_stats[StatId_Effect_Poison.Pos].getBase() + dmg);
        _stats[StatId_Effect_Poison.Pos_tick].SetDefaultValue(_stats[StatId_Effect_Poison.Pos_tick].getBase() + tick);
        _stats[StatId_Effect_Poison.Pos_stack].SetDefaultValue((int)_stats[StatId_Effect_Poison.Pos_stack].getBase() + max_stack);
        _stats[StatId_Effect_Poison.Pos_term].SetDefaultValue(_stats[StatId_Effect_Poison.Pos_term].getBase() + term);
        this.term = _stats[StatId_Effect_Poison.Pos_term].Get();
    }
    public override void upgrade()
    {
        //호출 횟수(level)에 따른 값 변화
        updateValue(upgradeValue[level, 0], upgradeValue[level, 1], upgradeValue[level, 2], (int)upgradeValue[level, 3]);
        level++;
    }
    public override Effect copy()
    {
        Effect effect = new Effect_Poison(_stats[StatId_Effect_Poison.Pos_term].getBase(), _stats[StatId_Effect_Poison.Pos].getBase(), _stats[StatId_Effect_Poison.Pos_tick].getBase(), (int)_stats[StatId_Effect_Poison.Pos_stack].getBase());
        effect.identifier = identifier;
        return effect;
    }
    public void enableWeaken()
    {
        Character obj = manager.GetCharacter();
        obj.status.Mul(StatId.ATK, identifier, -0.15f);
        obj.status.Mul(StatId.DEF, identifier, -0.2f);
    }
    public void disableWeaken()
    {
        Character obj = manager.GetCharacter();
        obj.status.RemoveBySource(identifier);
    }
    public override void OnExpired()
    {
        if (weaken)
        {
            disableWeaken();
            weaken = false;
        }
        stack = 0;
    }
    public void applyDamage(float dmg)
    {
        Character obj = manager.GetCharacter();
        obj.status.CurrentHP -= dmg;
        Debug.Log("남은 HP: " + obj.status.CurrentHP);
    }
}
