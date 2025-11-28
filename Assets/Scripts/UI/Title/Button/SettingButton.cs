using UnityEngine;
using UnityEngine.EventSystems;

public class SettingButton : MonoBehaviour, ISubmitHandler
{
    public void OnSubmit(BaseEventData eventData)
    {
        Debug.Log($"{gameObject.name} Submitted");
    }
}