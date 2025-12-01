using System.Collections.Generic;
using UnityEngine;

public sealed class SkillLib
{
    public Dictionary<string, Skill> skillMap = new Dictionary<string, Skill>();
    private static readonly SkillLib skillLib;

    // static 생성자에서 skillLib 인스턴스 생성
    static SkillLib()
    {
        skillLib = new SkillLib();
    }

    private SkillLib()
    {
        skillMap["Pollution"] = new Pollution();
        skillMap["FrostField"] = new FrostField();
        skillMap["Scar"] = new Scar();
        skillMap["Dash"] = new Dash();
        skillMap["FastDrop"] = new FastDrop();
    }

    public static SkillLib Instance => skillLib;

    public Skill getSkillbyID(string identifier)
    {
        if (skillMap.TryGetValue(identifier, out var skill))
        {
            return skill;
        }

        return null;
    }
}
