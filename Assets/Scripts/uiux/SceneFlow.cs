using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFlow : MonoBehaviour
{
    public static SceneFlow I { get; private set; }
    [SerializeField] string titleScene="Title";
    [SerializeField] string inGameScene="InGame";

    void Awake(){
        if (I!=null){ Destroy(gameObject); return; }
        I=this; DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDestroy(){ SceneManager.sceneLoaded -= OnSceneLoaded; }

    void OnSceneLoaded(Scene s, LoadSceneMode m){
        if (s.name==inGameScene) SaveManager.I?.AutoSave("sceneLoaded-InGame", 0);
    }

    public void LoadNewGame(){ SceneManager.LoadScene(inGameScene, LoadSceneMode.Single); }
    public void LoadContinue(){ SceneManager.LoadScene(inGameScene, LoadSceneMode.Single); }
    public void ReturnToTitle(){ SaveManager.I?.AutoSave("exitToTitle"); SceneManager.LoadScene(titleScene, LoadSceneMode.Single); }
    public void QuitGame(){
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying=false;
    #else
        Application.Quit();
    #endif
    }
}
