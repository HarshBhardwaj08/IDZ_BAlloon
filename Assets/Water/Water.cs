using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    public float ForceOnenter;
    public float ForceOnStay;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 4)
        {
            Debug.Log("OnCollisionEnter2D");
            GetComponent<Rigidbody2D>().AddForce(new Vector2(ForceOnenter, -ForceOnenter));
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(ForceOnenter, -ForceOnenter));
        }
    }

    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    if (collision.gameObject.layer == 4)
    //    {
    //        Debug.Log("OnCollisionStay2D");
    //        GetComponent<Rigidbody2D>().AddForce(new Vector2(ForceOnStay, -ForceOnStay));
    //        collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(ForceOnStay, -ForceOnStay));
    //    }
    //}

    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("OnCollisionExit2D");
        GetComponent<Rigidbody2D>().AddForce(new Vector2(ForceOnStay, ForceOnStay));
    }

}
