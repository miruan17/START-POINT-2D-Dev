using System;
using System.Collections.Generic;

public class Status
{
    private List<StatusModifier> modifiers;
    private bool isDirty = true;

    private float baseValue;
    private float additional;
    private float multiple;
    private float final;

    public Status(float baseValue)
    {
        SetDefaultValue(baseValue);
        modifiers = new List<StatusModifier>();
        Update();
    }

    public void Update()
    {
        additional = 0f;
        multiple = 1f;
        foreach (var mod in modifiers)
        {
            if (mod.type == StatusType.Add)
            {
                additional += mod.Getvalue();
            }
            else if (mod.type == StatusType.Multi)
            {
                multiple += mod.Getvalue();
            }
        }

        final = (baseValue + additional) * multiple;
        additional = Math.Max(0f, additional);
        multiple = Math.Max(0f, multiple);
        final = Math.Max(0f, final);

        isDirty = false;
    }

    public float Get()
    {
        if (isDirty)
            Update();

        return final;
    }

    // Modifier Management

    public void AddModifier(object source, float value, StatusType type)
    {
        if (value < 0f) value = 0f;

        modifiers.Add(new StatusModifier(source, value, type));
        isDirty = true;
    }

    public void RemoveModifier(object source)
    {
        modifiers.RemoveAll(m => m.source == source);
        isDirty = true;
    }

    public void SetDefaultValue(float value)
    {
        baseValue = Math.Max(0f, value);
        isDirty = true;
    }

    public void ClearModifiers()
    {
        modifiers.Clear();
        isDirty = true;
    }
    public float getBase()
    {
        return baseValue;
    }
}