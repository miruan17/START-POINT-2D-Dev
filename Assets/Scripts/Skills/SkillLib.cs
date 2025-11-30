using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Unity.Collections;
using UnityEngine;

public sealed class SkillLib
{
    public Dictionary<string, Skill> skillMap = new Dictionary<string, Skill>();
    private static readonly SkillLib skillLib = new SkillLib();
    static SkillLib() { }
    private SkillLib()
    {
        skillMap["Pollution"] = new Pollution();
        skillMap["FrostField"] = new FrostField();
        skillMap["Scar"] = new Scar();
        skillMap["Dash"] = new Dash();
        skillMap["FastDrop"] = new FastDrop();
    }
    public static SkillLib Instance
    {
        get
        {
            return skillLib;
        }
    }
    public Skill getSkillbyID(String identifier)
    {
        if (skillMap.TryGetValue(identifier, out var skill))
            return skill;

        return null;
    }
}
