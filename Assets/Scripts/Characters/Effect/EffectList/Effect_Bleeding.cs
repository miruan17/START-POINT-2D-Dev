using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum StatId_Effect_Bleeding{BLD, BLD_tick, BLD_stack, EXC_BLD}
public class Effect_Bleeding : Effect   //Manager class
{
    private readonly Dictionary<StatId_Effect_Bleeding, Status> _stats = new();
    public Effect_Bleeding()
    {
        _stats[StatId_Effect_Bleeding.BLD] = new Status(0);
        _stats[StatId_Effect_Bleeding.BLD_tick] = new Status(0);
        _stats[StatId_Effect_Bleeding.BLD_stack] = new Status(0);
        _stats[StatId_Effect_Bleeding.EXC_BLD] = new Status(0);
    }
    public override void Runtime()
    {
        
    }
    public override void Remove()
    {
        
    }
}
