using UnityEngine;
using UnityEngine.EventSystems;

public class QuitButton : MonoBehaviour, ISubmitHandler
{
    public void OnSubmit(BaseEventData eventData)
    {
        Debug.Log($"{gameObject.name} Submitted");
        #if UNITY_EDITOR
        // 에디터 플레이 중지
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        // 빌드된 게임 종료
        Application.Quit();
        #endif
    }
}