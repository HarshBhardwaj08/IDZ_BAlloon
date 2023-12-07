using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    public int LevelToLoad = 0;

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
