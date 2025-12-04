using UnityEngine;

[CreateAssetMenu(menuName = "Level/EnemyDefaultSettings")]
public class EnemyDefaultSettings : ScriptableObject
{
    public int enemyLevel;

    public GameObject prefab;
    public Vector2 spawnPoint;
}
