
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum SceneList
{
    Title,
    Village,
    Stage
}

public class SceneLoader
{
    Dictionary<SceneList, string> SceneList;
    public void Load(string SceneName)
    {
        
    }
    void Awake()
    {
        SceneManager.LoadSceneAsync("VillageScene",LoadSceneMode.Single);
    }
}
