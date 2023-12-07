using UnityEngine;

public class destroyChildren : MonoBehaviour
{
    //public PhysicsMaterial2D water;
    public void DestroyChild()
    {
        CreateWaterRadius.WaterRadius = 0;
        foreach (Transform child in transform)
        {
            Time.timeScale = 0;
            SpringJoint2D[] springs = child.GetComponents<SpringJoint2D>();
            Destroy(springs[1]);
            Destroy(springs[2]);
            Destroy(child.GetComponent<CreateJoints>());
            Destroy(child.GetComponent<PolygonCollider2D>());
            child.gameObject.AddComponent<CreateWaterRadius>();
            /*Destroy(child.GetComponent<CircleCollider2D>());
            Destroy(child.GetComponent<Rigidbody2D>());*/
        }
        /*Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
        rb.sharedMaterial = water;
        foreach (Transform child in transform)
        {
            CircleCollider2D circleCol = child.gameObject.AddComponent<CircleCollider2D>();
            circleCol.offset = new Vector2(0.15f, 0);
            circleCol.radius = .25f;
            circleCol.sharedMaterial=water;
        }*/
        Time.timeScale = 1;
    }

    public void increaseRadius()
    {
        CreateWaterRadius.WaterRadius += 0.05f;
        foreach (Transform child in transform)
        {
            child.GetComponent<CircleCollider2D>().radius = CreateWaterRadius.WaterRadius;
        }
    }
}
