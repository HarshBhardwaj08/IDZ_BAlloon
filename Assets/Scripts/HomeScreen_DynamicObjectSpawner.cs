using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class HomeScreen_DynamicObjectSpawner : MonoBehaviour
{

    public int maxObjectsOnPool;
    public float timeIntervalForSpawn;

    public GameObject spawnPrefab;
    private Transform[] spawnBoneTransforms;

    private GameObject[] objectPool;

    private float spawnStartTime;
    private float killHeightForSpawnedObjects;

    // Start is called before the first frame update
    void Start()
    {
        spawnBoneTransforms = spawnPrefab.GetComponent<SpriteSkin>().boneTransforms;

        objectPool = new GameObject[maxObjectsOnPool];
        for(int i = 0; i < maxObjectsOnPool; i++)
        {
            objectPool[i] = Instantiate(spawnPrefab, transform);
            objectPool[i].SetActive(false);
        }

        killHeightForSpawnedObjects = -25f;
        spawnStartTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnStartTime >= timeIntervalForSpawn)
        {
            Color randomColor = Random.ColorHSV(0f, 1f, 0.1f, 0.6f, 0.5f, 1f);

            Vector3 spawnPosition = transform.position;
            bool safeSpawn = false;
            int retries = 30;

            while (!safeSpawn && retries > 0)
            {
                spawnPosition = Camera.main.ViewportToWorldPoint(
                    Random.Range(0f, 1f) > 0.5f ? new Vector2(Random.Range(0.1f, 0.45f), 1f) : new Vector2(Random.Range(0.55f, 0.9f), 1f)
                );
                spawnPosition.y = transform.position.y;
                spawnPosition.z = transform.position.z;
                safeSpawn = true;

                foreach (GameObject objectInPool in objectPool)
                {
                    if (objectInPool.activeSelf)
                    {
                        if (Vector3.Distance(spawnPosition, objectInPool.transform.position) < 2f * spawnPrefab.transform.localScale.x + Mathf.Epsilon)
                        {
                            safeSpawn = false;
                        }
                    }
                }
                retries--;
            }

            foreach(GameObject toSpawn in objectPool)
            {
                if (!safeSpawn) break;

                if (!toSpawn.activeSelf)
                {
                    GameObject spawnObject = toSpawn;

                    copyOriginalBoneTransforms(spawnObject);
                    spawnObject.transform.position = spawnPosition;

                    spawnObject.GetComponent<SpriteRenderer>().material.color = randomColor;
                    spawnObject.SetActive(true);

                    spawnStartTime = 0f;

                    break;
                }
            }
        }
        spawnStartTime += Time.deltaTime;

        foreach (GameObject spawnedObject in objectPool)
        {
            if (!spawnedObject.activeSelf) continue;

            SpriteSkin skin = spawnedObject.GetComponent<SpriteSkin>();

            if(skin.rootBone.localPosition.y < killHeightForSpawnedObjects)
            {
                spawnedObject.SetActive(false);
            }
        }
    }

    private GameObject copyOriginalBoneTransforms(GameObject toSpawn)
    {
        SpriteSkin skin = toSpawn.GetComponent<SpriteSkin>();
        Transform[] boneTransformsToChange = skin.boneTransforms;

        for(int i = 0; i < boneTransformsToChange.Length; i++)
        {
            boneTransformsToChange[i].localPosition = spawnBoneTransforms[i].localPosition;
            boneTransformsToChange[i].localRotation = spawnBoneTransforms[i].localRotation;
            boneTransformsToChange[i].localScale = spawnBoneTransforms[i].localScale;
        }

        return toSpawn;
    }
}
