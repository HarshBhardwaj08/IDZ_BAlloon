using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeSegment : MonoBehaviour
{
    public GameObject connectUp, connectDown;

    private void Start()
    {
        connectUp = GetComponent<HingeJoint2D>().connectedBody.gameObject;
        RopeSegment Upsegment = connectUp.GetComponent<RopeSegment>();
        if (Upsegment != null)
        {
            Upsegment.connectDown = gameObject;
            float spritebottom = connectUp.GetComponent<SpriteRenderer>().bounds.size.y;
            GetComponent<HingeJoint2D>().connectedAnchor = new Vector2(0, -spritebottom);
        }
        else
            GetComponent<HingeJoint2D>().connectedAnchor = Vector2.zero;
    }
}
