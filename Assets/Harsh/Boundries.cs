using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundries : MonoBehaviour
{
    public GameObject wall;
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Wall")
        {
            Vector3 TargetPos = collision.gameObject.transform.position;
            TargetPos.x = wall.transform.position.x;
             collision.gameObject.transform.position = TargetPos;
        }
    }

}
