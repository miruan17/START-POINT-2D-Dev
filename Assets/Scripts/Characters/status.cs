using UnityEngine;

[System.Serializable]
public struct stat
{
    public float default_value;
    public float add_value;
    public float percent_value;
    public float final_value;

    public stat(float val)
    {
        default_value = val;
        add_value = 0f;
        percent_value = 100f;
        final_value = (default_value + add_value) * percent_value / 100f;
    }

    public void Recalculate()
    {
        final_value = (default_value + add_value) * percent_value / 100f;
    }

    public float Get() => final_value;
}

public enum StatType { Hp, Mp, Atk, Def, Spd, Ats }

public class status : MonoBehaviour
{
    [Header("Base Values")]
    public float hp = 100f;
    public float mp = 50f;
    public float atk = 10f;
    public float def = 10f;
    public float spd = 5f;
    public float ats = 1f;
    
}
