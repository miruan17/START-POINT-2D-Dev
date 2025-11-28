using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomSceneManager
{
    private GameManager gameManager;
    private string currentRoomSceneName;

    public RoomSceneManager(GameManager gm)
    {
        this.gameManager = gm;
    }

    public void ChangeRoom(string nextRoomId, string nextRoomSceneName)
    {
        gameManager.StartCoroutine(ChangeRoomRoutine(nextRoomId, nextRoomSceneName));
    }

    private IEnumerator ChangeRoomRoutine(string nextRoomId, string nextRoomSceneName)
    {
        // 나중에: 현재 RoomController에서 SaveStateToStage 호출, 언로드 등 추가 예정
        if (!string.IsNullOrEmpty(currentRoomSceneName))
        {
            yield return SceneManager.UnloadSceneAsync(currentRoomSceneName);
        }

        var loadOp = SceneManager.LoadSceneAsync(nextRoomSceneName, LoadSceneMode.Additive);
        currentRoomSceneName = nextRoomSceneName;
        gameManager.CurrentStage.SetCurrentRoom(nextRoomId);

        while (!loadOp.isDone)
            yield return null;

        // 나중에: ApplyStateFromStage 호출 위치
    }

    public void UnloadCurrentRoom()
    {
        if (!string.IsNullOrEmpty(currentRoomSceneName))
        {
            SceneManager.UnloadSceneAsync(currentRoomSceneName);
            currentRoomSceneName = null;
        }
    }
}
