using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Ballon : MonoBehaviour
{  
    Animator animator;
    Vector2 difference = Vector2.zero;
    public GameObject waterHolder;
    public GameObject Impactpoint;
    SpriteRenderer spriteRenderer;
   
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        waterHolder.SetActive(false);
        animator = GetComponent<Animator>();
        spriteRenderer.color =  new Color(Random.value,Random.value,Random.value,1.0f);
         
    }
   
    private void OnMouseDown()
    {
        difference = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition)- (Vector2)transform.position;

    }
    private void OnMouseDrag()
    {
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - difference;
    }
    private void OnMouseUp()
    {
        animator.SetTrigger("Burst");
    }

    public void BallonDestroy()
    {   
        Destroy(this.gameObject);
        waterHolder.transform.parent = null;
        waterHolder.SetActive(true);
       
    }

    
}
