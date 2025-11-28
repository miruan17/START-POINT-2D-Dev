using UnityEngine;
using UnityEngine.EventSystems;

public class DefaultSelect : MonoBehaviour
{
    [SerializeField] private GameObject firstSelected;

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(null);

        EventSystem.current.SetSelectedGameObject(firstSelected);
    }
}
