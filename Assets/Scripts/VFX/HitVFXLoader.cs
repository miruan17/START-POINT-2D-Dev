using UnityEngine;

public class HitVFXLoader : MonoBehaviour
{
    public GameObject hitEffectPrefab;

    private void Awake()
    {
        HitVFX.Instance.spawnVFX = hitEffectPrefab;
    }
}
