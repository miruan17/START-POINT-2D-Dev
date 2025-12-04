using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level/StageDefaultSettings")]
public class StageDefaultSettings : ScriptableObject
{
    [Header("ID")]
    public string stageName;
    public string sceneName;
    
    [Header("passage")]
    public Vector2 left;
    public Vector2 right;
    public Vector2 up;
    public Vector2 down;
    [Header("spawn")]
    public Vector2 spawn;

    [Header("Enemies")]
    public List<EnemyDefaultSettings> enemyList;
}
