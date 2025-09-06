using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;   // AudioMixer 사용

public class SoundOption : MonoBehaviour
{
    [Header("Audio Mixer 설정")]
    public AudioMixer gameMixer; 
    [Tooltip("AudioMixer에서 Expose한 파라미터 이름")]
    public string exposedParam = "MusicVolume";

    [Header("UI 슬라이더")]
    public Slider volumeSlider;

    void Start()
    {
        // 1) 슬라이더 범위 세팅 (0~1)
        volumeSlider.minValue = 0f;
        volumeSlider.maxValue = 1f;

        // 2) 현재 믹서 값(데시벨)을 읽어서 0~1 범위 값으로 바꿔 슬라이더에 반영
        float currentDb;
        if (gameMixer.GetFloat(exposedParam, out currentDb))
        {
            // dB → 0~1 값 변환: val = 10^(dB/20)
            volumeSlider.value = Mathf.Pow(10f, currentDb / 20f);
        }

        // 3) 슬라이더 값이 바뀔 때마다 볼륨 업데이트
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    void SetVolume(float sliderValue)
    {
        // 0~1 → dB 변환: dB = 20 * log10(val), 최소 볼륨은 -80dB 정도로 처리
        float dB = Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1f)) * 20f;
        gameMixer.SetFloat(exposedParam, dB);
    }
}
