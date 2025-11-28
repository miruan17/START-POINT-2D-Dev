using UnityEngine;
using UnityEngine.EventSystems;

public class StartButton : MonoBehaviour, ISubmitHandler
{
    public void OnSubmit(BaseEventData eventData)
    {
        Debug.Log($"{gameObject.name} Submitted");
        GameManager.Instance.GameStateManager.ChangeState(new VillageState());
    }
}