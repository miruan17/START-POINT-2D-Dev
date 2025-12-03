using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePortal : MonoBehaviour
{
    [SerializeField]
    public Vector2 set;
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.GameStateManager.ChangeState(new StageState());
            GameManager.Instance.setPlayer(set);
        }
    }
}