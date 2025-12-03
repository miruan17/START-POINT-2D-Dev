using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    public static SceneLoadManager Instance { get; private set; }

    [Header("Player Reference")]
    [SerializeField] private Transform player;

    private string currentScene = "";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }


    public void LoadSceneToPosition(string sceneName, Vector2 targetPosition)
    {
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

    }
}
