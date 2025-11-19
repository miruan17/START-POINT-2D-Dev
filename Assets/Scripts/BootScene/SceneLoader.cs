
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    void Awake()
    {
        SceneManager.LoadSceneAsync("VillageScene",LoadSceneMode.Single);
    }
}
