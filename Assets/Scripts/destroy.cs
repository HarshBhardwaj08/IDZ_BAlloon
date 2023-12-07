using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroy : MonoBehaviour
{
    private void OnCollisionStay2D(Collision2D collision)
    {
        /*if (collision.gameObject.name.Contains("Collide"))
            return;
        if (collision.gameObject.CompareTag("SoftBody"))
            Destroy(collision.transform.parent.gameObject);
        else
            Destroy(collision.gameObject);*/
    }
}
