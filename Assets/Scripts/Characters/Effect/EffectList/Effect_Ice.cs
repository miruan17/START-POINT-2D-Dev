using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using UnityEngine;
public enum StatId_Effect_Ice { Ice, Ice_stack, Ice_term, FRZ_slow, FRZ_Aslow } // dmg tick max_stack term
public class Effect_Ice : Effect   //Manager class
{
    private Dictionary<StatId_Effect_Ice, Status> _stats = new();
    private int level = 0;
    private int stack = 0;
    private float prevslow;
    private float prevAslow;
    private bool weaken = false;
    public Effect_Ice(float term, float dmg, int max_stack)
    {
        chance = 0.3f;
        can_stack = true;
        _stats[StatId_Effect_Ice.Ice] = new Status(dmg);
        _stats[StatId_Effect_Ice.Ice_stack] = new Status(max_stack);
        _stats[StatId_Effect_Ice.Ice_term] = new Status(term);
        _stats[StatId_Effect_Ice.FRZ_slow] = new Status(0.3f);
        _stats[StatId_Effect_Ice.FRZ_Aslow] = new Status(0);
    }
    public override void Runtime()
    {
        if (!weaken)
        {
            weaken = true;
            enableWeaken();
        }
        else
        {
            float nowslow = _stats[StatId_Effect_Ice.FRZ_slow].Get();
            float nowAslow = _stats[StatId_Effect_Ice.FRZ_Aslow].Get();
            if (prevslow != nowslow || prevAslow != nowAslow)
            {
                disableWeaken();
                enableWeaken();
            }
            prevslow = nowslow;
            prevAslow = nowAslow;
        }
    }
    public override void Refresh(Effect effect)
    {
        base.Refresh(effect);
        Effect_Ice eff = (Effect_Ice)effect;
        _stats = eff._stats;
        if (stack < _stats[StatId_Effect_Ice.Ice_stack].Get()) stack++;
        term = _stats[StatId_Effect_Ice.Ice_term].Get();
        level = eff.level;
        if (stack == _stats[StatId_Effect_Ice.Ice_stack].Get()) applyDamage(_stats[StatId_Effect_Ice.Ice].Get());
        prevAslow = _stats[StatId_Effect_Ice.FRZ_Aslow].Get();
        prevslow = _stats[StatId_Effect_Ice.FRZ_slow].Get();
    }
    public override void updateValue(float term, float dmg, float tick, int max_stack)
    {
        _stats[StatId_Effect_Ice.Ice].SetDefaultValue(_stats[StatId_Effect_Ice.Ice].getBase() + dmg);
        _stats[StatId_Effect_Ice.Ice_stack].SetDefaultValue((int)_stats[StatId_Effect_Ice.Ice_stack].getBase() + max_stack);
        _stats[StatId_Effect_Ice.Ice_term].SetDefaultValue(_stats[StatId_Effect_Ice.Ice_term].getBase() + term);
        this.term = _stats[StatId_Effect_Ice.Ice_term].Get();
    }
    public override void upgrade()
    {
        //호출 횟수(level)에 따른 값 변화. 레벨 많아야 3~4개이므로 하드코딩이 나을듯
        level++;
        if (level == 1)
        {
            // 빙결 + 1: 빙결 적 데미지 5 증가 및 빙결 상태 적 공속 20% 감소
            _stats[StatId_Effect_Ice.Ice].SetDefaultValue(_stats[StatId_Effect_Ice.Ice].getBase() + 5);
            _stats[StatId_Effect_Ice.FRZ_Aslow].SetDefaultValue(_stats[StatId_Effect_Ice.FRZ_Aslow].getBase() + 0.2f);
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
        Effect_Ice effect = new Effect_Ice(_stats[StatId_Effect_Ice.Ice_term].getBase(), _stats[StatId_Effect_Ice.Ice].getBase(), (int)_stats[StatId_Effect_Ice.Ice_stack].getBase());
        effect.identifier = identifier;
        effect.chance = chance;
        return effect;
    }
    public void enableWeaken()
    {
        Debug.Log("Weaken 활성화");
        Character obj = manager.GetCharacter();
        obj.status.Mul(StatId.SPD, identifier, -_stats[StatId_Effect_Ice.FRZ_slow].Get());
        obj.status.Mul(StatId.APS, identifier, -_stats[StatId_Effect_Ice.FRZ_Aslow].Get());
    }
    public void disableWeaken()
    {
        Debug.Log("Weaken 해제");
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
