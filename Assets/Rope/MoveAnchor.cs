using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class MoveAnchor : MonoBehaviour
{
    public bool canMove = true;
    private bool mouseDown = false;
    Vector2 position;

    void Update()
    {
        if (!canMove) return;

        if (Input.touchCount <= 0)
        {
            mouseDown = false;
            return;
        }

        position = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

        if (!mouseDown) {
            RaycastHit2D hit = Physics2D.CircleCast(position, 0.5f, Vector2.zero, 1f);

            if (hit && hit.transform == transform)
            {
                mouseDown = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (canMove && mouseDown)
        {
            Vector3 offset = new Vector3(position.x, position.y, 1f);
            transform.position = offset;
        }
    }
}
