using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameStateManager GameStateManager { get; private set; }

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

    public void CharacterInstantiate(GameObject prefab, Vector2 pos)
    {
        Instantiate(prefab, pos, Quaternion.identity);
    }
}

