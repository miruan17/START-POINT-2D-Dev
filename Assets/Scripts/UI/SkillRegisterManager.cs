using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillRegisterManager : MonoBehaviour
{

    public List<SkillNodeBase> unlockedSkillList = new();
    // Start is called before the first frame update
    public void updateUnlockedSkillList(List<SkillNodeBase> unlockedSkillList)
    {
        this.unlockedSkillList = unlockedSkillList;
        Debug.Log("list updated!");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
