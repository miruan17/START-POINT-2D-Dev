using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using UnityEngine;
public enum StatId_Effect_Ice { Ice, Ice_stack, Ice_term } // dmg tick max_stack term
public class Effect_Ice : Effect   //Manager class
{
    private Dictionary<StatId_Effect_Ice, Status> _stats = new();

    private readonly float[,] upgradeValue = { { 0, 1, 0, 0 }, { 2, 0, 0, 0 } };
    private float prevTime = 0;
    private float slow = -0.3f;
    private int level = 0;
    private int stack = 0;
    private bool weaken = false;
    public Effect_Ice(float term, float dmg, int max_stack)
    {
        probability = 0.4f;
        can_stack = true;
        _stats[StatId_Effect_Ice.Ice] = new Status(dmg);
        _stats[StatId_Effect_Ice.Ice_stack] = new Status(max_stack);
        _stats[StatId_Effect_Ice.Ice_term] = new Status(term);
    }
    public override void Runtime()
    {
        if (!weaken)
        {
            weaken = true;
            enableWeaken();
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
        //호출 횟수(level)에 따른 값 변화
        updateValue(upgradeValue[level, 0], upgradeValue[level, 1], upgradeValue[level, 2], (int)upgradeValue[level, 3]);
        level++;
    }
    public override Effect copy()
    {
        Effect effect = new Effect_Ice(_stats[StatId_Effect_Ice.Ice_term].getBase(), _stats[StatId_Effect_Ice.Ice].getBase(), (int)_stats[StatId_Effect_Ice.Ice_stack].getBase());
        effect.identifier = identifier;
        return effect;
    }
    public void enableWeaken()
    {
        Character obj = manager.GetCharacter();
        obj.status.Mul(StatId.SPD, identifier, slow);
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
