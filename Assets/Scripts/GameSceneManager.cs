using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager instance;
    [System.Serializable]
    struct ButtonDataHolder
    {
        public CategoryDataHolder.AvailableCategories CategoryType;
        public GameObject[] ButtonsToHide;
    }
    [SerializeField] ButtonDataHolder[] ButtonData;

    void Start()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        HideCategoryButtons();
    }

    private void HideCategoryButtons()
    {
        var _tempButtData = GetCategoryButtonData().Value;
        foreach (var v in _tempButtData.ButtonsToHide)
        {
            v.SetActive(false);
        }
    }

    ButtonDataHolder? GetCategoryButtonData()
    {
        foreach(var v in ButtonData)
        {
            if(v.CategoryType == CategoryDataHolder.Instance.ActiveCategory)
            {
                return v;
            }
        }
        return null;
    }

    public void backbuttonpressed(LinesDrawer lineDrawer)
    {
        StartCoroutine(backpressed(lineDrawer, "LevelSelection"));
    }

    public void dustbinpressed(LinesDrawer lineDrawer)
    {
        StartCoroutine(backpressed(lineDrawer, "GameScene"));
    }


    IEnumerator backpressed(LinesDrawer lineDrawer,string level)
    {
        yield return StartCoroutine(testScreenshot.instance.CaptureScreen());
        yield return null;
        yield return StartCoroutine(lineDrawer.savegameobject.SaveChildrenIenum());
        yield return null;
        SceneManager.LoadSceneAsync(level);
    }
}
