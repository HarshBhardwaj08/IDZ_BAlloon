using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    public GameObject waterPrefab; 
    public GameObject fallpoint;
    private void OnMouseDrag()
    {
        GameObject water = Instantiate(waterPrefab, fallpoint.transform.position, Quaternion.identity);
        StartCoroutine(StopWaterFall(water));
    }
    IEnumerator StopWaterFall(GameObject waterObject)
    {
        yield return new WaitForSeconds(3f);
        if (waterObject != null)
        {
            Destroy(waterObject);
        }
    }
}
