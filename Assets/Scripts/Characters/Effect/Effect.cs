using System;
using System.Diagnostics.Contracts;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
public abstract class Effect    //Manager class
{
    public EffectManager manager;
    public String identifier;

    // time
    public float term = 0;
    public float startTime = Time.time;
    public float RemainingTime => startTime + term - Time.time;
    public float chance = 1;
    public bool IsExpired => RemainingTime <= 0;
    public bool enable = false;

    //stack
    public bool can_stack = false;

    public abstract void Runtime();

    public void BindingManager(EffectManager manager)
    {
        this.manager = manager;
    }
    public void DisposeEffect()
    {
        manager = null;
    }
    public virtual void Refresh(Effect effect)
    {
        term = effect.term;
        startTime = Time.time;
        chance = effect.chance;
        enable = true;
    }
    public abstract void updateValue(float term, float dmg, float tick, int max_stack = 1);
    public abstract Effect copy();
    public virtual void upgrade() { }
    public virtual void OnExpired() { enable = false; }
    public void applyDamage(float dmg)
    {
        Character obj = manager.GetCharacter();
        obj.status.CurrentHP -= dmg;
        Debug.Log("남은 HP: " + obj.status.CurrentHP);
    }
}

