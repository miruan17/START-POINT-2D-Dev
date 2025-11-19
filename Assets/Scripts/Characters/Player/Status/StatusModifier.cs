using System;

public enum StatusType { Add, Multi }

public class StatusModifier
{
    public float value;
    public StatusType type;
    public String source;
    public StatusModifier(String source, float value, StatusType type)
    {
        this.value = value;
        this.type = type;
        this.source = source;
    }

    public float Getvalue()
    {
        return value;
    }

    public object GetSource()
    {
        return source;
    }
}
