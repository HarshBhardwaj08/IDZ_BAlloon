using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject Water;
    private List<GameObject> spawnList = new List<GameObject>();
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            position.z = 0;
            GameObject water = Instantiate(Water, position, Quaternion.identity, gameObject.transform);
            spawnList.Add(water);
        }
    }
    public void ClearScene()
    {
        if(spawnList.Count > 0)
        {
            foreach(GameObject spawn in spawnList)
            {
                Destroy(spawn);
            }
        }
        spawnList.Clear();
    }
}
