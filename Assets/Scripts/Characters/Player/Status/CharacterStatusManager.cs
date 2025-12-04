using System;
using System.Collections.Generic;
using UnityEngine;
public enum StatId { HP, SP, ATK, APS, DEF, SPD, JP, AP }
public sealed class CharacterStatusManager  // Manager class
{
    // Public resource(readOnly)
    public float CurrentHP;
    public float CurrentSP;

    // Private resource
    private readonly Dictionary<StatId, Status> _stats = new();


    public CharacterStatusManager(CharacterStatusDef def)
    {
        _stats[StatId.HP] = new Status(def.HP);
        _stats[StatId.SP] = new Status(def.SP);
        _stats[StatId.ATK] = new Status(def.ATK);
        _stats[StatId.APS] = new Status(def.APS);
        _stats[StatId.DEF] = new Status(def.DEF);
        _stats[StatId.SPD] = new Status(def.SPD);
        _stats[StatId.JP] = new Status(def.JP);
        _stats[StatId.AP] = new Status(0);

        CurrentHP = _stats[StatId.HP].Get();
        CurrentSP = _stats[StatId.SP].Get();
    }

    // API

    // view
    public float GetFinal(StatId id) => _stats[id].Get();

    // Default setting
    public void SetDefault(StatId id, float value)
    {
        _stats[id].SetDefaultValue(value);
    }

    // Modifier
    public void Add(StatId id, String source, float value)
    {
        _stats[id].AddModifier(source, value, StatusType.Add);
    }
    public void Mul(StatId id, String source, float value)
    {
        _stats[id].AddModifier(source, value, StatusType.Multi);
    }
    public void Add(String id, String source, float value)
    {
        if (TryParseStatId(id, out StatId result))
            _stats[result].AddModifier(source, value, StatusType.Add);
    }
    public void Mul(String id, String source, float value)
    {
        if (TryParseStatId(id, out StatId result))
            _stats[result].AddModifier(source, value, StatusType.Multi);
    }


    // Remove
    public void Remove(StatId id, String source)
    {
        _stats[id].RemoveModifier(source);
    }
    public void RemoveBySource(String source)
    {
        foreach (var kv in _stats) kv.Value.RemoveModifier(source);
    }

    // string -> StatId convertor
    public static bool TryParseStatId(string value, out StatId result)
    {
        return Enum.TryParse(value, true, out result);
    }


}