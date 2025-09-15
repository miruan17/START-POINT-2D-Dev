using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScreenOption : MonoBehaviour
{
    public TMP_Dropdown dropdown;

    public enum ScreenMode
    {
        FullScreenWindow = 0,
        Window = 1
    }

    private bool _inited;

    void Start()
    {
        if (dropdown == null)
        {
            Debug.LogError("[ScreenOption] TMP_Dropdown 할당 안됨");
            return;
        }

        // 옵션 세팅
        var options = new List<string> { "전체화면", "창모드" };
        dropdown.ClearOptions();
        dropdown.AddOptions(options);

        // 저장값 불러와 실제 모드 적용 (기본: 전체화면)
        int fsMode = PlayerPrefs.GetInt("pref.fs.mode", (int)FullScreenMode.FullScreenWindow);
        Screen.fullScreenMode = (FullScreenMode)fsMode;

        // UI 동기화 (알림 없이)
        int uiIndex = (Screen.fullScreenMode == FullScreenMode.Windowed)
            ? (int)ScreenMode.Window
            : (int)ScreenMode.FullScreenWindow;
        dropdown.SetValueWithoutNotify(uiIndex);

        // 리스너 중복 제거 후 등록
        dropdown.onValueChanged.RemoveAllListeners();
        dropdown.onValueChanged.AddListener(i => ChangeFullScreenMode((ScreenMode)i));

        _inited = true;
    }

    private void ChangeFullScreenMode(ScreenMode mode)
    {
        if (!_inited) return;

        switch (mode)
        {
            case ScreenMode.FullScreenWindow:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                PlayerPrefs.SetInt("pref.fs.mode", (int)FullScreenMode.FullScreenWindow);
                break;

            case ScreenMode.Window:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                PlayerPrefs.SetInt("pref.fs.mode", (int)FullScreenMode.Windowed);
                break;
        }

        PlayerPrefs.Save();
    }
}
