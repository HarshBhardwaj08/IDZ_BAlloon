using UnityEngine;
using UnityEngine.EventSystems;

public class ismouseOverui : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        LinesDrawer.Instance.isMouseOverUI = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        LinesDrawer.Instance.isMouseOverUI = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LinesDrawer.Instance.isMouseOverUI = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        LinesDrawer.Instance.isMouseOverUI = false;
    }
}
