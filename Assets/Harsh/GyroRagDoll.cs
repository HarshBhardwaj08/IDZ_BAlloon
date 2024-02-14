using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroRagDoll : MonoBehaviour
{
    [SerializeField] private float speed = 5f; // Adjust based on your desired sensitivity
    [SerializeField] private bool useGravity = true; // Option to enable gyroscope-based gravity
    [SerializeField] private float gravityMultiplier = 9.81f; // Adjust gravity strength, if enabled

    private Vector3 initialRotation;
    public Rigidbody2D rb;

    void Start()
    {
       
        initialRotation = transform.rotation.eulerAngles;
        Input.gyro.enabled = true;
    }

    void FixedUpdate()
    {
        if (!Input.gyro.enabled)
        {
            Debug.LogWarning("Gyroscope not enabled on this device.");
            return;
        }

        // Get gyroscope rotation relative to initial rotation
        Vector3 gyroRotation = Input.gyro.attitude.eulerAngles - initialRotation;

        // Convert to Z-axis rotation for 2D movement (rotate around world up vector)
        float rotationZ = gyroRotation.y;

        // Apply rotation to horizontal movement
        rb.velocity = new Vector2(rotationZ * speed, rb.velocity.y);

        if (useGravity)
        {
            // Apply downward force based on tilt (simulate gravity)
            Vector2 gravityForce = Vector2.down * (gravityMultiplier * gyroRotation.x);
            rb.AddForce(gravityForce, ForceMode2D.Force);
        }
    }
}
