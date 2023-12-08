using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundries : MonoBehaviour
{
    public GameObject wall;
    public float boundaryX = 10f; // Adjust this value based on your scene dimensions

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Wall")
        {   
          
            collision.gameObject.transform.localPosition = wall.transform.localPosition;
        }
       
        
    }

    
}
