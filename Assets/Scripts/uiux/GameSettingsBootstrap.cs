using UnityEngine;

public class GameSettingsBootstrap
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void ApplyEarly()
    {
        // 저장된 화면 모드만 '제일 먼저' 적용 (기본: 전체화면)
        int fsMode = PlayerPrefs.GetInt("pref.fs.mode", (int)FullScreenMode.FullScreenWindow);
        Screen.fullScreenMode = (FullScreenMode)fsMode;
    }
}
