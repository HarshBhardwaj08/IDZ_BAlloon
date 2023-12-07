using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll_MovementManager : MonoBehaviour
{
    public Transform head;
    public Transform body;

    public Rigidbody2D[] balancingBodies;

    public float forceToBalance;

    private Rigidbody2D bodyRB;
    private Vector2 positionToMove;

    private bool isUserTouching;

    // Start is called before the first frame update
    void Start()
    {
        isUserTouching = false;
        bodyRB = body.gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (LinesDrawer.Instance.penType == TypeOfPen.Destroy) return;

        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);

            if (touch.phase == TouchPhase.Began)
            {
                RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.zero);

                if(hit && (hit.transform.Equals(head) || hit.transform.Equals(body)))
                {
                    isUserTouching = true;
                }
            }
            else if((touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary) && isUserTouching)
            {
                positionToMove = Vector2.Lerp(bodyRB.position, touchPos, 0.5f);
                
            }
            else if(touch.phase == TouchPhase.Ended)
            {
                isUserTouching = false;
            }
        }
    }

    void FixedUpdate()
    {
        if(isUserTouching)
        {
            bodyRB.MovePosition(positionToMove);
        }
        else
        {
            bodyRB.MoveRotation(Mathf.LerpAngle(bodyRB.rotation, 0f, forceToBalance * Time.fixedDeltaTime));
            foreach (Rigidbody2D rb in balancingBodies)
            {
                rb.MoveRotation(Mathf.LerpAngle(rb.rotation, 0f, forceToBalance * Time.fixedDeltaTime));
            }
        }
    }
}
