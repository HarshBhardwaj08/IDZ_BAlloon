using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    public float MaxRadius = 4, Power;
    public List<GameObject> pulling;
    Vector3 force;

    LinesDrawer linesDrawer;

    void Start()
    {
        linesDrawer = FindObjectOfType<LinesDrawer>();
        pulling = new List<GameObject>();
    }

    void Update()
    {
        RaycastHit2D[] hit = Physics2D.CircleCastAll(transform.position, MaxRadius, Vector2.zero);
        if (hit != null && hit.Length > 0)
        {
            //pulling.Clear();
            foreach (RaycastHit2D hitobj in hit)
                if (!hitobj.transform.CompareTag("unmovable") && hitobj.transform.gameObject != gameObject && !hitobj.transform.name.Contains("Magnet"))
                {
                    if (pulling.Contains(hitobj.transform.gameObject))
                        return;
                    pulling.Add(hitobj.transform.gameObject);
                    StartCoroutine(pull(hitobj.transform.gameObject));                    
                }
        }
    }

    IEnumerator pull(GameObject obj)
    {
        yield return null;
        Vector2 force;
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        //bug, when the pull object is getting destroyed while pulling
        while (Vector2.Distance(transform.position, obj.transform.position) > Mathf.Epsilon)
        {
            force = transform.position - obj.transform.position;
            force *= Power;
            rb.AddForce(force);
            yield return null;
        }
    }
}
