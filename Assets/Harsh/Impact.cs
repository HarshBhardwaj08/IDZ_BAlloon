using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Impact : MonoBehaviour
{
    public float forcefeild;
    public float force;
    public LayerMask layerTohit;
    Collider2D[] objects;
    [SerializeField] float sec;
    
    private void Update()
    { 
        explode();
        Destroy(this.gameObject,sec);
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
