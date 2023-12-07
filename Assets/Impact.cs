using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Impact : MonoBehaviour
{
    public float forcefeild;
    public float force;
    public LayerMask layerTohit;
    public Collider2D[] objects;
    void Start()
    {
        explode();
    }

    void explode()
    {
       objects = Physics2D.OverlapCircleAll(transform.position, 3.0f, layerTohit);

        foreach (Collider2D obj in objects)
        {
            Vector2 direction = obj.transform.position - transform.position;
            obj.GetComponent<Rigidbody2D>().AddForce(direction * force);
        }
    }
}
