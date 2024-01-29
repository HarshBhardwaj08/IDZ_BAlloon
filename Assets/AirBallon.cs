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
    Camera cam;
    float cameraHeight;
    Vector3 Screenpoint;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        cam = Camera.main;
      
    }
    private void Start()
    {
        float cameraHeight = cam.orthographicSize * 2f;
        float cameraWidth = cameraHeight * cam.aspect;
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
      
        float randomX = Mathf.PerlinNoise(Time.time, 0) * 2 - 1; 
        float randomY = Mathf.PerlinNoise(0, Time.time) * 2 - 1; 

        Vector3 randomDirection = new Vector3(randomX, randomY, 0).normalized; 
        Vector3 randomMovement = randomDirection * floatSpeed * Time.deltaTime;

        transform.Translate(randomMovement);
        ClampPositionToViewport();
    }
    private void ClampPositionToViewport()
{
    Vector3 clampedPosition = cam.WorldToViewportPoint(transform.position);
    clampedPosition.x = Mathf.Clamp01(clampedPosition.x);
    clampedPosition.y = Mathf.Clamp01(clampedPosition.y);

    transform.position = cam.ViewportToWorldPoint(clampedPosition);
}
    public void BalloonDestroy()
    {
        Destroy(this.gameObject);
        int randomSplash = Random.Range(0, Splash.Count);
        GameObject splash = Instantiate(Splash[randomSplash], transform.position, Quaternion.identity);
        splash.transform.parent = colorParent;
    }
}