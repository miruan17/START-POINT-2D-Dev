using System;
using System.Collections.Generic;
using UnityEngine;
public enum StatId { HP, SP, ATK, APS, DEF, SPD }
public sealed class CharacterStatusManager
{
    // Public resource(readOnly)
    public float CurrentHP { get; private set; }
    public float CurrentSP { get; private set; }

    // Private resource
    private readonly Dictionary<StatId, Status> _stats = new();
    private readonly List<TimedEntry> _timed = new();

    private struct TimedEntry
    {
        public StatId id;
        public object source;
        public float expireTime;
        public TimedEntry(StatId id, object source, float expireTime)
        { this.id = id; this.source = source; this.expireTime = expireTime; }
    }

    
    public CharacterStatusManager(CharacterStatusDef def)
    {
        _stats[StatId.HP] = new Status(def.HP);
        _stats[StatId.SP] = new Status(def.SP);
        _stats[StatId.ATK] = new Status(def.ATK);
        _stats[StatId.APS] = new Status(def.APS);
        _stats[StatId.DEF] = new Status(def.DEF);
        _stats[StatId.SPD] = new Status(def.SPD);

        CurrentHP = _stats[StatId.HP].Get();
        CurrentSP = _stats[StatId.SP].Get();
    }

    // API

    // view
    public float Get(StatId id) => _stats[id].Get();

    // Default setting
    public void SetDefault(StatId id, float value)
    {
        _stats[id].SetDefaultValue(value);
        ClampResourceIfNeeded(id);
    }

    // Modifier
    public void Add(StatId id, object source, float value)
    {
        _stats[id].AddModifier(source, value, StatusType.Add);
        ClampResourceIfNeeded(id);
    }
    public void Mul(StatId id, object source, float value)
    {
        _stats[id].AddModifier(source, value, StatusType.Multi);
        ClampResourceIfNeeded(id);
    }


    // Remove
    public void Remove(StatId id, object source)
    {
        _stats[id].RemoveModifier(source);
        _timed.RemoveAll(t => t.id == id && ReferenceEquals(t.source, source));
        ClampResourceIfNeeded(id);
    }
    public void RemoveBySource(object source)
    {
        foreach (var kv in _stats) kv.Value.RemoveModifier(source);
        _timed.RemoveAll(t => ReferenceEquals(t.source, source));
        ClampResourceIfNeeded(StatId.HP);
        ClampResourceIfNeeded(StatId.SP);
    }

    // Resource Control
    public void Heal(float value)
    {
        if (value <= 0f) return;
        CurrentHP = Mathf.Clamp(CurrentHP + value, 0f, Get(StatId.HP));
    }
    public void Damage(float value)
    {
        if (value <= 0f) return;
        CurrentHP = Mathf.Clamp(CurrentHP - value, 0f, Get(StatId.HP));
    }
    public void UseSP(float value)
    {
        if (value <= 0f) return;
        CurrentSP = Mathf.Clamp(CurrentSP - value, 0f, Get(StatId.SP));
    }
    public void GainSP(float value)
    {
        if (value <= 0f) return;
        CurrentSP = Mathf.Clamp(CurrentSP + value, 0f, Get(StatId.SP));
    }

    // Runtime Resource
    public void SetCurrentHP(float value)
    {
        CurrentHP = Mathf.Clamp(value, 0f, Get(StatId.HP));
    }

    public void SetCurrentSP(float value)
    {
        CurrentSP = Mathf.Clamp(value, 0f, Get(StatId.SP));
    }


    private void ClampResourceIfNeeded(StatId id)
    {
        if (id == StatId.HP) CurrentHP = Mathf.Clamp(CurrentHP, 0f, Get(StatId.HP));
        else if (id == StatId.SP) CurrentSP = Mathf.Clamp(CurrentSP, 0f, Get(StatId.SP));
    }
}