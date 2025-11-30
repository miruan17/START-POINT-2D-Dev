using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour, ISubmitHandler
{
    public void OnSubmit(BaseEventData eventData)
    {
        Debug.Log($"{gameObject.name} Submitted");
        SceneManager.LoadSceneAsync("BootScene", LoadSceneMode.Single);
    }
}