using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseableSkillPointTextUI : MonoBehaviour
{
    [SerializeField]
    SkillTreeManager manager;
    private Text useableSkillPoint;
    // Start is called before the first frame update
    void Start()
    {
        useableSkillPoint = GetComponent<Text>();
        manager.skillPointText = this;
        UpdateText();
    }
    public void UpdateText()
    {
        useableSkillPoint.text = "스킬 포인트: " + manager.useableSkillPoints.ToString();
    }

}
