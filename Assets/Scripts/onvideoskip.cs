using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onvideoskip : MonoBehaviour
{
    public List<GameObject> tosetactive;

    static onvideoskip instance;

    private void Start()
    {
        if(instance)
        {
            task();
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void task()
    {
        foreach (var item in tosetactive)
            item.SetActive(true);
        gameObject.SetActive(false);
    }

    public void onvideoSkip()
    {
        task();
    }
}
