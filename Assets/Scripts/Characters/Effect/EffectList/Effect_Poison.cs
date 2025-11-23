using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using UnityEngine;
public enum StatId_Effect_Poison { Pos, Pos_tick, Pos_stack, Pos_term, Weak_ATK, Weak_DEF } // dmg tick max_stack term
public class Effect_Poison : Effect   //Manager class
{
    private Dictionary<StatId_Effect_Poison, Status> _stats = new();
    private float prevTime = 0;
    private int level = 0;
    private int stack = 0;
    private float prevATK;
    private float prevDEF;
    private bool weaken = false;
    public Effect_Poison(float term, float dmg, float tick, int max_stack)
    {
        can_stack = true;
        _stats[StatId_Effect_Poison.Pos] = new Status(dmg);
        _stats[StatId_Effect_Poison.Pos_tick] = new Status(tick);
        _stats[StatId_Effect_Poison.Pos_stack] = new Status(max_stack);
        _stats[StatId_Effect_Poison.Pos_term] = new Status(term);
        _stats[StatId_Effect_Poison.Weak_ATK] = new Status(0.15f);
        _stats[StatId_Effect_Poison.Weak_DEF] = new Status(0.2f);
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
            else
            {
                float nowATK = _stats[StatId_Effect_Poison.Weak_ATK].Get();
                float nowDEF = _stats[StatId_Effect_Poison.Weak_DEF].Get();
                if (prevATK != nowATK || prevDEF != nowDEF)
                {
                    disableWeaken();
                    enableWeaken();
                }
                prevATK = nowATK;
                prevDEF = nowDEF;
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
        prevATK = _stats[StatId_Effect_Poison.Weak_ATK].Get();
        prevDEF = _stats[StatId_Effect_Poison.Weak_DEF].Get();
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
        //호출 횟수(level)에 따른 값 변화. 레벨 많아야 3~4개이므로 하드코딩이 나을듯
        level++;
        if (level == 1)
        {
            //취약 + 1: 공깎 15% -> 18%, 방깎 20% -> 22%, 틱뎀 +1, 유지시간 +2초
            _stats[StatId_Effect_Poison.Weak_ATK].SetDefaultValue(_stats[StatId_Effect_Poison.Weak_ATK].getBase() + 0.03f);
            _stats[StatId_Effect_Poison.Weak_DEF].SetDefaultValue(_stats[StatId_Effect_Poison.Weak_DEF].getBase() + 0.02f);
            _stats[StatId_Effect_Poison.Pos].SetDefaultValue(_stats[StatId_Effect_Poison.Pos].getBase() + 1);
            _stats[StatId_Effect_Poison.Pos_term].SetDefaultValue(_stats[StatId_Effect_Poison.Pos_term].getBase() + 2);
        }
        if (level == 2)
        {
        }
        if (level == 3)
        {

        }
    }
    public override Effect copy()
    {
        Effect_Poison effect = new Effect_Poison(_stats[StatId_Effect_Poison.Pos_term].getBase(), _stats[StatId_Effect_Poison.Pos].getBase(), _stats[StatId_Effect_Poison.Pos_tick].getBase(), (int)_stats[StatId_Effect_Poison.Pos_stack].getBase());
        effect.identifier = identifier;
        return effect;
    }
    public void enableWeaken()
    {
        Character obj = manager.GetCharacter();
        obj.status.Mul(StatId.ATK, identifier, -_stats[StatId_Effect_Poison.Weak_ATK].Get());
        obj.status.Mul(StatId.DEF, identifier, -_stats[StatId_Effect_Poison.Weak_DEF].Get());
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
}
