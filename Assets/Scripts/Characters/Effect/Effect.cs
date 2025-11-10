using System;
using System.Diagnostics.Contracts;
using JetBrains.Annotations;
using UnityEngine;
public abstract class Effect    //Manager class
{
    public String identifier;

    // time
    public float term;
    public float startTime = Time.time;
    public float RemainingTime => startTime + term - Time.time;
    public bool IsExpired => RemainingTime <= 0;
    
    public abstract void Runtime();
    public abstract void Remove();
}

