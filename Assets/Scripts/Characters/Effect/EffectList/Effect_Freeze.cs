using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
public enum StatId_Effect_Freeze { FRZ, FRZ_term } // dmg, term
public class Effect_Freeze : Effect   //Manager class
{
    private Dictionary<StatId_Effect_Freeze, Status> _stats = new();
    int level = 0;
    public Effect_Freeze(float term, float dmg)
    {
        _stats[StatId_Effect_Freeze.FRZ] = new Status(dmg);
        _stats[StatId_Effect_Freeze.FRZ_term] = new Status(term);
    }

    public override void Runtime()
    {

    }
    public override void Refresh(Effect effect)
    {
        base.Refresh(effect);
        Effect_Freeze eff = (Effect_Freeze)effect;
        _stats = eff._stats; // 해당 구조를 통해 모든 Effect_Bleeding객체는 항상 player의 EffectLib속 Effect_Bleeding 객체의 _stat을 참조
        term = _stats[StatId_Effect_Freeze.FRZ_term].Get();
        level = eff.level;
        Debug.Log("in Freeze: " + manager);
        manager.GetCharacter().is_Freeze = true;
    }
    public override void updateValue(float term, float dmg, float tick, int max_stack) // 기존 base를 변경하는 연산
    {
    }
    public override void upgrade()
    {
        //호출 횟수(level)에 따른 값 변화. 레벨 많아야 3~4개이므로 하드코딩이 나을듯
        level++;
        if (level == 1)
        {
            _stats[StatId_Effect_Freeze.FRZ].SetDefaultValue(_stats[StatId_Effect_Freeze.FRZ].getBase() + 5);
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
        Effect_Freeze effect = new Effect_Freeze(_stats[StatId_Effect_Freeze.FRZ_term].getBase(), _stats[StatId_Effect_Freeze.FRZ].getBase());
        effect.identifier = identifier;
        return effect;
    }
    public override void OnExpired()
    {
        base.OnExpired();
        manager.GetCharacter().is_Freeze = false;
        Debug.Log("out Freeze");
    }
    public float getDmg()
    {
        return _stats[StatId_Effect_Freeze.FRZ].Get();
    }

}
