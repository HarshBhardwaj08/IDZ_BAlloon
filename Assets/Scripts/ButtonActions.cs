using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonActions : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public enum Direction
    {
        LEFT, RIGHT, UP
    }

    public Direction moveDirection;

    public void OnPointerDown(PointerEventData eventData)
    {
        switch(moveDirection)
        {
            case Direction.LEFT:
                LinesDrawer.Instance.OnCar_LeftButtonDown();
                break;
            case Direction.RIGHT:
                LinesDrawer.Instance.OnCar_RightButtonDown();
                break;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        LinesDrawer.Instance.OnCar_MoveButtonUp();
    }
}
