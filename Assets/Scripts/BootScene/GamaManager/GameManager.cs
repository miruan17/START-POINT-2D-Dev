using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameStateManager GameStateManager { get; private set; }
    public Dictionary<GameObject, int> enemies = new Dictionary<GameObject, int>();

    public GameObject player;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        GameStateManager = new GameStateManager();
        GameStateManager.parent = this;
    }

    private void Start()
    {
        GameStateManager.ChangeState(new VillageState());
    }
    public void setPlayer(Vector2 vector2)
    {
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        player.transform.position = vector2;
    }


    public void CharacterInstantiate(GameObject prefab, Vector2 pos, int id)
    {
        enemies[Instantiate(prefab, pos, Quaternion.identity)] = id;
    }
    public void clearEnemies()
    {
        enemies.Clear();
    }
}

