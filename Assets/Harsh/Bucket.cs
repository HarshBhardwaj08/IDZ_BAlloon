using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bucket : MonoBehaviour
{
    public float rotationSpeed = 100f;
    public GameObject waterPrefab; // Prefab of the water GameObject
    public GameObject fallpoint;
    private bool fall = true;
    private bool isRotating = false;
    float coolDowntime;
    Vector2 difference = Vector2.zero;
    Vector3 downDir;
    Vector3 upDir;
    float regerate = 0;
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            RotateCharacter(-rotationSpeed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            RotateCharacter(rotationSpeed);
        }
        if (Input.GetKey(KeyCode.Space))
        {  
            if(fall == true )
            {
            StartWaterFall();

            }
            coolDowntime += Time.deltaTime;
            if(coolDowntime >=3.0f )
            {
                fall = false;
                regerate = 0;
            }
        }
    }

    void RotateCharacter(float rotateAmount)
    {
        if (!isRotating)
        {
            Quaternion currentRotation = transform.rotation;
            float rotationAmount = rotateAmount * Time.deltaTime;
            transform.rotation = currentRotation * Quaternion.Euler(0f, 0f, -rotationAmount);
        }
    }

    void StartWaterFall()
    {
        GameObject water = Instantiate(waterPrefab, fallpoint.transform.position, Quaternion.identity);
        StartCoroutine(StopWaterFall(water));
    }

    IEnumerator StopWaterFall(GameObject waterObject)
    {
        yield return new WaitForSeconds(3f);
        if (waterObject != null)
        {
            Destroy(waterObject);
        }
    }
    private void OnMouseDrag()
    {
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - difference;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Water")
        {
            regerate += Time.deltaTime;
            Debug.Log(regerate);
            if (regerate >= 4)
            {
                fall = true;
                coolDowntime = 0;
            }
        }
    }
    private void OnMouseDown()
    {

        difference = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;
        downDir = transform.position;
    }
   
    private void OnMouseUp()
    {

        upDir = transform.position;
    }
}
