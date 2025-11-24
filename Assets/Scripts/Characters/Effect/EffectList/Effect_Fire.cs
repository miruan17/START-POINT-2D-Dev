using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
public enum StatId_Effect_Fire { FIRE, FIRE_tick, FIRE_term } // dmg, tick, term
public class Effect_Fire : Effect   //Manager class
{
    private Dictionary<StatId_Effect_Fire, Status> _stats = new();
    private float prevTime = 0;
    private int level = 0;
    public Effect_Fire(float term, float dmg, float tick)
    {
        chance = 0.4f;
        _stats[StatId_Effect_Fire.FIRE] = new Status(dmg);
        _stats[StatId_Effect_Fire.FIRE_tick] = new Status(tick);
        _stats[StatId_Effect_Fire.FIRE_term] = new Status(term);
    }

    public override void Runtime()
    {
        if (_stats[StatId_Effect_Fire.FIRE_tick].Get() <= 0) return;
        if (Time.time - prevTime >= _stats[StatId_Effect_Fire.FIRE_tick].Get())
        {
            prevTime = Time.time;
            Debug.Log(level + 1 + "단계 화염 틱 발생.");
            applyDamage(_stats[StatId_Effect_Fire.FIRE].Get());
        }
    }
    public override void Refresh(Effect effect)
    {
        base.Refresh(effect);
        Effect_Fire eff = (Effect_Fire)effect;
        _stats = eff._stats; // 해당 구조를 통해 모든 Effect_Bleeding객체는 항상 player의 EffectLib속 Effect_Bleeding 객체의 _stat을 참조
        term = _stats[StatId_Effect_Fire.FIRE_term].Get();
        chance = effect.chance;
        level = eff.level;
    }
    public override void updateValue(float term, float dmg, float tick, int max_stack) // 기존 base를 변경하는 연산
    {
        _stats[StatId_Effect_Fire.FIRE].SetDefaultValue(_stats[StatId_Effect_Fire.FIRE].getBase() + dmg);
        _stats[StatId_Effect_Fire.FIRE_tick].SetDefaultValue(_stats[StatId_Effect_Fire.FIRE_tick].getBase() + tick);
        _stats[StatId_Effect_Fire.FIRE_term].SetDefaultValue(_stats[StatId_Effect_Fire.FIRE_term].getBase() + term);
        this.term = _stats[StatId_Effect_Fire.FIRE_term].Get();
    }
    public override void upgrade()
    {
        //호출 횟수(level)에 따른 값 변화. 레벨 많아야 3~4개이므로 하드코딩이 나을듯
        level++;
        if (level == 1)
        {
            _stats[StatId_Effect_Fire.FIRE].SetDefaultValue(_stats[StatId_Effect_Fire.FIRE].getBase() + 2);
            _stats[StatId_Effect_Fire.FIRE_tick].SetDefaultValue(_stats[StatId_Effect_Fire.FIRE_tick].getBase() - 0.5f);
            chance += 0.05f;
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
        Effect_Fire effect = new Effect_Fire(_stats[StatId_Effect_Fire.FIRE_term].getBase(), _stats[StatId_Effect_Fire.FIRE].getBase(), _stats[StatId_Effect_Fire.FIRE_tick].getBase());
        effect.identifier = identifier;
        effect.chance = chance;
        return effect;
    }
    public override void OnExpired()
    {
    }


}
