using System.Collections.Generic;
using System.Linq;

public struct StatContribution
{
    public float Additional;
    public float Multiple;
}

public class StatusManager
{
    private readonly Dictionary<StatusType, float> _base       = new();
    private readonly Dictionary<StatusType, float> _additional = new();
    private readonly Dictionary<StatusType, float> _multiple   = new();

    //Activated Modifier List by SourceID
    private readonly Dictionary<string, List<StatusModifier>> _bySourceId = new();
    
    //Activated Modifier List by kind
    private readonly Dictionary<StatusSourceKind, List<StatusModifier>> _byKind = new();

    public StatusManager()
    {
        InitZeros();
        foreach (StatusSourceKind k in System.Enum.GetValues(typeof(StatusSourceKind)))
            _byKind[k] = new List<StatusModifier>();
    }

    //Setting Base Status
    public void   SetBase(StatusType t, float v) => _base[t] = v;
    public float  GetBase(StatusType t)          => _base[t];
    public float  GetAdditional(StatusType t)    => _additional[t];
    public float  GetMultiple(StatusType t)      => _multiple[t];

    //Return Final (Base + Additional) * (1 + Multiple)
    public float GetFinal(StatusType t)
    {
        return (_base[t] + _additional[t]) * (1f + _multiple[t]);
    }

    //Add Modifier
    public void AddModifier(StatusModifier mod)
    {
        if (!_bySourceId.TryGetValue(mod.SourceId, out var list))
        {
            list = new List<StatusModifier>();
            _bySourceId[mod.SourceId] = list;
        }
        list.Add(mod);

        _byKind[mod.SourceKind].Add(mod);

        if (mod.Kind == StatusModKind.Additional)
            _additional[mod.Type] += mod.Value;
        else
            _multiple[mod.Type]   += mod.Value;
    }

    public bool RemoveBySource(string sourceId)
    {
        if (!_bySourceId.TryGetValue(sourceId, out var list)) return false;

        foreach (var mod in list)
        {
            if (mod.Kind == StatusModKind.Additional)
                _additional[mod.Type] -= mod.Value;
            else
                _multiple[mod.Type]   -= mod.Value;

            _byKind[mod.SourceKind].Remove(mod);
        }

        _bySourceId.Remove(sourceId);
        return true;
    }

    // ===== 조회/디버깅 유틸 =====
    // 특정 스탯에 대해 출처 종류별(스킬/증강/버프/…) 가산/배율 합계를 반환.
    public Dictionary<StatusSourceKind, StatContribution> GetContributionsByKind(StatusType stat)
    {
        var result = new Dictionary<StatusSourceKind, StatContribution>();
        foreach (StatusSourceKind k in System.Enum.GetValues(typeof(StatusSourceKind)))
        {
            float add = 0f, mul = 0f;
            foreach (var m in _byKind[k])
            {
                if (m.Type != stat) continue;
                if (m.Kind == StatusModKind.Additional) add += m.Value;
                else                                   mul += m.Value;
            }
            result[k] = new StatContribution { Additional = add, Multiple = mul };
        }
        return result;
    }

    // 활성 모디파이어 열람(필터 선택: 종류/스탯).
    public IEnumerable<StatusModifier> GetActiveModifiers(StatusSourceKind? kind = null, StatusType? type = null)
    {
        IEnumerable<StatusModifier> all = _byKind.Values.SelectMany(x => x);
        if (kind.HasValue) all = all.Where(m => m.SourceKind == kind.Value);
        if (type.HasValue) all = all.Where(m => m.Type == type.Value);
        return all;
    }

    // 특정 SourceId가 어떤 출처 종류인지 판별.
    public bool TryGetSourceKind(string sourceId, out StatusSourceKind kind)
    {
        if (_bySourceId.TryGetValue(sourceId, out var list) && list.Count > 0)
        {
            kind = list[0].SourceKind;
            return true;
        }
        kind = StatusSourceKind.Other;
        return false;
    }
    // Reset
    private void InitZeros()
    {
        foreach (StatusType t in System.Enum.GetValues(typeof(StatusType)))
        {
            _base[t]       = 0f;
            _additional[t] = 0f;
            _multiple[t]   = 0f;
        }
    }
}
