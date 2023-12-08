using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Ballon : MonoBehaviour
{  
    Animator animator;
    Vector2 difference = Vector2.zero;
    Vector3 downDir;
    Vector3 upDir;
    Vector3 finalVector;
    public GameObject waterHolder;
    public GameObject Impactpoint;
    SpriteRenderer spriteRenderer;
    public float powerUp;
    public bool isPowerUp;
    Rigidbody2D rg2d;
    private void Awake()
    {
        rg2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        waterHolder.SetActive(false);
        animator = GetComponent<Animator>();
        spriteRenderer.color =  new Color(Random.value,Random.value,Random.value,1.0f);
         
    }
   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ballon")
        {
           
             if(isPowerUp == true)
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * powerUp, ForceMode2D.Impulse);
            }
           
        }
    }

    private void OnMouseDown()
    {   
        Vector3 finalVector = (downDir - upDir).normalized;
        isPowerUp = true;
        difference = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition)- (Vector2)transform.position;
        downDir = transform.position;
       
    }
    private void OnMouseDrag()
    {
        
        isPowerUp = true;
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - difference;
      
    }
    private void OnMouseUp()
    {
        
     
        animator.SetTrigger("Burst");
        isPowerUp = false;
        upDir = transform.position;
       
        finalVector = (upDir - downDir).normalized;
        Debug.Log(finalVector);
        rg2d.AddForce((finalVector) * powerUp, ForceMode2D.Impulse);
    }

    public void BallonDestroy()
    {   
        Destroy(this.gameObject);
       
        waterHolder.transform.parent = null;
        waterHolder.SetActive(true);
       
    }
    
    
}
