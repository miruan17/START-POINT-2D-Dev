using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject SkillUIPanel;
    public GameObject InvectoryUIPanel;
    public GameObject SettingUIPanel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            SkillUIPanel.SetActive(!SkillUIPanel.activeSelf);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            SkillUIPanel.SetActive(!SkillUIPanel.activeSelf);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SkillUIPanel.SetActive(!SkillUIPanel.activeSelf);
        }
    }
}
