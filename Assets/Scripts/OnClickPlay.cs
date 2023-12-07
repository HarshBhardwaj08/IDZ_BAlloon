using UnityEngine;
using UnityEngine.SceneManagement;

public class OnClickPlay : MonoBehaviour
{
    public int target_framerate = 65;

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = target_framerate;
    }

    public void onclickplay()
    {
        SceneManager.LoadSceneAsync("CategorySelectionScreen");
    }
}
