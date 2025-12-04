using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagePortal : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.GameStateManager.ChangeState(new StageState(1));
            GameManager.Instance.setPlayer(new Vector2(45,17));
        }
    }
}