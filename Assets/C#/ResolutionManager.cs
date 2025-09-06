using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ResolutionManager : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;

    private readonly List<Resolution> resolutions = new List<Resolution>();
    private bool _inited;

    void Awake()
    {
        // 해상도 목록
        resolutions.Add(new Resolution { width = 1280, height = 720 });
        resolutions.Add(new Resolution { width = 1280, height = 800 });
        resolutions.Add(new Resolution { width = 1440, height = 900 });
        resolutions.Add(new Resolution { width = 1600, height = 900 });
        resolutions.Add(new Resolution { width = 1680, height = 1050 });
        resolutions.Add(new Resolution { width = 1920, height = 1080 });
        resolutions.Add(new Resolution { width = 1920, height = 1200 });
        resolutions.Add(new Resolution { width = 2048, height = 1280 });
        resolutions.Add(new Resolution { width = 2560, height = 1440 });
        resolutions.Add(new Resolution { width = 2560, height = 1600 });
        resolutions.Add(new Resolution { width = 2880, height = 1800 });
        resolutions.Add(new Resolution { width = 3840, height = 2160 });
    }

    void Start()
    {
        if (resolutionDropdown == null)
        {
            Debug.LogError("[ResolutionManager] TMP_Dropdown 할당 안됨");
            return;
        }

        // 옵션 문자열
        var options = new List<string>(resolutions.Count);
        for (int i = 0; i < resolutions.Count; i++)
            options.Add($"{resolutions[i].width} x {resolutions[i].height}");

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(options);

        // 저장값 불러오기 (인덱스 안전하게 보정)
        int savedIndex = PlayerPrefs.GetInt("pref.res.index", 0);
        savedIndex = Mathf.Clamp(savedIndex, 0, resolutions.Count - 1);

        // 실제 적용(드롭다운 이벤트 막고 값 동기화)
        ApplyResolution(savedIndex, save:false);
        resolutionDropdown.SetValueWithoutNotify(savedIndex);

        // 리스너 중복 방지 후 등록
        resolutionDropdown.onValueChanged.RemoveAllListeners();
        resolutionDropdown.onValueChanged.AddListener(OnDropdownChanged);

        _inited = true;
    }

    private void OnDropdownChanged(int idx)
    {
        if (!_inited) return;
        ApplyResolution(idx, save:true);
    }

    private void ApplyResolution(int idx, bool save)
    {
        idx = Mathf.Clamp(idx, 0, resolutions.Count - 1);
        var r = resolutions[idx];
        Screen.SetResolution(r.width, r.height, Screen.fullScreenMode);

        if (save)
        {
            PlayerPrefs.SetInt("pref.res.index", idx);
            PlayerPrefs.Save();
        }
    }
}
