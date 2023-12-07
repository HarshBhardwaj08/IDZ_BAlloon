using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundingBox3d : MonoBehaviour
{
    public float width = 4f, zdepth = 12f, zPosition = 0f,RightColliderAdjust=300f;
    public Transform topCollider, bottomCollider, leftCollider, rightCollider;

    Camera cam;
    Vector2 screenSize;
    Vector3 cameraPos;
    // Use this for initialization
    void Start()
    {
        cam = Camera.main;

        //Generate world space point information for position and scale calculations
        cameraPos = cam.transform.position;
        Vector2 origin = new Vector2(0, 0), ScreenEndPoints = new Vector2(Screen.width-RightColliderAdjust, 0);
        screenSize.x = Vector2.Distance(cam.ScreenToWorldPoint(origin), Camera.main.ScreenToWorldPoint(ScreenEndPoints)) * 0.5f;

        ScreenEndPoints.x = 0; ScreenEndPoints.y = Screen.height;
        screenSize.y = Vector2.Distance(cam.ScreenToWorldPoint(origin), Camera.main.ScreenToWorldPoint(ScreenEndPoints)) * 0.5f;

        //Change our scale and positions to match the edges of the screen...   
        Vector3 vertical = new Vector3(width, screenSize.y * 2, zdepth);
        Vector3 rightbox_pos = new Vector3(cameraPos.x + screenSize.x + (rightCollider.localScale.x * 0.5f), cameraPos.y, zPosition);
        rightCollider.localScale = vertical;
        rightCollider.position = rightbox_pos;
        leftCollider.localScale = vertical;
        rightbox_pos.x = -rightbox_pos.x;
        leftCollider.position = rightbox_pos;

        Vector3 horizontal = new Vector3(screenSize.x * 2, width, zdepth);
        Vector3 topbox_pos = new Vector3(cameraPos.x, cameraPos.y + screenSize.y + (topCollider.localScale.y * 0.5f), zPosition);
        topCollider.localScale = horizontal;
        topCollider.position = topbox_pos;
        bottomCollider.localScale = horizontal;
        topbox_pos.y = -topbox_pos.y;
        bottomCollider.position = topbox_pos;
    }
}
