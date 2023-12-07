using UnityEngine;
using UnityEngine.EventSystems;

public class touchmove : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    public int n = 0;

    public void OnPointerDown(PointerEventData eventData)
    {
        Vehicle.buttonpressed = n;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Vehicle.buttonpressed = 0;
    }
}
