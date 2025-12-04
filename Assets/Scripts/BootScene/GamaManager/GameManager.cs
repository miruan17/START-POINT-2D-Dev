using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameStateManager GameStateManager { get; private set; }
    public Dictionary<GameObject, int> enemies = new Dictionary<GameObject, int>();
    public AudioClip VillageBGM;
    public AudioClip StageBGM;
    public GameObject playerGameObject;
    private Player player;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        player = playerGameObject.GetComponent<Player>();
        GameStateManager = new GameStateManager();
        GameStateManager.parent = this;
    }

    private void Start()
    {
        GameStateManager.ChangeState(new VillageState());
    }
    public void setPlayer(Vector2 vector2)
    {
        Rigidbody2D rb = playerGameObject.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        playerGameObject.transform.position = vector2;
    }


    public void CharacterInstantiate(GameObject prefab, Vector2 pos, int id)
    {
        enemies[Instantiate(prefab, pos, Quaternion.identity)] = id;
    }
    public void clearEnemies()
    {
        enemies.Clear();
    }

    public Player getPlayer()
    {
        return player;
    }
    public void GetXp()
    {

        player.GetXp();
    }
}

