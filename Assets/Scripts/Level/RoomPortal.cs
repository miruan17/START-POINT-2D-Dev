using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPortal : MonoBehaviour
{
    [SerializeField]
    public Navigate navigate;
    public Vector2 set;
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.GameStateManager.GetCurrentState().Move(navigate);
            GameManager.Instance.setPlayer(set);
        }
    }
}
