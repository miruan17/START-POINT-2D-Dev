using UnityEngine;

public sealed class HitVFX
{
    private static readonly HitVFX instance = new HitVFX();

    public static HitVFX Instance => instance;

    // Prevent external instantiation
    private HitVFX() { }

    public GameObject spawnVFX;
}
