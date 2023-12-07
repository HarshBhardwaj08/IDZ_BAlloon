using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float Power, foodtime;
    [SerializeField] Food currentsource;
    bool moving;
    float currentfoodtime;
    List<Food> CurrentFood = new List<Food>();
    void Start()
    {
        currentfoodtime = foodtime;
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Debug.Log("time':" + currentfoodtime);
        currentfoodtime -= Time.deltaTime;
        if (currentfoodtime <= 0)
        {
            StopAllCoroutines();
            moving = false;
            currentfoodtime = foodtime;
            CurrentFood.Add(currentsource); 
        }
        if (moving)
            return;
        Food[] allFood = FindObjectsOfType<Food>();
        if (allFood.Length > 0)
        {
            Food foundfood = allFood[0];
            float foundmin = Vector2.Distance(foundfood.transform.position, transform.position);

            foreach (Food food in allFood)
                if (Vector2.Distance(food.transform.position, transform.position) < foundmin && !CurrentFood.Contains(food))
                    foundfood = food;

            currentsource = foundfood;
            StartCoroutine(movetofood(currentsource));
        }
    }

    IEnumerator movetofood(Food obj)
    {
        moving = true;
        yield return null;
        Vector3 force; Vector2 foodloc;
        if (obj != null)
            foodloc = obj.transform.position;
        else
            yield break;
        foodloc.y = transform.position.y;
        force = foodloc - (Vector2)transform.position;
        force.y = 0;
        if (force.x > 0)
            force.x = 1;
        else if (force.x < 0)
            force.x = -1;
        force *= Power;
        //Mathf.Abs(transform.position.x - foodloc.x) > transform.localScale.x
        while (Vector2.Distance(transform.position, foodloc) > Mathf.Epsilon)
        {
            if (obj == null)
            {
                moving = false;
                yield break;
            }
            /*foodloc = obj.transform.position;
            foodloc.y = transform.position.y;
            transform.LookAt(foodloc);
            force = foodloc - (Vector2)transform.position;*/
            force.y = rb.velocity.y;
            rb.velocity = force;
            yield return null;
        }
        moving = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Food foodobj = collision.gameObject.GetComponent<Food>();
        if (foodobj != null)
        {
            Destroy(foodobj.gameObject);
            moving = false;
        }
    }
}
