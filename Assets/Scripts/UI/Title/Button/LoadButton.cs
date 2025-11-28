using UnityEngine;
using UnityEngine.EventSystems;

public class LoadButton : MonoBehaviour, ISubmitHandler
{
    public void OnSubmit(BaseEventData eventData)
    {
        Debug.Log($"{gameObject.name} Submitted");
    }
}