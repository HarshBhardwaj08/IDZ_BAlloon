using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public LayerMask DestroyLayer;
    private bool OnStart = false;
    void Start()
    {
        OnStart = true;
        // Debug.Log(child.GetComponent<CircleCollider2D>().composite);

        //yield return new WaitForSeconds(0.5f);
        //foreach (Transform child in transform)
        //{
        //    child.GetComponent<CircleCollider2D>().enabled = true;
        //}
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.25f, Vector2.zero, 1f, DestroyLayer);
        if(hit)
        {
            foreach (Transform child in transform)
            {
                child.GetComponent<CircleCollider2D>().enabled = false;
            }
            StartCoroutine(SetPosition(hit.collider.gameObject));
        }
        foreach (Transform child in transform)
        {
            child.GetComponent<CircleCollider2D>().enabled = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("ddOnCollisionEnter2D");
        if (OnStart)
        {
            Debug.Log("OnCollisionEnter2D");
            OnStart = false;
            foreach (Transform child in transform)
            {
                child.GetComponent<CircleCollider2D>().enabled = false;
            }
           // StartCoroutine(SetPosition(collision));
        }

    }
     IEnumerator SetPosition(GameObject collision)
    {
        Debug.Log("SetPosition");
        if (collision.transform.position.x > transform.position.x)
        {
            transform.position -= new Vector3(transform.position.x - transform.localScale.x, transform.position.y, 0);
        }
        else
        {
            transform.position += new Vector3(transform.position.x - transform.localScale.x, transform.position.y, 0);
        }
        yield return new WaitForEndOfFrame();
        Debug.Log("ASetPosition");
        foreach (Transform child in transform)
        {
            child.GetComponent<CircleCollider2D>().enabled = true;
        }
    }

}
