using UnityEngine;

public class CreateWaterRadius : MonoBehaviour
{
    public static float WaterRadius = 0.25f;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.parent != transform.parent)
            if (collision.transform.parent.CompareTag("Water"))
            {
                print("Collided");
                Destroy(collision.transform.parent.gameObject);
                transform.parent.GetComponent<destroyChildren>().increaseRadius();
            }
    }
}
