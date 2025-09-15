using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundOption : MonoBehaviour
{
    [Header("Mixer / Param / Slider")]
    public AudioMixer gameMixer;
    [Tooltip("AudioMixer에서 Expose한 파라미터 이름")]
    public string exposedParam = "MusicVolume";
    public Slider volumeSlider;

    [Header("중앙 피벗 매핑 설정")]
    [Range(0.1f, 0.9f)] public float pivot = 0.5f; 
    public float minDb = -40f;   
    public float boostDb = +6f;  
    private float baselineDb = 0f; 
    void Start()
    {
        
        volumeSlider.wholeNumbers = false;
        volumeSlider.minValue = 0f;
        volumeSlider.maxValue = 1f;

        
        if (!gameMixer.GetFloat(exposedParam, out baselineDb))
            baselineDb = 0f; 

        volumeSlider.onValueChanged.RemoveAllListeners();
        volumeSlider.value = pivot;
        volumeSlider.onValueChanged.AddListener(ApplyVolume);

        
        ApplyVolume(volumeSlider.value);
    }

    void ApplyVolume(float v)
    {
        v = Mathf.Clamp01(v);
        float dB;

        if (v < pivot)
        {
            float t = Mathf.InverseLerp(0f, pivot, v);   
            dB = Mathf.Lerp(minDb, baselineDb, t);
        }
        else
        {
            float t = Mathf.InverseLerp(pivot, 1f, v);  
            dB = Mathf.Lerp(baselineDb, baselineDb + boostDb, t);
        }

        gameMixer.SetFloat(exposedParam, dB);
    
    }
}
