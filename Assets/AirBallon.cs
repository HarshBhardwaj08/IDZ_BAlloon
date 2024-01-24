using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirBallon : MonoBehaviour
{
    Animator animator;
    SpriteRenderer spriteRenderer;
    public Transform colorParent;
    public List<GameObject> Splash = new List<GameObject>();
    Vector2 difference = Vector2.zero;
    Vector3 downDir;
    Vector3 upDir;
    Vector3 finalVector;
    public float floatSpeed = 0.1f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
      
        spriteRenderer.color = new Color(Random.value, Random.value, Random.value, 1.0f);
    }

    private void OnMouseDown()
    {
        difference = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;
        downDir = transform.position;
    }

    private void OnMouseDrag()
    {
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - difference;
    }
    private void OnMouseUp()
    {
        animator.SetTrigger("Burst");
    }
   

    private void Update()
    {
        float randomX = Mathf.Sin(Time.time) * floatSpeed;
        float randomY = Mathf.Cos(Time.time) * floatSpeed;
        transform.Translate(new Vector3(randomX, randomY, 0) * Time.deltaTime);
    }

    public void BalloonDestroy()
    {
        Destroy(this.gameObject);
        int randomSplash = Random.Range(0, Splash.Count);
        GameObject splash = Instantiate(Splash[randomSplash], transform.position, Quaternion.identity);
        splash.transform.parent = colorParent;
    }
}