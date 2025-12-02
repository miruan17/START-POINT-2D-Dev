using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePortal : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.GameStateManager.ChangeState(new StageState());
        }
    }
}