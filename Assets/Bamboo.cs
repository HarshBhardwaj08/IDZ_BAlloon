using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bamboo : MonoBehaviour
{
    [SerializeField]private Transform m_Transform;
    public List<GameObject>water = new List<GameObject>();
    private Animator animator;
    private bool isFlow;
    void Start()
    {
        animator = GetComponent<Animator>();
        isFlow = true;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool("Turn", true);

            StartCoroutine(waters());
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Water")
        {
            if (isFlow == true)
            {
                water.Add(collision.gameObject);
                collision.gameObject.SetActive(false);
            collision.gameObject.transform.parent = this.transform;
            }
        }
    }
    IEnumerator waters()
    {
        yield return new WaitForSeconds(2.0f);
        Fallen();
        water.Clear();
        yield return new WaitForSeconds(8.0f);
        animator.SetBool("Turn", false);
        isFlow = true;
    }
    public void Fallen()
    {
        for (int i = 0; i < water.Count; i++)
        {
            water[i].transform.position = m_Transform.position;
            water[i].gameObject.SetActive(true);
            water[i].gameObject.transform.parent = null;
            isFlow = false;
        }
        
       
    }
}
