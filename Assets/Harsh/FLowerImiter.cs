using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FLowerImiter : MonoBehaviour
{
    int val;
    public List<GameObject> flowerPrefabs = new List<GameObject>();
    public int poolSize = 5;
    public GameObject parent;
    private List<GameObject> flowerPool;

    private void Start()
    {
        InitializeFlowerPool();
    }

    private void OnMouseDown()
    {
        Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        val = Random.Range(0, flowerPrefabs.Count);

      
        GameObject newFlower = GetPooledFlower();
        newFlower.transform.position = touchPosition;
        newFlower.SetActive(true);

       
        newFlower.GetComponent<Rigidbody2D>().gravityScale = 0;

       
        StartCoroutine(EnableGravityAfterDelay(newFlower.GetComponent<Rigidbody2D>()));
    }

    private void InitializeFlowerPool()
    {
        flowerPool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject flower = Instantiate(flowerPrefabs[val], Vector3.zero, Quaternion.identity,parent.transform);
            flower.SetActive(false);
            flowerPool.Add(flower);
        }
    }

    private GameObject GetPooledFlower()
    {
        foreach (GameObject flower in flowerPool)
        {
            if (!flower.activeInHierarchy)
            {
                return flower;
            }
        }

       
        GameObject newFlower = Instantiate(flowerPrefabs[val], Vector3.zero, Quaternion.identity, parent.transform);
        newFlower.SetActive(false);
        flowerPool.Add(newFlower);

        return newFlower;
    }

    IEnumerator EnableGravityAfterDelay(Rigidbody2D rb)
    {
        yield return new WaitForSeconds(5f);

        if (rb != null)
        {
            rb.gravityScale = 1;
        }
    }
}
