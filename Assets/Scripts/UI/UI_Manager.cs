using System.Collections.Generic;
using UnityEngine;

public class UIShortcutWindows : MonoBehaviour
{
    [System.Serializable]
    public struct UI_Keys
    {
        public GameObject window;
        public KeyCode key;
    }

    public List<UI_Keys> entries = new List<UI_Keys>();

    int current = -1;

    void Awake()
    {
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
            if (Input.GetKeyDown(entries[current].key) || Input.GetKeyDown(KeyCode.Escape))
            {
                CloseCurrent();
            }
        }
    }

    void Open(int i)
    {
        if (i < 0 || i >= entries.Count || entries[i].window == null) return;
        entries[i].window.SetActive(true);
        current = i;
        Time.timeScale = 0f;
    }

    void CloseCurrent()
    {
        if (current == -1 || entries[current].window == null) return;
        entries[current].window.SetActive(false);
        current = -1;
        Time.timeScale = 1f;
    }
}
