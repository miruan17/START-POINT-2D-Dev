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
    public bool can_stack = false;
    public int stack = 1;
    public int max_stack = 1;
    public abstract void Runtime();
    public abstract Effect copy();

    public void BindingManager(EffectManager manager)
    {
        this.manager = manager;
    }
    public void DisposeEffect()
    {
        manager = null;
    }


    public void toString()
    {
        Debug.Log(identifier + " " + term + " " + startTime);
    }
}

