using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameStateManager GameStateManager { get; private set; }

    public StageRuntime CurrentStage { get; private set; }
    public RoomSceneManager RoomSceneManager { get; private set; }

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

        RoomSceneManager = new RoomSceneManager(this);
    }

    private void Start()
    {
        GameStateManager.ChangeState(new VillageState());
    }

    public void StartStage(string stageId, string startRoomId, string startRoomSceneName)
    {
        CurrentStage = new StageRuntime(stageId);
        RoomSceneManager.ChangeRoom(startRoomId, startRoomSceneName);
    }

    // 게임오버 시 세션 지우기
    public void ClearStage()
    {
        CurrentStage = null;
        // 필요하면 세이브/로드 시스템 연동도 여기서
    }
}

