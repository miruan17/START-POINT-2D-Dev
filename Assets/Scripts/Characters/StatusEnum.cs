using UnityEngine;

public enum StatusType
{
    Hp, Sp, Atk, Aps, Def, Spd
}
public enum StatusModKind
{
    Additional, Multiple
}
public enum StatusSourceKind
{
    SkillNode,      //스킬노드
    Augment,        //증강
    Buff,           //버프
    Item,           //장비
    Other           //그 외
}
