using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    [System.Serializable]
    public struct UI_Keys
    {
        public GameObject window;
        public KeyCode key;
    }
    public UI_Keys DefaultEntry;
    public List<UI_Keys> entries = new List<UI_Keys>();

    int current = -1;

    void Awake()
    {
        DefaultEntry.window.SetActive(true);
        for (int i = 0; i < entries.Count; i++)
            if (entries[i].window) entries[i].window.SetActive(false);
    }

    void Update()
    {
        if (current == -1)
        {
            for (int i = 0; i < entries.Count; i++)
            {
                if (entries[i].window && Input.GetKeyDown(entries[i].key))
                {
                    Open(i);
                    break;
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(entries[current].key))
            {
                Close();
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (entries[current].key == KeyCode.K)
                {
                    var controller = entries[current].window.GetComponent<UIFocusController>();
                    if (!controller.isEnter) Close();
                }
                else Close();
            }
        }
    }

    void Open(int i)
    {
        DefaultEntry.window.SetActive(false);
        if (i < 0 || i >= entries.Count || entries[i].window == null) return;
        entries[i].window.SetActive(true);
        current = i;
        Time.timeScale = 0f;
    }

    void Close()
    {
        DefaultEntry.window.SetActive(true);
        if (current == -1 || entries[current].window == null) return;
        entries[current].window.SetActive(false);
        current = -1;
        Time.timeScale = 1f;
    }
}
