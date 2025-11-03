using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubSkillNode : SkillNodeBase
{
    [SerializeField]
    public PlayerMove player;
    void Start()
    {

        //Debug.Log("player hp up: " + player.FinalHp);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
