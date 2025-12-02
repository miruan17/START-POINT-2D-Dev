using UnityEngine;
using UnityEngine.EventSystems;

public class PanelClickDetector : MonoBehaviour, IPointerClickHandler
{
    public int now = 0;

    public void OnPointerClick(PointerEventData eventData)
    {
        // 패널이 클릭되면 UIFocusController에 보고
        UIFocusController.Instance.OnClicked(now);
    }
}
