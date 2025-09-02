using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public bool isLeft;
    public SpriteRenderer spriter;

    SpriteRenderer player;

    Quaternion leftRot = Quaternion.Euler(0.23f,-0.45f, -0.01f);
    Quaternion leftRotReverse = Quaternion.Euler(0.23f, -0.45f, -100.01f);
    void Awake()
    {
        player = GetComponentsInParent<SpriteRenderer>()[1];

    }

    void LateUpdate()
    {
        bool isReverse = player.flipX;

        if (isLeft)
        {
            transform.localRotation = isReverse ? leftRotReverse : leftRot;
            spriter.flipY = isReverse;
            spriter.sortingOrder = isReverse ? 0 : 6;
        }
    }

}
