using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;

public delegate void EffectDelegate(float value, float term);   //함수 포인터 대용이라 함. 더 공부해봐야 할 듯
public struct effectData
{
    int identifier;         // 식별자
    EffectDelegate effect;  // effect
    Vector<int> argv;       // 매개변수. Vector<> 사용불가. 수정필요.
    float term;             // 시전시간. -1로 선언 시 무제한으로 바꾸는 기능 추가해야함.

    float startTime;
    public effectData(int identifier, EffectDelegate effect, Vector<int> argv, float term)
    {
        this.identifier = identifier;
        this.effect = effect;
        this.argv = argv;
        this.term = term;
        startTime = Time.time;
    }
    public float RemainingTime => startTime + term - Time.time;
    public bool IsExpired => RemainingTime <= 0;
}
public class EffectManager
{
    // Effect Manager
    List<effectData> effectList = new List<effectData>();
    public EffectManager(int identifier, EffectDelegate effect, Vector<int> argv, float term)
    {
        effectData input = new effectData(identifier, effect, argv, term);
        effectList.Add(input);
    }

    public void RuntimeEffect()
    {
        if (effectList.Count == 0) return;
        effectList.RemoveAll(e => e.IsExpired);
    }
}