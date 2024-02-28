using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tap : MonoBehaviour
{
    public GameObject waterPrefab;
    public GameObject fallpoint;
    private void OnMouseDrag()
    {
        GameObject water = Instantiate(waterPrefab, fallpoint.transform.position, Quaternion.identity);
    }
}
