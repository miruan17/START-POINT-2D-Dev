using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
public enum StatId_Effect_Bleeding { BLD, BLD_tick, BLD_stack, EXC_BLD }
public class Effect_Bleeding : Effect   //Manager class
{
    private readonly Dictionary<StatId_Effect_Bleeding, Status> _stats = new();
    private float prevTime = 0;
    public Effect_Bleeding()
    {
        _stats[StatId_Effect_Bleeding.BLD] = new Status(0);
        _stats[StatId_Effect_Bleeding.BLD_tick] = new Status(0.5f);
        _stats[StatId_Effect_Bleeding.BLD_stack] = new Status(0);
        _stats[StatId_Effect_Bleeding.EXC_BLD] = new Status(0);
    }
    public override void Runtime()
    {
        if (_stats[StatId_Effect_Bleeding.BLD_tick].Get() <= 0) return;
        if (Time.time - prevTime >= _stats[StatId_Effect_Bleeding.BLD_tick].Get())
        {
            prevTime = Time.time;
            Debug.Log("출혈 틱 발생");
        }
    }

    // EffectManager에 자신 제거요청

    public override Effect copy()
    {
        Effect_Bleeding newEffect = new Effect_Bleeding();
        newEffect.identifier = identifier;
        newEffect.term = term;

        return newEffect;
    }

}
