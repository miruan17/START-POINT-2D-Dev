using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
public enum StatId_Effect_Lightning { LN, LN_Chain, LN_Range } // dmg, tick, max_stack, term
public class Effect_Lightning : Effect   //Manager class
{
    private Dictionary<StatId_Effect_Lightning, Status> _stats = new();
    private int level = 0;
    public Effect_Lightning(float dmg, int chain, float range)
    {
        chance = 0.3f;
        _stats[StatId_Effect_Lightning.LN] = new Status(dmg);
        _stats[StatId_Effect_Lightning.LN_Chain] = new Status(chain);
        _stats[StatId_Effect_Lightning.LN_Range] = new Status(range);
    }

    public override void Runtime()
    {
        if (term <= 0) return;
        var caster = manager.GetCharacter();
        Vector2 casterPos = caster.transform.position;

        var enemies =
            Enemy.AllEnemies
            .OrderBy(e => Vector2.Distance(casterPos, e.transform.position));

        float chain = _stats[StatId_Effect_Lightning.LN_Chain].Get();
        int cnt = 0;
        foreach (var enemy in enemies)
        {
            if (chain <= 0 || _stats[StatId_Effect_Lightning.LN_Range].Get() < Vector2.Distance(casterPos, enemy.transform.position) || cnt >= chain) break;
            cnt++;
        }
        float dmg = _stats[StatId_Effect_Lightning.LN].Get() * chain / (cnt == 0 ? 1 : cnt);
        foreach (var enemy in enemies)
        {
            if (cnt-- <= 0) break;
            enemy.status.CurrentHP -= dmg;
            Debug.Log(enemy.name + "에게 전격데미지 " + dmg + "발생.");
        }
        term = 0;
    }

    public override void Refresh(Effect effect)
    {
        base.Refresh(effect);
        Effect_Lightning eff = (Effect_Lightning)effect;
        _stats = eff._stats; // 해당 구조를 통해 모든 Effect_Bleeding객체는 항상 player의 EffectLib속 Effect_Bleeding 객체의 _stat을 참조
        level = eff.level;
        term = 1;
    }
    public override void updateValue(float term, float dmg, float tick, int max_stack) // 기존 base를 변경하는 연산
    {
        _stats[StatId_Effect_Lightning.LN].SetDefaultValue(_stats[StatId_Effect_Lightning.LN].getBase() + dmg);
    }
    public override void upgrade()
    {
        //호출 횟수(level)에 따른 값 변화. 레벨 많아야 3~4개이므로 하드코딩이 나을듯
        level++;
        if (level == 1)
        {
            _stats[StatId_Effect_Lightning.LN].SetDefaultValue(_stats[StatId_Effect_Lightning.LN].getBase() + 2);
            _stats[StatId_Effect_Lightning.LN_Chain].SetDefaultValue(_stats[StatId_Effect_Lightning.LN_Chain].getBase() + 2);
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
        Effect_Lightning effect = new Effect_Lightning(_stats[StatId_Effect_Lightning.LN].getBase(), (int)_stats[StatId_Effect_Lightning.LN_Chain].getBase(), _stats[StatId_Effect_Lightning.LN_Range].getBase());
        effect.identifier = identifier;
        return effect;
    }
}
