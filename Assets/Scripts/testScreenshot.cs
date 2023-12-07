using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using System.Collections;
using System.IO;

public class testScreenshot : MonoBehaviour
{
    public static testScreenshot instance;
    public Canvas can;
    public GameObject boundingbox;
    public string pathtofile;
    private void Start()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        // pathtofile = Application.dataPath + "/Resources/LevelIcon_";
    }

    [Button]
    public void TakeScreenshot()
    {
        StartCoroutine(CaptureScreen());
    }

    public IEnumerator CaptureScreen()
    {
        Time.timeScale = 0;
        boundingbox.SetActive(false);
        can.gameObject.SetActive(false);

        // Wait till the last possible moment before screen rendering to hide the UI
        yield return null;
        // Wait for screen rendering to complete
        yield return new WaitForEndOfFrame();

        // Take screenshot
#if UNITY_EDITOR
        pathtofile = Path.Combine(Application.persistentDataPath, $"LevelIcon_");
        pathtofile += DataManager.instance.LevelToLoad + ".png";
#else
        pathtofile = "LevelIcon_" + DataManager.instance.LevelToLoad + ".png";
#endif
        ScreenCapture.CaptureScreenshot(pathtofile);

        yield return new WaitForEndOfFrame();
        print("Saved screenshot to " + pathtofile);

        // Show UI after we're done
        can.gameObject.SetActive(true);
        boundingbox.SetActive(true);

        yield return null;

        Time.timeScale = 1;
    }
}
