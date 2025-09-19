using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SkillTree/SkillNodeDef")]
public class SkillNodeDef : ScriptableObject
{
    public string id;
    public string skillName;
    public Sprite icon;
    public int unlockCost = 1;
    public int cost = 1;
    public List<string> prerequisiteIds = new(); // IDs of nodes that must be unlocked first
    public bool isMainNode = false;              // optional: mark main vs sub
}
