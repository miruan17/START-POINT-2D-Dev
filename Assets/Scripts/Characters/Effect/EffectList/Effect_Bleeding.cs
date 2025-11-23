using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
public enum StatId_Effect_Bleeding { BLD, BLD_tick, BLD_stack, BLD_term, EXE_BLD } // dmg, tick, max_stack, term
public class Effect_Bleeding : Effect   //Manager class
{
    private Dictionary<StatId_Effect_Bleeding, Status> _stats = new();
    private float prevTime = 0;
    private int level = 0;
    private int stack = 0;
    public Effect_Bleeding(float term, float dmg, float tick, int max_stack)
    {
        can_stack = true;
        _stats[StatId_Effect_Bleeding.BLD] = new Status(dmg);
        _stats[StatId_Effect_Bleeding.BLD_tick] = new Status(tick);
        _stats[StatId_Effect_Bleeding.BLD_stack] = new Status(max_stack);
        _stats[StatId_Effect_Bleeding.BLD_term] = new Status(term);
        _stats[StatId_Effect_Bleeding.EXE_BLD] = new Status(0.1f);
    }

    public override void Runtime()
    {
        if (_stats[StatId_Effect_Bleeding.BLD_tick].Get() <= 0) return;
        if (stack > _stats[StatId_Effect_Bleeding.BLD_stack].Get())
        {
            Character obj = manager.GetCharacter();
            Debug.Log("과다출혈!!!");
            applyDamage(obj.status.GetFinal(StatId.HP) * _stats[StatId_Effect_Bleeding.EXE_BLD].Get());
            enable = false;
            return;
        }
        if (Time.time - prevTime >= _stats[StatId_Effect_Bleeding.BLD_tick].Get())
        {
            prevTime = Time.time;
            Debug.Log(level + 1 + "단계 출혈 틱 발생. 스택: " + stack);
            applyDamage(_stats[StatId_Effect_Bleeding.BLD].Get() * stack);
        }
    }
    public override void Refresh(Effect effect)
    {
        base.Refresh(effect);
        Effect_Bleeding eff = (Effect_Bleeding)effect;
        _stats = eff._stats; // 해당 구조를 통해 모든 Effect_Bleeding객체는 항상 player의 EffectLib속 Effect_Bleeding 객체의 _stat을 참조 
        if (stack <= _stats[StatId_Effect_Bleeding.BLD_stack].Get()) stack++;
        term = _stats[StatId_Effect_Bleeding.BLD_term].Get();
        level = eff.level;
    }
    public override void updateValue(float term, float dmg, float tick, int max_stack) // 기존 base를 변경하는 연산
    {
        _stats[StatId_Effect_Bleeding.BLD].SetDefaultValue(_stats[StatId_Effect_Bleeding.BLD].getBase() + dmg);
        _stats[StatId_Effect_Bleeding.BLD_tick].SetDefaultValue(_stats[StatId_Effect_Bleeding.BLD_tick].getBase() + tick);
        _stats[StatId_Effect_Bleeding.BLD_stack].SetDefaultValue((int)_stats[StatId_Effect_Bleeding.BLD_stack].getBase() + max_stack);
        _stats[StatId_Effect_Bleeding.BLD_term].SetDefaultValue(_stats[StatId_Effect_Bleeding.BLD_term].getBase() + term);
        this.term = _stats[StatId_Effect_Bleeding.BLD_term].Get();
    }
    public override void upgrade()
    {
        //호출 횟수(level)에 따른 값 변화. 레벨 많아야 3~4개이므로 하드코딩이 나을듯
        level++;
        if (level == 1)
        {
            // 과다출혈 + 1: hp 10% -> 11%
            _stats[StatId_Effect_Bleeding.EXE_BLD].SetDefaultValue(_stats[StatId_Effect_Bleeding.EXE_BLD].getBase() + 0.01f);
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
        Effect_Bleeding effect = new Effect_Bleeding(_stats[StatId_Effect_Bleeding.BLD_term].getBase(), _stats[StatId_Effect_Bleeding.BLD].getBase(), _stats[StatId_Effect_Bleeding.BLD_tick].getBase(), (int)_stats[StatId_Effect_Bleeding.BLD_stack].getBase());
        effect.identifier = identifier;
        return effect;
    }
    public override void OnExpired()
    {
        stack = 0;
    }
}
