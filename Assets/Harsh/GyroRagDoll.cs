using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroRagDoll : MonoBehaviour
{
    [SerializeField] float jumpforce;
    [SerializeField] float gravity;
    [SerializeField] float diry;
    public float sensitivity = 10.0f;
    private Vector3 initialRotation;
    public Rigidbody2D rb2D;

    void Start()
    {
       
        initialRotation = transform.rotation.eulerAngles;
        Input.gyro.enabled = true;
    }
    private void Update()
    {
        float dirx = Input.acceleration.x * sensitivity * Time.deltaTime;
        float yMove = Input.acceleration.y * 4 * Time.deltaTime;
        float diry = -gravity * Time.deltaTime; // Gravity should be applied constantly.

        //  Debug.Log("dirx = " + dirx + " diry = " + diry);

        if (dirx > 0.02 || Input.GetKey(KeyCode.D))
        {
            rb2D.velocity = new Vector2(dirx*sensitivity ,rb2D.velocity.y);
        }
        else if (dirx < -0.02)
        {
            rb2D.velocity += new Vector2(dirx, yMove);
        }
        else
        {
            rb2D.velocity = new Vector2(0, rb2D.velocity.y); // Maintain vertical velocity, only update horizontal.
        }
    }
}
