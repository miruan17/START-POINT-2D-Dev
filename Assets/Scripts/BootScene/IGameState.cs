using Unity.Collections;
using UnityEngine.SceneManagement;

public interface IGameState
{
    public void GenerateScene();
    public void UnGenerateScene();
    public string Get();
}

public class TitleState : IGameState
{
    private string stateName="TitleState";

    public void GenerateScene()
    {
        SceneManager.LoadSceneAsync(stateName,LoadSceneMode.Additive);
    }
    public void UnGenerateScene()
    {
        SceneManager.UnloadSceneAsync(stateName);
    }
    public string Get() => stateName;
}

public class VillageState : IGameState
{
    private string stateName="VillageState";
    public void GenerateScene()
    {
        SceneManager.LoadSceneAsync(stateName,LoadSceneMode.Additive);
    }
    public void UnGenerateScene()
    {
        SceneManager.UnloadSceneAsync(stateName);
    }
    public string Get() => stateName;
}

public class StageState : IGameState
{
    private string stateName="StageState";
    private string[] scenelist;

    public StageState(string[] sceneList)
    {
        this.scenelist = sceneList;
    }

    // Stage Generator
    public void GenerateScene()
    {
        foreach(string x in scenelist)
        {
        SceneManager.LoadSceneAsync(x,LoadSceneMode.Additive);
        }
    }
    public void UnGenerateScene()
    {
        foreach(string x in scenelist)
        {
        SceneManager.UnloadSceneAsync(x);
        }
    }
    public string Get() => stateName;
}