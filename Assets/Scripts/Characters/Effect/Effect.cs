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
    public bool IsExpired => RemainingTime <= 0;

    //stack
    public bool can_stack = false;
    public int stack = 0;
    public int max_stack = 1;

    //promotion
    public bool is_promotion = false;

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
    }
}

