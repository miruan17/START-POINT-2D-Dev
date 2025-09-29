public class StatusModifier
{
    public string SourceId;             //ModifierName
    public StatusSourceKind SourceKind; //ModifierType (SkillNode, Augment...)
    public StatusType Type;             //StatusType   (HP,SP...)
    public StatusModKind Kind;          //StatusModKing(Additional, Multiple)
    public float Value;                 //StatusValue

    //ModifierDef
    public StatusModifier(string sourceId, StatusSourceKind sourceKind,
                        StatusType type, StatusModKind kind, float value)
    {
        SourceId = sourceId;
        SourceKind = sourceKind;
        Type = type;
        Kind = kind;
        Value = value;
    }
}
