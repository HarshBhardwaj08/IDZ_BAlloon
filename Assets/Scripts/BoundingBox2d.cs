using UnityEngine;

[ExecuteInEditMode]
public class BoundingBox2d : MonoBehaviour
{
    public Transform[] colliderBoxes; // [top, right, bottom, left]
    public float[] transformAdjust; // [topAdjust, rightAdjust, bottomAdjust, leftAdjust]

    private void Start()
    {
        UpdateColliderPositions();
    }

    private void FixedUpdate()
    {
        UpdateColliderPositions();
    }

    private void UpdateColliderPositions()
    {
        // Adjust TOP collider
        Vector3 topPosition = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 1f));
        topPosition.y = topPosition.y + transformAdjust[0] + colliderBoxes[0].localScale.y / 2;
        topPosition.z = 1;

        colliderBoxes[0].localPosition = topPosition;

        //Adjust RIGHT collider
        Vector3 rightPosition = Camera.main.ViewportToWorldPoint(new Vector2(1f, 0.5f));
        rightPosition.x = rightPosition.x + transformAdjust[1] + colliderBoxes[1].localScale.x / 2;
        rightPosition.z = 1;

        colliderBoxes[1].localPosition = rightPosition;

        //Adjust BOTTOM collider
        Vector3 bottomPosition = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 0f));
        bottomPosition.y = bottomPosition.y + transformAdjust[2] - colliderBoxes[2].localScale.y / 2;
        bottomPosition.z = 1;

        colliderBoxes[2].localPosition = bottomPosition;

        //Adjust LEFT collider
        Vector3 leftPosition = Camera.main.ViewportToWorldPoint(new Vector2(0f, 0.5f));
        leftPosition.x = leftPosition.x + transformAdjust[3] - colliderBoxes[3].localScale.x / 2;
        leftPosition.z = 1;

        colliderBoxes[3].localPosition = leftPosition;
    }
}
