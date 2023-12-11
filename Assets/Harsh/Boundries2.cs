using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundries2 : MonoBehaviour
{
    public GameObject wall;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Wall")
        {
            StartCoroutine(delay(collision, 1.5f));
        }
    }

    IEnumerator delay(Collider2D collision, float time)
    {
        yield return new WaitForSeconds(time);
        MoveToWall(collision);
    }

    void MoveToWall(Collider2D collision)
    {
        Vector3 targetPos = collision.gameObject.transform.position;
        targetPos.x = wall.transform.position.x;
        collision.gameObject.transform.position = targetPos;
    }
}
