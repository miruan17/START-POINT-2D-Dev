using System.Net;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class OkOrCancel : MonoBehaviour
{
    /*private AudioManager theAudio;
    public string key_sound;
    public string enter_sound;
    public string cancel_sound;*/

    public GameObject up_Panel;
    public GameObject down_Panel;

    public TextMeshProUGUI up_Text;
    public TextMeshProUGUI down_Text;

    public bool activated;
    private bool keyInput;
    private bool result = true;

    void Start()
    {
        //theAudio = FindObjectOfType<AudioManager>();
    }

    public void Selected()
    {
        //theAudio.Play(key_sound);
        result = !result;

        if (result)
        {
            up_Panel.gameObject.SetActive(false);
            down_Panel.gameObject.SetActive(true);
        }else
        {
            up_Panel.gameObject.SetActive(true);
            down_Panel.gameObject.SetActive(false);
        }
    }


    public void ShowTwoChoice(string _upText, string _downText)
    {
        activated = true;
        result = true;
        up_Text.text = _upText;
        down_Text.text = _downText;
        up_Panel.gameObject.SetActive(false);
        down_Panel.gameObject.SetActive(true);

        StartCoroutine(ShowTwoChoiceCoroutine());
    }

    public bool GetResult()
    {
        return result; 
    }

    IEnumerator ShowTwoChoiceCoroutine()
    {
        yield return new WaitForSeconds(0.01f);
        keyInput = true; 
    }

    void Update()
    {
        if (keyInput)
        {
            if(Input.GetKeyDown(KeyCode.DownArrow))
            { 
                Selected();
            }else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Selected();
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                //theAudio.Play(enter_sound);
                keyInput = false;
                activated = false;
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                //theAudio.Play(cancle_sound);
                keyInput = false;
                activated = false;
                result = false;
            }
        }
    }


}
