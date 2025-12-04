using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;

    public AudioSource sfxSource;

    private void Awake()
    {
        Instance = this;
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.clip = clip;
        sfxSource.Play();
    }
}