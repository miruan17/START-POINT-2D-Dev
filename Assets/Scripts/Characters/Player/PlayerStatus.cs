using UnityEngine;

[System.Serializable]
public struct Status
{
    public float default_value;
    public float add_value;
    public float percent_value;
    public float final_value;

    public Status(float val)
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

public class PlayerStatus : MonoBehaviour
{
    public static PlayerStatus Instance { get; private set; }

    [Header("Base Values (Inspector)")]
    public float hp = 100f;
    public float mp = 50f;
    public float atk = 10f;
    public float def = 10f;
    public float spd = 5f;
    public float ats = 1f;

    private Status HP, MP, ATK, DEF, SPD, ATS;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        HP  = new Status(hp);
        MP  = new Status(mp);
        ATK = new Status(atk);
        DEF = new Status(def);
        SPD = new Status(spd);
        ATS = new Status(ats);
    }

    private Status GetStatus(StatType t)
    {
        switch (t)
        {
            case StatType.Hp: return HP;
            case StatType.Mp: return MP;
            case StatType.Atk: return ATK;
            case StatType.Def: return DEF;
            case StatType.Spd: return SPD;
            case StatType.Ats: return ATS;
            default: return HP;
        }
    }

    private void SetStatus(StatType t, Status s)
    {
        switch (t)
        {
            case StatType.Hp:  HP = s; break;
            case StatType.Mp:  MP = s; break;
            case StatType.Atk: ATK = s; break;
            case StatType.Def: DEF = s; break;
            case StatType.Spd: SPD = s; break;
            case StatType.Ats: ATS = s; break;
        }
    }

    public static float Get(StatType t)
    {
        
        return Instance.GetStatus(t).Get();
    }
    public static void SetBase(StatType t, float value)
    {
        var s = Instance.GetStatus(t);
        s.default_value = value;
        s.Recalculate();
        Instance.SetStatus(t, s);
    }

    public static void Add(StatType t, float delta_value)
    {
        var s = Instance.GetStatus(t);
        s.add_value += delta_value;
        s.Recalculate();
        Instance.SetStatus(t, s);
    }

    public static void SetPercent(StatType t, float percent_value)
    {
        var s = Instance.GetStatus(t);
        s.percent_value = percent_value;
        s.Recalculate();
        Instance.SetStatus(t, s);
    }
}
