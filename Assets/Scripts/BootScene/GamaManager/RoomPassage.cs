using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level/RoomPassage")]
public class RoomPassage : ScriptableObject
{
    [Header("passage")]
    public Vector2 left;
    public Vector2 right;
    public Vector2 up;
    public Vector2 down;
    [Header("spawn")]
    public Vector2 spawn;
}
